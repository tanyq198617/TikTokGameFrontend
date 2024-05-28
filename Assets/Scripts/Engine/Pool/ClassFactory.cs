using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 类对象池总池
/// </summary>
public class ClassFactory
{
    public static T GetOrCreate<T>() where T : class, new() => ClassPool<T>.Instance.RequestObj();
    public static void RecycleAll<T>() where T : class, new() => ClassPool<T>.Instance.RecycleAll();
    public static void Recycle<T>(T t) where T : class, new() => ClassPool<T>.Instance.Recycle(t);
    public static void DestroyAll<T>() where T : class, new() => ClassPool<T>.Instance.DestroyAll();
    public static void PreLoad<T>(int count = 1) where T : class, new() => ClassPool<T>.Instance.PreLoad(count);

    public static void Release<T>(int count = 50) where T : class, new() => ClassPool<T>.Instance.OnRelease(count);

    public static bool IsLog(Type t)
    {
        if (LogHashSets.IsNullOrNoCount())
            return false;
        return LogHashSets.Contains(t);
    }

    public static HashSet<Type> LogHashSets = new HashSet<Type>
    {
        //typeof(TextureMergeItem),
        //typeof(ColorLoader),
    };

    public static void ReleaseClass()
    {
    }
}
