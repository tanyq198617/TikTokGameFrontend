using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class AItemBase : IItemBase
{
    public int index;

    protected AItemBase _ownItem = null;

    protected internal GameObject m_gameobj = null;

    protected internal Transform Trans = null;

    protected internal RectTransform RectTrans = null;

    public AItemBase() { }

    public virtual void setObj(GameObject obj)
    {
        if (obj == null) Debug.Log("------------AItemBase中OBJ不同----------");

        this.m_gameobj = obj;

        this.Trans = obj.transform;
        this.RectTrans = obj.GetComponent<RectTransform>();
    }

    public virtual bool IsActive
    {
        set { if (null != m_gameobj) m_gameobj.SetActiveEX(value); }
        get { return m_gameobj != null ? m_gameobj.activeInHierarchy : false; }
    }

    public virtual void Refresh() { }
    public virtual void Refresh<T>(T data) { }
    public virtual void Update() { }
    public virtual void OnClick(GameObject obj, PointerEventData eventData) { }
    public virtual void Clear() { }
    public virtual void Destory()
    {
        if (null != m_gameobj)
            GameObject.Destroy(m_gameobj);
        m_gameobj = null;
    }

    public virtual void sendPara(object args) { }
    public virtual void ResetUI() { }
    public void SetActive(bool active) { m_gameobj?.SetActiveEX(active); }
}

/// <summary>
/// 带互斥选择的item基类
/// </summary>
public class ASelectItemBase : AItemBase
{
    private bool _isSelect = false;
    /** 记录被选择状态 */
    public bool IsSelect
    {
        get { return _isSelect; }
        set
        {
            if (_isSelect == value && !value) return;
            if (value) { InSelect(); }
            else { NoSelect(); }
        }
    }

    /** 选中状态 */
    public virtual void InSelect() { _isSelect = true; }
    /** 非选中状态 */
    public virtual void NoSelect() { _isSelect = false; }

}

public class AItemPageBase : AItemBase
{
    private bool isListener = false;
    public virtual void AddEventListener() { }
    public virtual void RemoveEventListener() { }
    public virtual void Close() { }

    public override bool IsActive
    {
        get => base.IsActive;
        set
        {
            base.IsActive = value;
            if (value)
                Show();
            else
                Hide();
        }
    }

    private void Show()
    {
        Refresh();

        if (!isListener)
        {
            AddEventListener();
            isListener = true;
        }
    }

    private void Hide()
    {
        Close();
        if (isListener)
        {
            RemoveEventListener();
            isListener = false;
        }
    }
}

public class AItemButtonBase : AItemBase
{
    public Action<int> OnItemClick;

    protected bool _select;

    public override bool IsActive
    {
        get => _select;
        set
        {
            _select = value;
            if (value)
                OnSelect();
            else
                NoSelect();
        }
    }

    public virtual void OnSelect() { }
    public virtual void NoSelect() { }
}
