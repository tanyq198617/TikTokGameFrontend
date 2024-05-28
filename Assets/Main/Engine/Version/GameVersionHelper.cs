using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

/// <summary>
/// HTTP资源服配置版本
/// </summary>
public class ServerVersion
{
    private readonly Dictionary<string, string> keyVal = new Dictionary<string, string>();
    private readonly Dictionary<string, object> objVal = new Dictionary<string, object>();
    
    public void ParasConfig(string text)
    {
        keyVal.Clear();
        objVal.Clear();
        var strArr = text.Split('\n');
        for (int i = 0; i < strArr.Length; i++)
        {
            var line = strArr[i];
            if (line.Contains("="))
            {
                string[] kv = line.Split('=');
                string key = kv[0].Trim();
                var v = kv[1].Contains("#") ? kv[1].Substring(0, kv[1].IndexOf("#", StringComparison.Ordinal)) : kv[1];
                string value = v.Trim();
                keyVal.Add(key, value);
            }
        }
    }
    
    public string GetValue(string key, string defaultValue = "") => keyVal.TryGetValue(key, out string value) ? value : defaultValue;

    public T GetValue<T>(string key, T defaultValue = default) where T : struct, IConvertible
    {
        if (!objVal.TryGetValue(key, out var obj))
        {
            var value = GetValue(key);
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            try
            {
                obj = Convert.ChangeType(value, typeof(T));
                objVal[key] = obj;
            }
            catch (Exception e)
            {
                Debuger.LogFatal($"配置解析错误：{value}\n{e}");
            }
        }
        return (T)obj;
    } 
    
    private int GetValidAndroidVersion() => int.Parse(GetValue("AndroidVersion").Replace(".", ""));
    private int GetValidIOSVersion() => int.Parse(GetValue("IOSVersion").Replace(".", ""));
    private int GetValidPCVersion() => int.Parse(GetValue("PCVersion").Replace(".", ""));
    private int GetCurrentVersion() => int.Parse(Application.version.Replace(".", ""));

    private bool ValidAndroidVersion() => GetCurrentVersion() >= GetValidAndroidVersion();
    private bool ValidIOSVersion() => GetCurrentVersion() >= GetValidIOSVersion();
    private bool ValidPCVersion() => GetCurrentVersion() >= GetValidPCVersion();

    public bool ValidVersion()
    {
#if UNITY_EDITOR
        var activeBuildTarget = Boot.Instance.GetSimulationPlatform();
        if (activeBuildTarget == UnityEditor.BuildTarget.iOS)
            return ValidIOSVersion();
        else if (activeBuildTarget == UnityEditor.BuildTarget.Android)
            return ValidAndroidVersion();
        else
            return ValidPCVersion();
#else
        if (Application.platform == RuntimePlatform.IPhonePlayer)
			return ValidIOSVersion();
        else if (Application.platform == RuntimePlatform.Android)
			return ValidAndroidVersion();
		else
			return ValidPCVersion();
#endif
    }

    public string GetValidVersionStr()
    {
#if UNITY_EDITOR
        var activeBuildTarget = Boot.Instance.GetSimulationPlatform();
        if (activeBuildTarget == UnityEditor.BuildTarget.iOS)
            return GetValue("IOSVersion");
        else if (activeBuildTarget == UnityEditor.BuildTarget.Android)
            return GetValue("AndroidVersion");
        else
            return GetValue("PCVersion");
#else
        if (Application.platform == RuntimePlatform.IPhonePlayer)
			return GetValue("IOSVersion");
        else if (Application.platform == RuntimePlatform.Android)
			return GetValue("AndroidVersion");
		else
			return GetValue("PCVersion");
#endif
    }
    
    public int GetValidVersion()
    {
#if UNITY_EDITOR
        var activeBuildTarget = Boot.Instance.GetSimulationPlatform();
        if (activeBuildTarget == UnityEditor.BuildTarget.iOS)
            return GetValidIOSVersion();
        else if (activeBuildTarget == UnityEditor.BuildTarget.Android)
            return GetValidAndroidVersion();
        else
            return GetValidPCVersion();
#else
        if (Application.platform == RuntimePlatform.IPhonePlayer)
			return GetValidIOSVersion();
        else if (Application.platform == RuntimePlatform.Android)
			return GetValidAndroidVersion();
		else
			return GetValidPCVersion();
#endif
    }

    public string GetAppUrl()
    {

#if UNITY_EDITOR
        var activeBuildTarget = Boot.Instance.GetSimulationPlatform();
        if (activeBuildTarget == UnityEditor.BuildTarget.iOS)
            return GetValue("IOSUrl");
        else if (activeBuildTarget == UnityEditor.BuildTarget.Android)
            return GetValue("AndroidApkUrl");
        else
            return GetValue("PCUrl");
#else
        if (Application.platform == RuntimePlatform.IPhonePlayer)
			return GetValue("IOSUrl");
        else if (Application.platform == RuntimePlatform.Android)
            return GetValue("AndroidApkUrl");
		else
			return GetValue("PCUrl");
#endif
    }
    
    public string GetResVersion(string package)
    {
#if UNITY_EDITOR
        var activeBuildTarget = Boot.Instance.GetSimulationPlatform();
        if (activeBuildTarget == UnityEditor.BuildTarget.iOS)
            return Boot.GetValue($"IOS{package}Version");
        else if (activeBuildTarget == UnityEditor.BuildTarget.Android)
            return Boot.GetValue($"Android{package}Version");
        else
            return Boot.GetValue($"PC{package}Version");
#else
        if (Application.platform == RuntimePlatform.IPhonePlayer)
			return Boot.GetValue($"IOS{package}Version");
        else if (Application.platform == RuntimePlatform.Android)
			return Boot.GetValue($"Android{package}Version");
		else
			return Boot.GetValue($"PC{package}Version");
#endif
    }
}

/// <summary>
/// 游戏app冷更包版本
/// </summary>
public class GameVersionHelper
{
    public static ServerVersion AppVersion { get; private set; } = null;

    public static async UniTask<ServerVersion> GetServerVersion(string baseURL)
    {
        string json = await HttpUtility.HttpGetAsync($"{baseURL}config.txt");
        if (string.IsNullOrEmpty(json))
            return null;
        AppVersion ??= new ServerVersion();
        AppVersion.ParasConfig(json);
        return AppVersion;
    }

    public static UniTask<ServerVersion> CheckAppVersion(EPlayMode playMode, string baseURL)
    {
        return playMode != EPlayMode.HostPlayMode ? UniTask.FromResult(AppVersion) : GetServerVersion(baseURL);
    }
    
    public static string GetValue(string key, string defaultValue = "") => AppVersion?.GetValue(key, defaultValue) ?? defaultValue;
    public static T GetValue<T>(string key, T defaultValue = default) where T : struct, IConvertible => AppVersion?.GetValue<T>(key, defaultValue) ?? defaultValue;
    
}
