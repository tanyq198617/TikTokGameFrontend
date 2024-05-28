using Sirenix.OdinInspector;
using UnityEngine;
using YooAsset;

/// <summary>
/// 启动配置，跟主包走
/// </summary>
public class BootConfig : MonoBehaviour
{
    [Header("资源服地址")] public string HostServerIP;
    
    [Header("游戏服地址")] public string GameServerIP;

    [Header("运行模式")]
    [DetailedInfoBox("运行模式 [点击查看]", "[Editor Simulate Mode = 编辑器模式]\n[Offline Play Mode = 本地流目录模式]\n[Host Play Mode = 网络资源模式]")]
    [OnValueChanged("PlayModeChanged")]
    public EPlayMode PlayMode;
    
    [Header("是否自动登录")] public bool AutoLogin;

    [Header("是否开启DEBUG输出")] public bool IsDebug;

    [OnValueChanged("NetDebugChanged")]
    [Header("是否开启网络层日志")] public bool IsNetDebug;

    [Header("是否本地服务器模式")] public bool IsLocalServerMode;

    [Header("编辑器平台模拟")] public SimulationType simulationType;

    [Header("登录方式")] public LoginType LoginType;

    public void SetHostPlayMode() 
    {
        AutoLogin = true;
        PlayMode = EPlayMode.HostPlayMode;
    }

    private void PlayModeChanged()
    {
#if UNITY_EDITOR
        DevelopSceneChange.ChangeDevelopScene();
#endif
    }

    private void NetDebugChanged()
    {
        EventMgr.Dispatch(nameof(NetDebugChanged));
    }

#if UNITY_EDITOR
    public UnityEditor.BuildTarget GetSimulationPlatform()
    {
        switch (simulationType)
        {
            case SimulationType.模拟Android: return UnityEditor.BuildTarget.Android;
            case SimulationType.模拟IOS: return UnityEditor.BuildTarget.iOS;
            case SimulationType.模拟PC: return UnityEditor.BuildTarget.StandaloneWindows64;
        }

        return UnityEditor.EditorUserBuildSettings.activeBuildTarget;
    }
#endif

    public enum SimulationType
    {
        不模拟,
        模拟PC,
        模拟Android,
        模拟IOS,
    }

}
