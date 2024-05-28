using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class FactoryBase
{
    public event Func<APoolGameObjectBase> ONCreate;
    public event Action<APoolGameObjectBase> ONGet;
    public event Action<APoolGameObjectBase> ONPush;
    public event Action<APoolGameObjectBase> ONDestroy;
    public event Action<int> ONLog;

    /// <summary>
    /// 池子
    /// </summary>
    private readonly Queue<APoolGameObjectBase> _queue;

    protected int Count => _queue?.Count ?? 0;

    protected int CreateCount;
    public bool isRelease;

    //private Lock locked = new Lock();

    /// <summary>
    /// 创建对象池
    /// </summary>
    /// <param name="initCapacity">初始容量</param>
    protected FactoryBase(int initCapacity = 1000)
    {
        _queue = new Queue<APoolGameObjectBase>(initCapacity);
        CreateCount = 0;
    }

    /// <summary>
    /// 有没有对象
    /// </summary>
    public bool HasFree => _queue != null && _queue.Count > 0;

    /// <summary>
    /// 获取一个对象
    /// </summary>
    public T GetOrCreate<T>() where T : APoolGameObjectBase
    {
        var ret = HasFree ? (T)_queue.Dequeue() : CreateObj<T>();
        if (ONGet is Action<T> call)
            call(ret);
        LogPool();
        return ret;
    }

    protected T CreateObj<T>() where T : APoolGameObjectBase
    {
        T ret;
        if (ONCreate == null)
        {
            if (typeof(T).IsAssignableFrom(typeof(UnityEngine.Object)))
                throw new InvalidOperationException("没有注册OnCreate事件");
            ret = ActivatorFactory.CreateInstance<T>();
        }
        else
        {
            ret = (T)ONCreate();
        }
        CreateCount++;
        return ret;
    }


    /// <summary>
    /// 返还对象
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="log"></param>
    public void Recycle<T>(T obj, bool log = true) where T : APoolGameObjectBase
    {
        if (isRelease) return;
        if (obj == null) return;
        if (ONPush is Action<T> call)
            call(obj);
        _queue.Enqueue(obj);
        if (log) LogPool();
    }

    protected void PreMultiObj<T>(T obj) where T : APoolGameObjectBase
    {
        if (obj == null) return;
        _queue.Enqueue(obj);
    }

    protected void OnDispose()
    {
        isRelease = true;
        while (_queue.Count > 0)
        {
            var t = _queue.Dequeue();
            if (ONDestroy != null) ONDestroy(t);
            CreateCount--;
        }
        isRelease = false;
        LogPool();
    }

    public void OnRelease(int capacity)
    {
        isRelease = true;
        while (_queue.Count > capacity)
        {
            var t = _queue.Dequeue();
            if (ONDestroy != null) ONDestroy(t);
            CreateCount--;
        }
        isRelease = false;
        LogPool();
        Debug.LogError($"{GetType()}池内个数:{_queue.Count}");
    }

    protected void LogPool() { if (ONLog != null) ONLog.Invoke(Count); }
    
}
