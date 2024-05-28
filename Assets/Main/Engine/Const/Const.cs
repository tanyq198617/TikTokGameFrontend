using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

internal class Const
{
    public const int targetFrameRate = 60;  //游戏帧率
    
    public const bool IsEnableAddressable = false;

    public const string UIRoot = "UI Root";
    public const string HotfixRoot = "Assets/HotUpdateResources";
    public const string Prefab = "Prefabs";
    public const string Canvas = "Canvas";
    public const string AOTPath = HotfixRoot + "/Codes";
    public const string HotfixPath = HotfixRoot + "/" + Prefab + "/Item";
    public const string HotfixPrefab = "HotfixBoot";
    
    public const string Token = "-token=Qq/bSa3QCLydbrfFWDJ4/8btcfkCu9YC/9YhsGXT8/32E7CJ1l+rYCzRI7QANsKU13nBCO++UowU83V3f1ActUpV+w4YYGFWeHK7UeZrlQCYrD1Iyc5EkCbpqko=";
    public const string Appver = "1.1.1.0000";

    public static string GetPrefabPath(string name) => $"{Prefab}/{name}";
    public static string GetAOTPath(string aot) => IsEnableAddressable ? aot : $"{AOTPath}/{aot}";
    public static string GetHotfixPrefab() => IsEnableAddressable ? HotfixPrefab : $"{HotfixPath}/{HotfixPrefab}";

    #region Yooassets
    public const string DefaultYooFolderName = "yoo";
    public static string GetDefaultSandboxRoot()
    {
#if UNITY_EDITOR
        // 注意：为了方便调试查看，编辑器下把存储目录放到项目里。
        string projectPath = Path.GetDirectoryName(UnityEngine.Application.dataPath);
        projectPath = projectPath.Replace('\\', '/').Replace("\\", "/");
        return $"{projectPath}/{DefaultYooFolderName}";
#elif UNITY_STANDALONE
			return $"{UnityEngine.Application.dataPath}/{DefaultYooFolderName}";
#else
			return $"{UnityEngine.Application.persistentDataPath}/{DefaultYooFolderName}";
#endif
    }
    #endregion
}
