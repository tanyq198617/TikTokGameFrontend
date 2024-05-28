#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using YooAsset;

/// <summary>
/// 编辑器运行模式
/// 格式：E:/Xuxinjie/HyBrid_HT/Bundles/Main/StandaloneWindows64/Simulate/PatchManifest_Main_Simulate.bytes
/// </summary>
internal class EditorSimulateRunMode : ARunMode
{
    private const string SimulatePatchManifestBytes = "PatchManifest_Main_Simulate.bytes";

    public override InitializeParameters GetInitializeParameters(ResourcePackage package, ServerVersion appVersion)
    {
        Debug.LogError($"<color=#FF33CC>[编辑器模式]</color>如果增减了配置资源, 则需要执行顶部工具栏：<color=#FF33CC>YooAsset --> 编辑器模式缓存清理</color>");
        var initParamenters = new EditorSimulateModeParameters();
        string defaultOutputRoot = Directory.GetParent(Application.dataPath).FullName;
        string SimulatePath = $"{defaultOutputRoot}/Bundles/{AssetConst.DefaultPackage}/{EditorUserBuildSettings.activeBuildTarget.ToString()}/Simulate/{SimulatePatchManifestBytes}";
        if (!File.Exists(SimulatePath))
            SimulatePath = EditorSimulateModeHelper.SimulateBuild(AssetConst.DefaultPackage);
        initParamenters.SimulateManifestFilePath = SimulatePath;
        return initParamenters;
    }
}
#endif
