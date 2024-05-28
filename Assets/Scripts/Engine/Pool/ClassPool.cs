
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ClassPool<T> : PoolBase<T> where T : new()
{
    public static ClassPool<T> Instance { get { return Singleton<ClassPool<T>>.Instance; } }

    private readonly HashSet<T> objs = new HashSet<T>();

    private CancellationTokenSource source;

    /// <summary> 创建时事件 </summary>
    public event Action<T> CreateEvent;

    /// <summary> 销毁时事件 </summary>
    public event Action<T> DestroyEvent;

    /// <summary> 获取时事件 </summary>
    public event Action<T> DequeueEvent;

    /// <summary> 还回时事件 </summary>
    public event Action<T> EnqueueEvent;

    public ClassPool()
    {
        ONCreate += OnCreate;
        ONGet += OnGet;
        ONPush += OnPush;
        ONDestroy += OnDestroy;
#if UNITY_EDITOR
        ONLog += OnLog;
#endif
    }

    public void PreLoad(int count)
    {
        PreLoadAssets(count).Forget();
    }

    private async UniTaskVoid PreLoadAssets(int count)
    {
        if (Count >= count)
            return;

        int num = count - Count;

        source = new CancellationTokenSource();

        for (int i = 0; i < num; i++)
        {
            if (source == null || source.IsCancellationRequested)
                return;

            PreMultiObj(CreateObj());

            if (i > 0 && i % 5 == 0)
                await UniTask.Yield();
        }
        LogPool();
    }

    private T OnCreate()
    {
        T t = new T();
        CreateEvent?.Invoke(t);
        //Debuger.LogError($"创建了{t.GetType().Name}");
        return t;
    }

    private void OnDestroy(T obj)
    {
        objs.Remove(obj);
        DestroyEvent?.Invoke(obj);
    }

    private void OnGet(T obj)
    {
        DequeueEvent?.Invoke(obj);
        objs.Add(obj);
    }

    private void OnPush(T obj)
    {
        if (obj == null)
            return;
        EnqueueEvent?.Invoke(obj);
        objs.Remove(obj);
    }

    private void OnLog(int count)
    {
        Type t = typeof(T);
        if (ClassFactory.IsLog(t))
        {
            Log();
        }
    }

    public void Log()
    {
        Type t = typeof(T);
        Debuger.LogError($"{t} 池总创建个数：{CreateCount} 当前在使用个数：{objs.Count}, 池内对象个数：{Count}");
    }

    public void Recycle(T t)
    {
        ReturnObj(t);
    }

    public void RecycleAll()
    {
        var arr = objs.ToArray();
        objs.Clear();
        for (int i = 0; i < arr.Length; i++)
        {
            ReturnObj(arr[i], false);
        }
        arr = null;
        LogPool();
    }

    public void DestroyAll()
    {
        source?.Cancel(false);
        source = null;

        var arr = objs.ToArray();
        objs.Clear();
        for (int i = 0; i < arr.Length; i++)
        {
            ReturnObj(arr[i], false);
        }
        OnDispose();
        CreateEvent = null;
        DestroyEvent = null;
        DequeueEvent = null;
        EnqueueEvent = null;
    }
}
