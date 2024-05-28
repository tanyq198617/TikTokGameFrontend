using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 多行多列
/// </summary>
public class UILayoutGroup<T, W> : AItemBase where T : ALayoutItem
{
    /// <summary> 列表控制器 </summary>
    public UIInfiniteTable m_table;

    public ScrollRect m_Scroll;

    protected Dictionary<int, T> m_itemDict;
    protected List<W> m_dataList;
    protected Action<T> onItemClick;

    private T temp;
    private object args = null;


    public int SelectIndex = -1;

    public void SetSelect(int selectIndex)
    {
        if (m_itemDict == null || m_itemDict.Count <= 0)
        {
            SelectIndex = -1;
            return;
        }

        foreach (T value in m_itemDict.Values)
        {
            if (value != null)
                value.IsSelect = value.index == selectIndex;
        }

        SelectIndex = selectIndex;
    }

    public void OnInit(GameObject obj, Action<T> bindItemEvent = null, Action onLoadEnd = null)
    {
        if (obj == null)
        {
            Debug.LogError("不存在的 UIInfiniteTable 组件!!!");
            return;
        }

        m_table = obj.GetComponent<UIInfiniteTable>();
        if (m_table == null)
        {
            Debug.LogError("不存在的 UIInfiniteTable 组件!!!");
            return;
        }

        m_itemDict = new Dictionary<int, T>();
        m_Scroll = m_table.ScrollRect;

        this.setObj(obj);
        this.onItemClick = (Action<T>)bindItemEvent;

        m_table.onCreate = OnCreate;
        m_table.onRefreshItem = OnRefreshItem;
        m_table.onHide = OnHide;
        m_table.onLoadEnd = onLoadEnd;
    }

    private void OnHide(int onlyId, GameObject obj)
    {
        if (m_itemDict.TryGetValue(onlyId, out T item))
            item.Clear();
        obj.name = "-1";
    }

    private void OnCreate(int onlyId, GameObject obj)
    {
        T item = Activator.CreateInstance<T>();
        item.setObj(obj);
        if (m_Scroll == null)
            m_Scroll = m_table.ScrollRect;
        item.OnBindInitEvent(m_Scroll, onItemClick);
        if (!m_itemDict.TryGetValue(onlyId, out temp))
            m_itemDict.Add(onlyId, item);
        else
            m_itemDict[onlyId] = item;
    }

    //刷新UI 回调 -- 用来更新数据
    private void OnRefreshItem(GameObject go, int dataIndex, int onlyId)
    {
        if (m_itemDict.TryGetValue(onlyId, out T item))
        {
            item.m_gameobj.name = dataIndex.ToString();
            item.index = dataIndex;
            item.OnlyId = onlyId;
            if (dataIndex >= m_dataList.Count)
            {
                Debuger.LogError($"[{typeof(T)}] 超出数据个数...");
                return;
            }

            item.Refresh(m_dataList[dataIndex]);

            if (args != null)
                item.sendPara(args);
        }
    }

    public int GetItemCount()
    {
        int count = 0;
        if (m_itemDict.IsNotNull()) count = m_itemDict.Count;
        return count;
    }


    public T GetItem(int onlyId)
    {
        if (m_itemDict.TryGetValue(onlyId, out T item))
        {
            return item;
        }

        return null;
    }

    public List<T> GetAllItem()
    {
        List<T> list = new List<T>(m_itemDict.Values);
        return list;
    }


    /// <summary> 刷新数据入口 </summary>
    public void DefaultRefresh(List<W> dataList, object args = null)
    {
        if (dataList == null)
            return;
        this.m_dataList = dataList;
        this.args = args;
        OnReBuild();
    }

    public void RefreshChild(int dataIndex)
    {
        if (dataIndex < 0 || dataIndex >= m_dataList.Count)
            return;

        int onlyId = m_table.GetItemChildIndexByIndex(dataIndex);

        if (m_itemDict.TryGetValue(onlyId, out T item))
        {
            item.Refresh(m_dataList[dataIndex]);
        }
    }

    /// <summary>
    /// 重建列表
    /// </summary>
    public void OnReBuild()
    {
        //赋值数据总数
        m_table.TableDataCount = m_dataList.IsNotNullOrNoCount() ? m_dataList.Count : 0;
    }

    public int GetOnlyID(int dataIndex)
    {
        return m_table.GetItemChildIndexByIndex(dataIndex);
    }

    /// <summary>
    /// 还原位置
    /// </summary>
    public void OnReposition()
    {
        m_Scroll?.StopMovement();
        m_table?.OnResetPosition();
    }

    public void OnMoveTo(int index)
    {
        m_Scroll.StopMovement();
        m_table.MoveTo(index);
    }

    public void UpdateAnchor()
    {
        m_table?.UpdateAnchor();
    }

    public override void Clear()
    {
        foreach (var item in m_itemDict)
        {
            item.Value.Clear();
        }
    }
}

public class ALayoutItem : AItemBase
{
    public int OnlyId;
    public Delegate OnItemClick { get; private set; }
    public ScrollRect ScrollRect { get; private set; }

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

    public void OnBindInitEvent(ScrollRect scroll, Delegate onclick)
    {
        this.OnItemClick = onclick;
        this.ScrollRect = scroll;
        if (ScrollRect != null)
        {
            UIUtility.BindDragBeginEvent(m_gameobj, OnBeginDrag);
            UIUtility.BindDragEvent(m_gameobj, OnDrag);
            UIUtility.BindDragEndEvent(m_gameobj, OnEndDrag);
            UIUtility.BindScrollEvent(m_gameobj,OnScroll);
        }
    }
    
    private void OnScroll(GameObject go, PointerEventData eventdata)
    {
        ScrollRect.OnScroll(eventdata);
    }

    protected virtual void OnBeginDrag(GameObject go, PointerEventData eventData)
    {
        ScrollRect.OnBeginDrag(eventData);
    }

    protected virtual void OnDrag(GameObject go, PointerEventData eventData)
    {
        ScrollRect.OnDrag(eventData);
    }

    protected virtual void OnEndDrag(GameObject go, PointerEventData eventData)
    {
        ScrollRect.OnEndDrag(eventData);
    }
    
    protected virtual void StopMovement()
    {
        ScrollRect?.StopMovement();
    }

}