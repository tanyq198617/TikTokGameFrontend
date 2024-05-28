using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolBase<T>
{
    /// <summary>
    /// 创建T对象的事件
    /// </summary>
    public event Func<T> ONCreate;

    /// <summary>
    /// 获取T对象的时候的事件
    /// </summary>
    public event Action<T> ONGet;

    /// <summary>
    /// 返还T对象时的事件
    /// </summary>
    public event Action<T> ONPush;

    /// <summary>
    /// 销毁T对象时的事件
    /// </summary>
    public event Action<T> ONDestroy;

    /// <summary>
    /// 销毁T对象时的事件
    /// </summary>
    public event Action<int> ONLog;

    /// <summary>
    /// 池子
    /// </summary>
    private readonly Queue<T> _queue;

    protected int Count => _queue?.Count ?? 0;

    protected int CreateCount;
    public bool isRelease;

    //private Lock locked = new Lock();

    /// <summary>
    /// 创建对象池
    /// </summary>
    /// <param name="initCapacity">初始容量</param>
    protected PoolBase(int initCapacity = 1000)
    {
        _queue = new Queue<T>(initCapacity);
        CreateCount = 0;
    }

    /// <summary>
    /// 有没有对象
    /// </summary>
    public bool HasFree => _queue != null && _queue.Count > 0;

    /// <summary>
    /// 获取对象
    /// </summary>
    public T RequestObj()
    {
        var ret = HasFree ? _queue.Dequeue() : CreateObj();
        ONGet?.Invoke(ret);
        LogPool();
        return ret;
    }

    protected T CreateObj()
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
            ret = ONCreate.Invoke();
        }
        CreateCount++;
        return ret;
    }

    public bool IsInPool(T obj) => _queue.Contains(obj);

    /// <summary>
    /// 返还对象
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="log"></param>
    public void ReturnObj(T obj, bool log = true)
    {
        if (isRelease) return;
        if (obj == null) return;
        if (IsInPool(obj)) return;
        ONPush?.Invoke(obj);
        _queue.Enqueue(obj);
        if (log) LogPool();
    }

    protected void PreMultiObj(T obj)
    {
        if (obj == null) return;
        _queue.Enqueue(obj);
    }

    protected void OnDispose()
    {
        isRelease = true;
        while (_queue.Count > 0)
        {
            T t = _queue.Dequeue();
            ONDestroy?.Invoke(t);
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
            ONDestroy?.Invoke(t);
            CreateCount--;
        }
        isRelease = false;
        LogPool();
        Debug.LogError($"{GetType()}池内个数:{_queue.Count}");
    }

    protected void LogPool() { if (ONLog != null) ONLog.Invoke(Count); }
}
