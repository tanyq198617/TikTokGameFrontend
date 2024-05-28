using HotUpdateScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool<T> : PoolBase<T> where T : AItemBase, new()
{
    public GameObject templete { get; protected set; }
    public RectTransform root { get; protected set; }

    public ItemPool(GameObject obj, RectTransform root)
    {
        this.templete = obj;
        this.root = root;

        ONCreate += OnCreate;
        ONGet += OnGet;
        ONPush += OnPush;
        ONDestroy += OnDestroy;
        ONLog += OnLog;
    }

    private T OnCreate() => UIUtility.CreateItem<T>(templete, root);

    private void OnDestroy(T obj)
    {
        obj.Destory();
        obj = null;
    }

    private void OnGet(T obj)
    {
        obj.IsActive = true;
    }

    private void OnPush(T obj)
    {
        obj.Clear();
        obj.IsActive = false;
    }

    private void OnLog(int obj)
    {
    }

}