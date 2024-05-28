using CatJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class CatJsonExtension
{
    
    public static T ParseJson<T>(this TextAsset asset, JsonParser parser = null) 
    {
        if (asset == null) return default;
        if(string.IsNullOrEmpty(asset.text)) return default;
        return asset.text.ParseJson<T>(parser);
    }
    public static T ParseJson<T>(this DownloadHandler handler, JsonParser parser = null)
    {
        if (handler == null) return default;
        if (string.IsNullOrEmpty(handler.text)) return default;
        return handler.text.ParseJson<T>(parser);
    }
}
