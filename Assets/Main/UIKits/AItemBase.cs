using UnityEngine;
using System;
using UnityEngine.EventSystems;

internal interface IItemBase
{
    void setObj(GameObject obj);

    void Refresh();

    void Refresh<T>(T data);

    void OnClick(GameObject obj, PointerEventData eventData);

    void Destory();
}


internal abstract class AItemBase : IItemBase
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
        set
        {
            if (null != m_gameobj)
                m_gameobj.SetActive(value);
        }

        get { return m_gameobj.activeInHierarchy; }
    }

    public virtual void Refresh() { }
    public virtual void Refresh<T>(T data) { if (data == null) return; }
    public virtual void Update() { }
    public virtual void OnClick(GameObject obj, PointerEventData eventData) { }
    public virtual void Clear() { }
    public virtual void Destory()
    {
        if (null != m_gameobj)
            GameObject.Destroy(m_gameobj);
        m_gameobj = null;
    }

    public virtual void ResetUI() { }
}

internal class AItemPageBase : AItemBase
{
    public virtual void AddEventListener() { }
    public virtual void RemoveEventListener() { }

    public override bool IsActive
    {
        get => base.IsActive;
        set
        {
            if (IsActive == value)
                return;

            base.IsActive = value;
            if (value)
            {
                Refresh();
                AddEventListener();
            }
            else
                Close();
        }
    }

    public virtual void Close()
    {
        RemoveEventListener();
    }
}