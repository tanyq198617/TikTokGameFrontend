using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AStateNode : IStateNode, IEvent
{
    protected bool IsComplete = false;

    protected StateMachine _machine;

    public virtual void OnCreate(StateMachine machine) => _machine = machine;

    public void OnEnter() 
    {
        IsComplete = false; 
        AddEventListener();
        Begin();
    }

    public void OnExit() 
    {
        RemoveEventListener();
        End(); 
        IsComplete = false; 
    }

    protected virtual void Begin() { }
    protected virtual void End() { }

    public virtual void FixedUpdate() { }
    public virtual void LateUpdate() { }
    public virtual void SysUpdate() { }

    public virtual void AddEventListener() { }
    public virtual void RemoveEventListener() { }
}