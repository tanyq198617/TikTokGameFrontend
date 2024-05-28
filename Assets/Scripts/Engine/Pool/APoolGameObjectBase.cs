using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class APoolGameObjectBase : IGameObject
{
    protected GameObject m_gameobj;
    protected Transform Trans;
    
    public virtual void setObj(GameObject gameObject)
    {
        this.m_gameobj = gameObject;
        this.Trans = gameObject.transform;
    }

    public virtual void OnDestroy()
    {
        this.m_gameobj = null;
        this.Trans = null;
    }

    public virtual void OnDisable()
    {
        IsActive = false;
    }

    public virtual void OnEnable()
    {
        IsActive = true;
    }

    public GameObject GetGameObject() => m_gameobj;
    public Transform GetTransform() => Trans;
    public void Attach(Transform root) => UIUtility.Attach(root, m_gameobj);
    
    public virtual bool IsActive
    {
        set
        {
            if (null != m_gameobj)
                m_gameobj.SetActiveEX(value);
        }

        get => m_gameobj.activeSelf;
    }
}
