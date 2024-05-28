using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Factory<T> : FactoryBase where T : APoolGameObjectBase, new()
{
    public static Factory<T> Instance => Singleton<Factory<T>>.Instance;

    /// <summary> 挂点 </summary>
    protected Transform mRoot;

    /// <summary> 模板预制体 </summary>
    protected GameObject mPrefab;

    public FactoryBase Init(Transform root, GameObject prefab)
    {
        this.mRoot = root;
        this.mPrefab = prefab;
        Instance.ONCreate += OnCreate;
        Instance.ONGet += OnEnable;
        Instance.ONPush += OnDisable;
        Instance.ONDestroy += OnDestroy;
        Instance.ONLog += OnLog;
        return Instance;
    }

    public T OnCreate()
    {
        var item = ClassFactory.GetOrCreate<T>();
        item.setObj(Object.Instantiate(mPrefab, mRoot));
        return item;
    }

    private void OnEnable(APoolGameObjectBase item)
    {
        item.OnEnable();
    }

    private void OnDisable(APoolGameObjectBase item)
    {
        item.OnDisable();
    }

    private void OnDestroy(APoolGameObjectBase item)
    {
        var cls = item as T;
        var gameobj = cls.GetGameObject();
        item.OnDisable();
        ClassFactory.Recycle(cls);
        Object.Destroy(gameobj);
    }

    private void OnLog(int count)
    {
        Debuger.Log($"当前对象池[{typeof(T)}]个数：{count}");
    }
}