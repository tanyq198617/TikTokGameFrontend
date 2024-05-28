using CatJson;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.Networking.UnityWebRequest;

public class HttpUtility
{
    public static async UniTask<string> HttpGetAsync(string url, IProgress<float> onProgress = null, int timeout = 30000)
    {
        try
        {
            using (UnityWebRequest www = Get(url))
            {
                www.timeout = timeout;
                www.SetRequestHeader("User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)");
                var weq = await www.SendWebRequest().ToUniTask(onProgress);
               
                if (weq.IsError())
                    return "";

                return weq.downloadHandler.text;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message + "地址：" + url);
            return "";
        }
    }

    public static async UniTask<T> HttpGetAsync<T>(string url, int timeout = 10000) where T : class, new()
    {
        string result = await HttpGetAsync(url, null, timeout);
        if (result.IsJson())
        {
            var value = result.ParseJson<T>();
            return value;
        }
        Debug.LogError($"无法解析到类型[{typeof(T).FullName}], result={result} | 地址：" + url);
        return null;
    }
    
    public static async UniTask<Texture> HttpGetTextureAsync(string url, int timeout = 10000)
    {
        try
        {
            var texD1 = new DownloadHandlerTexture(true);
            using var www = Get(url);
            www.timeout = timeout;
            www.downloadHandler = texD1;
            var weq = await www.SendWebRequest().ToUniTask();
            if (weq.IsError())
                return null;
            var tex = texD1.texture;
            return tex;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message + "地址：" + url);
            return null;
        }
    }
}
