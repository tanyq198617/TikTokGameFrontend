using Cysharp.Threading.Tasks;
using HybridCLR;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

internal class DllMgr
{
    public const string Assembly_CSharp_dll = "Assembly-CSharp.dll";

    public const string Assembly_CSharp_bytes = "Assembly-CSharp.dll.bytes";

    public static string Assembly_CSharp_Path => $"{Const.AOTPath}/Assembly-CSharp.dll.bytes";

    private static readonly Dictionary<string, byte[]> assetDataDict = new Dictionary<string, byte[]>();

    /// <summary> 主包元数据 </summary>
    private static readonly List<string> aotMetaAssemblyList = new List<string>
    {
        "mscorlib.dll",
        "System.dll",
        "System.Core.dll",

        "UniTask.dll", //需要补充UniTask的AOT
        "CatJson.dll",
        "EventDispatcher.dll",
        "Nino.Serialization.dll",
        "Nino.Shared.dll",
        "MeshAnimator.dll",
        "Unity.Timeline.dll",
    };

    /// <summary> 热更元数据 </summary>
    private static readonly List<string> aotHotfixAssemblyList = new List<string>
    {
        //必须最后加载
        Assembly_CSharp_dll,
    };

    public static async UniTask Initialize() 
    {
        //加载元数据dll
        await DownLoadAssmbly();

        //补充AOT元数据
        LoadMetadataForAOTAssemblies();

#if !UNITY_EDITOR
        //加载热更数据
        await DownLoadHotFixAssmbly();
#endif
    }

    /// <summary> 记录补充元素的dll </summary>
    /// <returns></returns>
    private static async UniTask DownLoadAssmbly()
    {
        for (int i = 0; i < aotMetaAssemblyList.Count; i++)
        {
            var aot = aotMetaAssemblyList[i];
            var handle = YooAssets.LoadRawFileAsync(Const.GetAOTPath(aot));
            await handle.ToUniTask();
            var bytes = handle.GetRawFileData();
            assetDataDict.Add(aot, bytes);
        }
    }

    /// <summary> 热更程序集的加载和补充AOT的不一样 </summary>
    private static async UniTask DownLoadHotFixAssmbly()
    {
        //必须最后加载主库(Assembly-CSharp.dll)
        for (int i = 0; i < aotHotfixAssemblyList.Count; i++)
        {
            var aot = aotHotfixAssemblyList[i];
            var handle = YooAssets.LoadRawFileAsync(Const.GetAOTPath(aot));
            await handle.ToUniTask();
            var bytes = handle.GetRawFileData();
            System.Reflection.Assembly.Load(bytes);
        }
    }

    /// 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
    /// 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
    private static void LoadMetadataForAOTAssemblies()
    {
        HomologousImageMode mode = HomologousImageMode.SuperSet;
        foreach (var aotDllName in aotMetaAssemblyList)
        {
            byte[] dllBytes = assetDataDict[aotDllName];
            //byte[] dllBytes = BetterStreamingAssets.ReadAllBytes(aotDllName + ".bytes");
            // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
            // Debug.Log($"[程序集] LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
        }
    }
}
