using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene
{
    private readonly Dictionary<Type, GameObject> monoDict = new Dictionary<Type, GameObject>();

    public T Register<T>(bool dontDestroyOnLoad = false) where T : MonoBehaviour
    {
        Type t = typeof(T);
        if (monoDict.ContainsKey(t))
        {
            if (monoDict[t] != null)
                return monoDict[t].GetComponent<T>();
            monoDict.Remove(t);
        }
        GameObject go = new GameObject(t.Name);
        T script = go.AddComponent<T>();
        if (dontDestroyOnLoad)
            GameObject.DontDestroyOnLoad(go);
        monoDict.Add(t, go);
        Debuger.Log($"[Scene] 脚本【{t.FullName}】添加成功!!! ");
        return script;
    }

    public T GetOrAddComponent<T>(bool dontDestroyOnLoad = false) where T : MonoBehaviour
    {
        Type t = typeof(T);
        T script = null;
        monoDict.TryGetValue(t, out var go);
        if (go == null)
        {
            script = Register<T>(dontDestroyOnLoad);
            return script;
        }
        script = go.GetComponent<T>();
        return script;
    }

    public T GetComponent<T>() where T : MonoBehaviour
    {
        Type t = typeof(T);
        monoDict.TryGetValue(t, out var go);
        if (go == null) return null;
        return go.GetComponent<T>();
    }

    public void RemoveComponent<T>() where T : MonoBehaviour
    {
        Type t = typeof(T);
        monoDict.TryGetValue(t, out var go);
        if (go == null)
            return;
        var comp = go.GetComponent<T>();
        monoDict.Remove(t);
        comp?.Destroy();
        go?.Destroy();
    }

    public void OnDespose()
    {
        List<GameObject> objs = new List<GameObject>(monoDict.Values);
        for (int i = objs.Count - 1; i >= 0; i--)
        {
            GameObject.Destroy(objs[i]);
        }
        monoDict.Clear();
    }
}
