using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using YooAsset;

internal class RunMode 
{
    public static async UniTask<bool> InitializeAsync(ResourcePackage package, BootConfig config)
    {
        ARunMode runMode = GetRunMode(config.PlayMode);
        var url = Boot.GetHostServerIP();
        var appVersion = await GameVersionHelper.CheckAppVersion(config.PlayMode, url);

#if UNITY_EDITOR
        Debug.LogError($"请求服务器地址:{url}");
        if (appVersion != null && !appVersion.ValidVersion())
            return false;
#else
        if (appVersion == null || !appVersion.ValidVersion())
            return false;
#endif

        var initializeParameters = runMode.GetInitializeParameters(package, appVersion);
        await package.InitializeAsync(initializeParameters);
        return true;
    }

    private static ARunMode GetRunMode(EPlayMode playMode) 
    {
        switch (playMode)
        {
#if UNITY_EDITOR
            case EPlayMode.EditorSimulateMode: return new EditorRunMode();
#endif
            case EPlayMode.OfflinePlayMode: return new OfflineRunMode();
            case EPlayMode.HostPlayMode: return new HostPlayRunMode();
        }

        Debuger.LogError($"不存在的运行模式：{playMode.ToString()}");
        return null;
    }


    /// <summary>
    /// 获取默认资源服务器地址
    /// </summary>
    public static string GetDefaultHostServer(string version)
    {
        string server = Boot.GetHostServerIP();
        server = server.EndsWith("/") ? server.Substring(0, server.Length - 1) : server;

#if UNITY_EDITOR
        if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
            return $"{server}/android/{version}";
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
            return $"{server}/ios/{version}";
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
            return $"{server}/WebGL/{version}";
        else
            return $"{server}/PC/{version}";
#else
		if (Application.platform == RuntimePlatform.Android)
			return $"{server}/android/{version}";
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
			return $"{server}/ios/{version}";
		else if (Application.platform == RuntimePlatform.WebGLPlayer)
			return $"{server}/WebGL/{version}";
		else
			return $"{server}/PC/{version}";
#endif
    }
    
    /// <summary>
    /// 获取默认资源服务器地址
    /// </summary>
    public static RuntimePlatform GetRuntimePlatform()
    {
#if UNITY_EDITOR
        if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
            return RuntimePlatform.Android;
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
            return RuntimePlatform.IPhonePlayer;
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
            return RuntimePlatform.WebGLPlayer;
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.StandaloneWindows)
            return RuntimePlatform.WindowsPlayer;
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.StandaloneWindows64)
            return RuntimePlatform.WindowsPlayer;
        else
            return RuntimePlatform.WindowsEditor;
#else
        return Application.platform;
#endif
    }
}
