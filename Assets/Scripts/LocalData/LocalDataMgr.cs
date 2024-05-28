using CatJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalDataMgr
{
    public static int GetInt(string key, int defaultValue = 0) => PlayerPrefs.GetInt(key, defaultValue);
    public static void SetInt(string key, int value) => PlayerPrefs.SetInt(key, value);

    public static bool GetBool(string key, bool defaultValue = false) => PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;//0=false 1=true
    public static void SetBool(string key, bool value) => PlayerPrefs.SetInt(key, value ? 1 : 0);//0=false 1=true

    public static float GetFloat(string key, float defaultValue = 0) => PlayerPrefs.GetFloat(key, defaultValue);
    public static void SetFloat(string key, float value) => PlayerPrefs.SetFloat(key, value);

    public static string GetStr(string key, string defaultValue = "") => PlayerPrefs.GetString(key, defaultValue);
    public static void SetStr(string key, string value) => PlayerPrefs.SetString(key, value);

    public static void Save() => PlayerPrefs.Save();
    public static void DeleteAll() => PlayerPrefs.DeleteAll();
    public static void DeleteKey(string key) => PlayerPrefs.DeleteKey(key);
    public static bool HasKey(string key) => PlayerPrefs.HasKey(key);

    public static void SetObject<T>(string key, T obj) where T : class
    {
        var json = obj.ToJson<T>();
        SetStr(key, json);
    }

    public static T GetObject<T>(string key) where T : class
    {
        string json = GetStr(key);
        if (string.IsNullOrEmpty(json))
            return default;
        var obj = json.ParseJson<T>();
        return obj as T;
    }
}
