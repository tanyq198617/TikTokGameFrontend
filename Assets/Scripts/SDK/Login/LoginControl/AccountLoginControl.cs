using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 普通登录方式
/// </summary>
public class AccountLoginControl : ALoginControl
{
    public override LoginType LoginType => LoginType.普通;

    /// <summary> 打开登录面板 </summary>
    public override void ShowLoginView(Action openCall, bool clear)
    {
        OnReconnect();
        UIMgr.Instance.ShowUI(UIPanelName.StartGameView, openCall: (p) => openCall?.Invoke());
    }

    /// <summary> 自动登录 </summary>
    public override void OnAutoLogin(Action loginCall)
    {
        ShowLoginView(loginCall, false);
    }

    public override bool OnReconnect()
    {
        base.OnReconnect();
        WaitCircleView.Instance.OnLock();
        NetMgr.GetHandler<LoginHandler>().ReqLogin();
        return true;
    }
}
