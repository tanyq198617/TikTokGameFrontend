using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtension 
{
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        T t = gameObject.GetComponent<T>();
        if (t == null)
            t = gameObject.AddComponent<T>();
        return t;
    }

    public static T GetOrAddComponent<T>(this Component comp) where T : Component
    {
        T t = comp.GetComponent<T>();
        if (t == null)
            t = comp.gameObject.AddComponent<T>();
        return t;
    }

    public static T GetOrAddComponent<T>(this Transform trans) where T : Component
    {
        if (trans == null) return null;
        return trans.gameObject.GetOrAddComponent<T>();
    }

    public static bool RemoveComponent<T>(this Component comp) where T : Component
    {
        T t = comp.GetComponent<T>();
        if (t != null)
        {
            GameObject.Destroy(t);
            return true;
        }
        return false;
    }

    public static bool RemoveSelf(this Component comp)
    {
        if (comp != null)
        {
            GameObject.Destroy(comp);
            return true;
        }
        return false;
    }

    public static void DestroyChildren(this Transform trans)
    {
        if (trans == null) return;
        foreach (Transform item in trans)
        {
            GameObject.Destroy(item.gameObject);
        }
    }

    public static void DestroyChildren(this GameObject obj)
    {
        if (obj == null) return;
        DestroyChildren(obj.transform);
    }
}
