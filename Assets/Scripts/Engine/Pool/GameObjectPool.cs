using Cysharp.Threading.Tasks;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using YooAsset;
using Object = UnityEngine.Object;

public class GameObjectPool<T> : PoolBase<T> where T : IGameObject, new()
{
    public static GameObjectPool<T> Instance { get { return Singleton<GameObjectPool<T>>.Instance; } }

    protected string Path;
    protected GameObject templete;
    protected Transform _root;
    protected AssetOperationHandle loadHandler;

    /// <summary> 类对象池 </summary>
    //protected readonly ClassPool<T> classPool;

    /// <summary> 记录在使用中的对象 </summary>
    protected readonly HashSet<T> useSet = new HashSet<T>();

    private CancellationTokenSource source;

    /// <summary> 获取时事件 </summary>
    public event Action<T> DequeueEvent;

    /// <summary> 还回时事件 </summary>
    public event Action<T> EnqueueEvent;

    public GameObjectPool()
    {
        // _root = Create();
        //classPool = ClassPool<T>.Instance;

        ONCreate += OnCreate;
        ONGet += OnGet;
        ONPush += OnPush;
        ONDestroy += OnDestroy;
        ONLog += OnLog;
    }

    public virtual void Init(string resPath, Transform defaultRoot)
    {
        this.Path = resPath;
        _root = defaultRoot != null ? defaultRoot : Create();
        var gameObject = GameObjectFactory.FindPrefab(resPath);
        if (gameObject == null)
        {
            loadHandler = YooAssets.LoadAssetSync<GameObject>(Path);
            this.templete = loadHandler.GetAssetObject<GameObject>();
            GameObjectFactory.AddToPrefab(resPath, templete);
        }
        else
            this.templete = gameObject;
    }

    public virtual async UniTask InitAsync(string resPath, Transform defaultRoot)
    {
        this.Path = resPath;
        _root = defaultRoot != null ? defaultRoot : Create();
        var gameObject = GameObjectFactory.FindPrefab(resPath);
        if (gameObject == null)
        {
            loadHandler = YooAssets.LoadAssetAsync<GameObject>(Path);
            await loadHandler.ToUniTask();
            this.templete = loadHandler.GetAssetObject<GameObject>();
            GameObjectFactory.AddToPrefab(resPath, templete);
        }
        else
            this.templete = gameObject;
    }

    public void PreLoad(int count)
    {
        PreLoadAssets(count).Forget();
    }

    public async UniTask PreLoadAssets(int count)
    {
        if (Count >= count)
            return;

        int num = count - Count;

        source = new CancellationTokenSource();

        for (int i = 0; i < num; i++)
        {
            if (source == null || source.IsCancellationRequested)
                return;

            var item = CreateObj();
            item.OnDisable();
            PreMultiObj(item);

            if (i > 0 && i % 5 == 0)
                await UniTask.Delay(25, cancellationToken: source.Token);
        }
        LogPool();
    }

    private Transform Create() 
    {
        GameObject rootObj = new GameObject($"~{typeof(T).Name}.pool");
        Object.DontDestroyOnLoad(rootObj);
        return rootObj.transform;
    }

    private T OnCreate()
    {
        if (templete == null)
        {
            Debug.LogError($"[对象池] 游戏物体池未初始化, 请通过 GameObjectFactory.Register 进行初始化, Type={typeof(T)}");
            return default;
        }
        GameObject go = GameObject.Instantiate(templete, _root);
        //go.SetActiveEX(false);
        //T item = classPool.RequestObj();
        T item = new T();
        item.setObj(go);
        return item;
    }

    private void OnDestroy(T obj)
    {
        if (obj == null) return;
        if (obj.GetGameObject() == null) return;
        obj.OnDestroy();
        if (obj.GetGameObject() != null) 
            GameObject.Destroy(obj.GetGameObject());
        //classPool.ReturnObj(obj);
    }

    protected virtual void OnGet(T obj)
    {
        obj?.OnEnable();
        useSet.Add(obj);
        DequeueEvent?.Invoke(obj);
    }

    protected virtual void OnPush(T obj)
    {
        obj?.Attach(_root);
        obj?.OnDisable();
        EnqueueEvent?.Invoke(obj);
        useSet.Remove(obj);
    }

    private void OnLog(int obj)
    {
        Type t = typeof(T);
        if (GameObjectFactory.IsLog(t))
        {
            Log();
        }
    }

    public void Log()
    {
        Type t = typeof(T);
        Debuger.LogError($"{t} 池总创建个数：{CreateCount} 当前在使用个数：{useSet.Count}, 池内对象个数：{Count}");
    }

    public T GetOrCreate() => RequestObj();
    public void Recycle(T obj)
    {
        ReturnObj(obj);
    }

    public void RecycleAll()
    {
        var arr = useSet.ToArray();
        for (int i = 0; i < arr.Length; i++)
            ReturnObj(arr[i], false);
        useSet.Clear();
        arr = null;
        LogPool();
    }

    public void DestroyAll()
    {
        RecycleAll();
        OnDispose();
    }

    public void OnDestory() 
    {
        loadHandler?.Release();
        templete = null;
        Path = string.Empty;
    }
}
