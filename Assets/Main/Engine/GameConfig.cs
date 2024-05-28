using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class GameConfig : MonoBehaviour
{
    private readonly Dictionary<string, object> objVal = new Dictionary<string, object>();
    public readonly Dictionary<string, string> keyVal = new Dictionary<string, string>();

    private const string ConfigName = "GameConfig.ini";
    
    public async UniTask SetConfigAsync() 
    {
        var configPath = Path.Combine(GetDataPath(), ConfigName);
        if (!File.Exists(configPath))
        {
            await CoDownloadFile(configPath).ToUniTask();
            return;
        }
        ParasConfig(configPath);
    }

    IEnumerator CoDownloadFile(string savePath)
    {
        MakeSureCanSaveToPath(savePath);
        string url = Path.Combine(Application.streamingAssetsPath, ConfigName);
        var downloadRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET, new DownloadHandlerFile(savePath), null);

        yield return downloadRequest.SendWebRequest();
        if (downloadRequest.IsError())
        {
            Debug.LogErrorFormat("Download File Failed!!url = {0}, savePath = {1}， err = {2}", url, savePath, downloadRequest.error);
            File.Delete(savePath);
            yield break;
        }
        downloadRequest.Dispose();
        ParasConfig(savePath);
    }

    private void ParasConfig(string path)
    {
        if (!File.Exists(path)) return;
        using var sr = new StreamReader(path, Encoding.Default);
        while (sr.ReadLine() is { } line)
        {
            if (line.Contains("="))
            {
                line = line.Contains("#") ? line.Substring(0, line.IndexOf("#", StringComparison.Ordinal)).Trim() : line;
                if(string.IsNullOrEmpty(line))
                    continue;
                string[] kv = line.Split('=');
                if (kv.Length < 2) continue;
                string key = kv[0].Trim();
                string v = kv[1].Trim();
                keyVal.Add(key, v);
            }
        }
    }

    public string GetValue(string key, string defaultValue = "")
    {
        return keyVal.TryGetValue(key, out string value) ? value : defaultValue;
    }
    
    public bool GetValue<T>(string key, out T result, T defaultValue = default) where T : struct, IConvertible
    {
        if (!objVal.TryGetValue(key, out var obj))
        {
            var value = GetValue(key);
            if (string.IsNullOrEmpty(value))
            {
                result = defaultValue;
                return false;
            }

            if (typeof(T) == typeof(bool))
            {
                if ("1".Equals(value))
                    value = "true";
                if ("0".Equals(value))
                    value = "false";
            }

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
        result = (T)obj;
        return true;
    } 
    
    public void SetValue(string key, string value)
    {
        key = key.Trim();
        if (key.Length < 1)
            return;
        value = value.Trim();
        if (value.Length < 1)
            return;
        keyVal[key] = value;
    }

    /// <summary>
    /// 确保文件可以保存在这个路径,如果目标文件存在，则删除之,如果目标文件所在文件夹不存在，则创建之
    /// </summary>
    public void MakeSureCanSaveToPath(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        else
        {
            var dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
    
    /// <summary>
    /// 取得数据存放目录
    /// </summary>
    public static string GetDataPath()
    {
#if UNITY_EDITOR
        switch (UnityEditor.EditorUserBuildSettings.activeBuildTarget)
        {
            case UnityEditor.BuildTarget.Android:
            case UnityEditor.BuildTarget.iOS:
            case UnityEditor.BuildTarget.WebGL:
                return Application.persistentDataPath;
            
            default: return Application.streamingAssetsPath;
        }
#else
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
            case RuntimePlatform.WebGLPlayer:
                return Application.persistentDataPath;
                
            default: return Application.streamingAssetsPath;
        }
#endif
    }
    

    public static void Clear()
    {
        File.Delete(Path.Combine(Application.persistentDataPath, ConfigName));
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}
