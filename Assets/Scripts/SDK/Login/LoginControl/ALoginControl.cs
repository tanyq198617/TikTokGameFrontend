using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ALoginControl
{
    public abstract LoginType LoginType { get; }

    public void OnCreate()
    {
        AddEventListener();
        Init();
        Debuger.LogWarning($"初始化SDK, 登录类型={LoginType.ToString()}");
    }

    public void OnDispose()
    {
        Clear();
        RemoveEventListener();
    }

    protected virtual void Init() { }
    public abstract void ShowLoginView(Action openCall, bool clear);
    public abstract void OnAutoLogin(Action loginCall);
    public virtual bool OnReconnect() { return false; }
    protected virtual void Clear() { }

    protected virtual void AddEventListener()
    {
        EventMgr.AddEventListener(GameEvent.Login_Succeed, OnLoginSucceed);
        EventMgr.AddEventListener(GameEvent.Login_Fail, OnLoginFail);
    }

    protected virtual void RemoveEventListener()
    {
        EventMgr.RemoveEventListener(GameEvent.Login_Succeed, OnLoginSucceed);
        EventMgr.RemoveEventListener(GameEvent.Login_Fail, OnLoginFail);
    }

    /// <summary> 登录成功 </summary>
    protected virtual void OnLoginSucceed()
    {
        WaitCircleView.Instance.UnLock();
        EventMgr.Dispatch(ServerEvent.S2C_RetLogin_Succeed);
    }
    
    protected virtual void OnLoginFail()
    {
        WaitCircleView.Instance.UnLock();
        EventMgr.Dispatch(ServerEvent.S2C_RetLogin_Fail);
    }
}
