using System;
using System.IO;
using Cysharp.Threading.Tasks;
using HybridCLR;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;
using YooAsset;

/// <summary>
/// 游戏启动的根入口
/// </summary>
[RequireComponent(typeof(BootConfig))]
[RequireComponent(typeof(GameConfig))]
public class Boot : MonoBehaviour
{
    public static Boot Instance { get; private set; }
    public static BootConfig Config { get; private set; }
    public static GameConfig Game { get; private set; }
    public static bool IsEnableAddressable => Const.IsEnableAddressable;
    public static string[] CommandLineArgs;

    private CancellationTokenSource source;
    private bool needTryAgain = false;

    private void Awake()
    {
        Instance = this;
        Config = GetComponent<BootConfig>();
        Game = GetComponent<GameConfig>();
        BetterStreamingAssets.Initialize();
        UIRoot.InitRoot().Forget();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        // Application.targetFrameRate = Const.targetFrameRate;
        OnDemandRendering.renderFrameInterval = 2;
        Application.runInBackground = true;
        GameObject.DontDestroyOnLoad(this);
        Debug.unityLogger.logEnabled =
#if UNITY_EDITOR
            true;
#else
        Config.IsDebug;
#endif
        //传递的参数
        CommandLineArgs = Environment.GetCommandLineArgs();
    }

    public async void OnStart()
    {
        //初始YooAssets
        bool ok = await InitializeYooAssets();

        //初始化失败
        if (!ok) return;

        //下载完成后, 进行加载HybridCLR补充元数据
        await DllMgr.Initialize();

        //进行加载热更内容
        await StartGame();
    }

    private async UniTask StartGame()
    {
        //进行加载热更内容
        EventMgr.Dispatch(MainEvent.Boot_Message, $"开始进入场景.");
        var handler = YooAssets.LoadAssetAsync<GameObject>(Const.GetHotfixPrefab());
        await handler.ToUniTask();
        handler.InstantiateSync();
    }

    private async UniTask<bool> InitializeYooAssets()
    {
        //初始化本地配置
        await Game.SetConfigAsync();
        
        EventMgr.Dispatch(MainEvent.Boot_Message, $"准备初始化");
        
        //初始化资源加载器
        YooAssets.Initialize();

        //创建默认的资源包
        var package = YooAssets.CreatePackage(AssetConst.DefaultPackage);

        //设置包为默认的资源包
        YooAssets.SetDefaultPackage(package);
        
        EventMgr.Dispatch(MainEvent.Boot_Message, $"获取版本信息");

        //根据运行模式初始化配置
        bool isOk = await RunMode.InitializeAsync(package, Config);

        if (!isOk)
        {
            var appVersion = GameVersionHelper.AppVersion;
            if (appVersion == null)
            {
                Debug.LogError($"[APP] 程序版本检测异常! 请联系管理员");
                return false;
            }
            CheckDownLoadApp(appVersion, package);
            Debug.LogError($"[APP] 程序包异常, 请检查! 当前包版本：{Application.version}, 允许运行版本：{appVersion.GetValidVersionStr()}");
            return false;
        }

        //广播主包版本
        EventMgr.Dispatch(MainEvent.App_Version, Application.version, package.GetPackageVersion());

        EventMgr.Dispatch(MainEvent.Boot_Message, $"版本信息效验中...");

        //获取资源版本
        var operation = package.UpdatePackageVersionAsync();
        try
        {
            await operation.ToUniTask();
        }
        catch (Exception ex)
        {
            AlterView.Instance.Ok("更新失败", "获取资源版本失败!!", "退出");
            AlterView.Instance.onComplete = (ok) => Quit();
            return false;
        }

        if (operation.Status != EOperationStatus.Succeed)
        {
            AlterView.Instance.Ok("更新失败", "获取资源版本失败!!", "退出");
            AlterView.Instance.onComplete = (ok) => Quit();
            return false;
        }

        Debug.Log($"[Boot] 获取版本成功, Version={operation.PackageVersion}");

        //补丁清单
        string packageVersion = operation.PackageVersion;

        var manifestOperation = package.UpdatePackageManifestAsync(packageVersion);
        await manifestOperation.ToUniTask();

        if (manifestOperation.Status != EOperationStatus.Succeed)
        {
            AlterView.Instance.Ok("更新失败", "资源包更新失败!!", "退出");
            AlterView.Instance.onComplete = (ok) => Quit();
            return false;
        }

        //更新成功
        Debug.Log($"[Boot] 检查下载资源");
        EventMgr.Dispatch(MainEvent.Boot_Message, $"检查资源完整性...");
        bool result = await DownLoadResource();
        return result;
    }

    private void CheckDownLoadApp(ServerVersion appVersion, ResourcePackage package) 
    {
        var InAppDownLoad = appVersion.GetValue<bool>("InAppDownLoad");

        AlterView.Instance.OkAndCancel(
            "有新的安装包",
            $"发现新版本：v{appVersion.GetValidVersionStr()}", InAppDownLoad ? "点击下载" : "去下载",
            "关闭");

        source = new CancellationTokenSource();
        AlterView.Instance.onComplete = async ok =>
        {
            if (ok)
            {
                if (InAppDownLoad)
                    await AppDownLoadHelper.DownloadApp(appVersion.GetAppUrl(), OnDownloadAppStart, VersionView.Instance, OnDownloadAppResult, source);
                else
                    Application.OpenURL(appVersion.GetAppUrl());
            }
            else
                Quit();
        };
    }

    /// <summary>
    /// 下载资源
    /// </summary>
    /// <returns></returns>
    private UniTask<bool> DownLoadResource()
    {
        var package = YooAssets.GetPackage(AssetConst.DefaultPackage);
        var downloader = package.CreateResourceDownloader(AssetConst.DownLoadingMaxNum, AssetConst.FailedTryAgain, AssetConst.TimeOut);

        //没有需要下载的任务
        if (downloader.TotalDownloadCount == 0)
            return UniTask.FromResult(true);

        Debug.Log($"[Boot] 开始下载资源");

        int totalDownloadCount = downloader.TotalDownloadCount;
        long totalDownloadBytes = downloader.TotalDownloadBytes;

        UniTaskCompletionSource<bool> tcs = new UniTaskCompletionSource<bool>();

        //var tips = $"发现{totalDownloadCount}个资源有更新，总计需要下载 {totalDownloadBytes.ToSizeStr()}";
        //AlterView.Instance.OkAndCancel("提示", tips, "下载", "退出");
        //AlterView.Instance.onComplete = onComplete;
        onComplete(true);
        async void onComplete(bool ok)
        {
            if (!ok)
            {
                tcs.TrySetResult(false);
                Quit();
                return;
            }

            downloader.OnDownloadErrorCallback = OnDownLoadErrorCallback;
            downloader.OnDownloadProgressCallback = OnDownloadProgressCallback;
            downloader.OnDownloadOverCallback = OnDownloadOverCallback;
            downloader.OnStartDownloadFileCallback = OnStartDownloadFileCallback;

            EventMgr.Dispatch(MainEvent.Assets_Progress_Start);

            downloader.BeginDownload();

            try
            {
                await downloader.ToUniTask();
            }
            catch (Exception)
            {
                needTryAgain = true;
            }
            
            if (needTryAgain)
            {
                AlterView.Instance.OkAndCancel("下载错误", "部分文件下载失败了\n是否再次尝试？", "尝试", "退出");
                AlterView.Instance.onComplete = async (o) =>
                {
                    if (o)
                    {
                        bool oo = await DownLoadResource();
                        tcs.TrySetResult(oo);
                    }
                    else { Boot.Quit(); }
                };
                return;
            }

            if (downloader.Status != EOperationStatus.Succeed)
            {
                Debug.LogError($"【资源下载】资源下载失败!!!");
                EventMgr.Dispatch(MainEvent.Assets_Download_Failed);
            }
            else
            {
                EventMgr.Dispatch(MainEvent.Assets_Progress_End);
            }

            Debug.Log($"[Boot] 资源下载完毕");

            //广播新版本
            EventMgr.Dispatch(MainEvent.App_Version, Application.version, package.GetPackageVersion());

            tcs.TrySetResult(true);
        };

        return tcs.Task;
    }

    private void OnStartDownloadFileCallback(string fileName, long sizeBytes)
    {
        EventMgr.Dispatch(nameof(OnStartDownloadFileCallback), fileName, sizeBytes);
    }

    private void OnDownloadOverCallback(bool isSucceed)
    {
        EventMgr.Dispatch(nameof(OnDownloadOverCallback), isSucceed);
    }

    private void OnDownloadProgressCallback(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes)
    {
        EventMgr.Dispatch(nameof(OnDownloadProgressCallback), totalDownloadCount, currentDownloadCount, totalDownloadBytes, currentDownloadBytes);
    }

    private void OnDownLoadErrorCallback(string fileName, string error)
    {
        EventMgr.Dispatch(nameof(OnDownLoadErrorCallback), fileName, error);
    }

    private void OnDownloadAppStart() => EventMgr.Dispatch(MainEvent.Assets_Progress_Start);

    private void OnDownloadAppResult(bool ok, string result)
    {
        if (!ok) 
        {
            AlterView.Instance.Ok("提示", "安装包下载失败", "退出");
            AlterView.Instance.onComplete = (o)=> Quit();
            return;
        }

        AlterView.Instance.OkAndCancel("安装包下载完成", "是否进行安装？", "安装", "退出");
        AlterView.Instance.onComplete = (o) =>
        {
            if (!o) 
            {
                Quit();
                return;
            }
            ok = AppDownLoadHelper.InstallApp(result);
            if (!ok)
            {
                AlterView.Instance.OkAndCancel("安装失败", "是否跳转到下载链接手动下载安装？", "进行跳转", "退出");
                AlterView.Instance.onComplete = oo =>
                {
                    if (oo) Application.OpenURL(GameVersionHelper.AppVersion.GetAppUrl());
                    Quit();
                };
            }
            else
            {
                AlterView.Instance.Ok("安装提示", "安装成功！请重启！", "立即重启");
                AlterView.Instance.onComplete = (oo) => Quit();
            }
        };
    }

    public static void TryClear()
    {
        AlterView.Instance.OkAndCancel("提示", "确定清理缓存吗？\n此操作会导致重新下载资源。", "确定", "取消");
        AlterView.Instance.onComplete = (ok) =>
        {
            if (!ok) return;
            PlayerPrefs.DeleteAll();
            Directory.Delete(Const.GetDefaultSandboxRoot(), true);
            YooAssets.Destroy();
            GameConfig.Clear();
            //Instance.InitializeYooAssets().Forget();
        };
    }

    /// <summary>
    /// 获取配置
    /// 先从StreamingAssets查找
    /// 再从网络配置查找
    /// </summary>
    public static string GetValue(string key)
    {
        var val = Game.GetValue(key);
        return !string.IsNullOrEmpty(val) ? val : GameVersionHelper.GetValue(key);
    }
    
    public static T GetValue<T>(string key) where T : struct, IConvertible
    {
        return Game.GetValue<T>(key, out var val) ? val : GameVersionHelper.GetValue<T>(key);
    }

    public static string GetHostServerIP()
    {
        var url = Game.GetValue("HostServerIP");
        if (!string.IsNullOrEmpty(url))
        {
            url = url.EndsWith("/") ? url : $"{url}/";
            return url;
        }

        url = Config.HostServerIP;
        if (!string.IsNullOrEmpty(url))
        {
            url = url.EndsWith("/") ? url : $"{url}/";
            return url;
        }
        
        Debuger.LogFatal($"[BOOT] 不存在IP地址");
        return string.Empty;
    }

    /// <summary>
    /// 获得Token
    /// </summary>
    public static string GetToKen()
    {
        string token = string.Empty;
        //如果强制读本地token
        var isLocal = GetValue<bool>("IsLocalToken");
#if UNITY_EDITOR
        if (true)
#else
        if (isLocal)
#endif
        {
            token = Game.GetValue("Token");
            if (string.IsNullOrEmpty(token))
            {
                Debuger.LogError($"Token为空,尝试获取默认测试Token");
                return Const.Token;
            }
            return token;
        }

        //获取传递的token
        for (int i = 0; i < CommandLineArgs.Length; i++)
        {
            if (CommandLineArgs[i].Contains("token"))
            {
                token = CommandLineArgs[i];
                break;
            }
        }
        return token;
    }

    /// <summary>
    /// 获得渠道
    /// </summary>
    public static string GetChannel()
    {
#if UNITY_EDITOR
        return "fastlogin";
#else
        var isLocal = GetValue<bool>("IsLocalToken");
        return isLocal ? "fastlogin" : GetValue("Channel");
#endif
    }
    
    /// <summary>
    /// 获得版本
    /// </summary>
    public static string GetAppver(string key)
    {
        var val = Game.GetValue(key);
        if (!string.IsNullOrEmpty(val))
            return val;
        val = GameVersionHelper.GetValue(key);
        if (!string.IsNullOrEmpty(val))
            return val;
        return Const.Appver;
    }

    public static bool IsNetLog()
    {
#if UNITY_EDITOR
        return Config.IsNetDebug;
#else
        return "1".Equals(GetValue("IsNetLog"));
#endif
    }


#if UNITY_EDITOR
    public UnityEditor.BuildTarget GetSimulationPlatform()
    {
        return Config.GetSimulationPlatform();
    }
#endif

    public static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
