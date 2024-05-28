using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AGroupItemBase : AItemBase
{
    public Delegate OnItemClick { get; internal set; }

    public virtual void OnReBuild()
    {
    }
}

public class UIGridGroup<T, U> where T : AGroupItemBase, new()
{
    private GameObject m_gameobj;
    private RectTransform rectTransform;

    private GameObject templete;

    private GridLayoutGroup grid;
    private ItemPool<T> _pool;

    private HashSet<T> hashset;
    private Action<T> onItemClick = null;

    public void Init(GameObject obj, Action<T> onItemClick = null)
    {
        m_gameobj = obj;
        rectTransform = obj.GetComponent<RectTransform>();
        this.onItemClick = onItemClick;
        grid = obj.GetComponent<GridLayoutGroup>();
        templete = rectTransform.GetChild(0).gameObject;
        templete.SetActiveEX(false);
        _pool = new ItemPool<T>(templete, rectTransform);
        hashset = new HashSet<T>();
    }

    public void DefaultRefresh(List<U> dataList)
    {
        Clear();
        T item = null;
        for (int i = 0; i < dataList.Count; i++)
        {
            item = _pool.RequestObj();
            item.OnItemClick = onItemClick;
            item.Refresh(dataList[i]);
            hashset.Add(item);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }

    public void Clear()
    {
        var array = hashset.ToArray();
        foreach (var item in array)
        {
            item.OnItemClick = null;
            _pool.ReturnObj(item);
        }

        array = null;
    }
}