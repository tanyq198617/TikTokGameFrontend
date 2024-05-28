using System.Collections;
using System.Collections.Generic;
using HotUpdateScripts;
using UnityEngine;

/// <summary>
/// 炮口基类
/// </summary>
public abstract class AMuzzleControlBase : APoolGameObjectBase, IEvent
{
    protected FireComponent fire;
    
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        this.fire = obj.GetOrAddComponent<FireComponent>();
    }

    public override void OnDestroy()
    {
        Object.Destroy(fire);
        base.OnDestroy();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        AddEventListener();
    }

    public override void OnDisable()
    {
        RemoveEventListener();
        fire?.Clear();
        AutoClear();
        base.OnDisable();
    }

    protected virtual void AutoClear() { }
    public virtual void AddEventListener() { }
    public virtual void RemoveEventListener() { }
}
