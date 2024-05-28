﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGroup<T, W> : AItemBase where T : ALoopItem
{
    public LoopListView m_LoopGroup;

    //游戏物体对应的字典T
    protected Dictionary<GameObject, T> _objTDic = new Dictionary<GameObject, T>();

    public Dictionary<GameObject, T> ObjTDic
    {
        get { return _objTDic; }
    }

    //当前数据
    protected List<W> _curDataList = new List<W>();

    public List<W> CurDataList
    {
        get { return _curDataList; }
    }

    protected Func<W, int, string> _getPrefab;

    protected bool m_IsInit = false;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        m_LoopGroup = obj.GetComponent<LoopListView>();

        if (m_LoopGroup == null)
        {
            Debuger.LogError("游戏物体{0}未指定<LoopListView>组件，请指定", obj.name);
        }

        m_IsInit = false;
    }

    public virtual void DefaultRefresh(List<W> dataList, Func<W, int, string> getPrefab)
    {
        if (dataList.IsNull())
        {
            Debuger.LogError("LoopListView 传入的数据列表为NULL");
            return;
        }

        _curDataList = dataList;
        _getPrefab = getPrefab;
        if (!m_IsInit)
        {
            m_IsInit = true;
            m_LoopGroup.InitListView(dataList.Count, OnGetItemByIndex);
        }
        else
        {
            m_LoopGroup.ResetListView();
            m_LoopGroup.SetListItemCount(_curDataList.Count, false);
            m_LoopGroup.RefreshAllShownItem();
        }
    }

    protected virtual LoopListItem OnGetItemByIndex(LoopListView listView, int index)
    {
        if (index < 0 || index >= listView.ItemTotalCount)
        {
            return null;
        }

        if (index < 0 || index >= _curDataList.Count)
        {
            return null;
        }

        //以下判定也可以根据需求，暴露出去用以重写

        W data = _curDataList[index];
        string name = _getPrefab(data, index);
        LoopListItem item = listView.NewListViewItem(name);
        item.Index = index;
        item.gameObject.name = index.ToString();

        T itemScript;
        if (!ObjTDic.TryGetValue(item.gameObject, out itemScript))
        {
            itemScript = UIUtility.CreateItemNoClone<T>(item.gameObject);
            ObjTDic.Add(item.gameObject, itemScript);
        }

        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
        }

        itemScript.SetItemControl(item, listView);
        itemScript.Refresh(data);
        return item;
    }

    public void MoveToIndex(int index, float offset = 0)
    {
        m_LoopGroup.MovePanelToItemIndex(index, offset);
    }

    public virtual void Reposition()
    {
        m_LoopGroup.ResetListView();
    }

    public virtual void ReBuildAll()
    {
        m_LoopGroup.RefreshAllShownItem();
    }

    public virtual void RefreshByChangeCount()
    {
        m_LoopGroup.SetListItemCount(_curDataList.Count, false);
    }

    public T GetItemScriptByIndex(int itemIndex)
    {
        foreach (T value in ObjTDic.Values)
        {
            if (value.m_gameobj.activeSelf && value.index == itemIndex)
            {
                return value;
            }
        }

        return null;
    }
}

public class ALoopItem : AItemBase
{
    public new int index
    {
        get { return ItemControl.Index; }
    }

    protected LoopListItem ItemControl { get; private set; }
    protected ScrollRect ScrollRect { get; private set; }

    private LoopListView loopListView;

    public void SetItemControl(LoopListItem item, LoopListView view)
    {
        ItemControl = item;
        ScrollRect = item.GetComponent<ScrollRect>();
        loopListView = view;
    }

    public void SetLastOnTachItem()
    {
        loopListView.LastOnTachItem = ItemControl;
    }


    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        UIUtility.BindDragBeginEvent(m_gameobj, OnBeginDrag);
        UIUtility.BindDragEvent(m_gameobj, OnDrag);
        UIUtility.BindDragEndEvent(m_gameobj, OnEndDrag);
    }

    protected virtual void OnBeginDrag(GameObject go, PointerEventData eventData)
    {
        if (ScrollRect == null) ScrollRect = Trans.GetComponentInParent<ScrollRect>();
        //eventData.selectedObject = ScrollRect.gameObject;
        //eventData.pointerDrag = ScrollRect.gameObject;
        ScrollRect.OnBeginDrag(eventData);
    }

    private bool isMove = false;

    protected virtual void OnDrag(GameObject go, PointerEventData eventData)
    {
        if (ScrollRect == null) ScrollRect = Trans.GetComponentInParent<ScrollRect>();
        //eventData.selectedObject = ScrollRect.gameObject;
        //eventData.pointerDrag = ScrollRect.gameObject;
        ScrollRect.OnDrag(eventData);
    }

    protected virtual void OnEndDrag(GameObject go, PointerEventData eventData)
    {
        if (ScrollRect == null) ScrollRect = Trans.GetComponentInParent<ScrollRect>();
        //eventData.selectedObject = ScrollRect.gameObject;
        //eventData.pointerDrag = ScrollRect.gameObject;
        ScrollRect.OnEndDrag(eventData);
    }

    public virtual void SetItemSize(RectTransform.Axis axis, float size)
    {
        RectTrans.SetSizeWithCurrentAnchors(axis, size);
        ItemControl.ParentListView.OnItemSizeChanged(ItemControl.ItemIndex);
    }
}

public class ASelectLoopItem : ALoopItem
{
    private bool _isSelect = false;

    /** 记录被选择状态 */
    public bool IsSelect
    {
        get { return _isSelect; }
        set
        {
            if (_isSelect == value && !value) return;
            if (value)
            {
                InSelect();
            }
            else
            {
                NoSelect();
            }
        }
    }

    /** 选中状态 */
    public virtual void InSelect()
    {
        _isSelect = true;
    }

    /** 非选中状态 */
    public virtual void NoSelect()
    {
        _isSelect = false;
    }
}

public class UISelectGroup<T, W> : UIGroup<T, W> where T : ASelectLoopItem
{
    public int SelectIndex = -1;

    public void SetSelect(int selectIndex)
    {
        foreach (T value in ObjTDic.Values)
        {
            value.IsSelect = value.index == selectIndex;
        }

        SelectIndex = selectIndex;
    }

    protected override LoopListItem OnGetItemByIndex(LoopListView listView, int index)
    {
        LoopListItem item = base.OnGetItemByIndex(listView, index);
        if (item != null)
        {
            T itemScript;
            if (ObjTDic.TryGetValue(item.gameObject, out itemScript))
            {
                itemScript.IsSelect = index == SelectIndex;
            }
        }

        return item;
    }
}