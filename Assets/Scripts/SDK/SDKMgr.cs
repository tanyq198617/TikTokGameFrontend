using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SDKMgr : MonoBehaviour
{
    public static SDKMgr Instance { get; private set; }

    private LoginType SDKType { get; set; }

    private ALoginControl control;


    private void Awake()
    {
        Instance = this;
        control = LoginControlCache.Get(Boot.Config.LoginType);
        GameObject.DontDestroyOnLoad(this);
    }

    /// <summary> 自动登录 </summary>
    public void OnAutoLogin(Action loginCall)
    {
        Debuger.LogWarning($"SDK自动登录...");
        control?.OnAutoLogin(loginCall);
    }

    public bool OnReconnect()
    {
        if (control == null)
            return false;
        return control.OnReconnect();
    }

    /// <summary> 显示登录界面 </summary>
    public void ShowLoginView(Action openCall, bool clear = true) { control?.ShowLoginView(openCall, true); }

    private void OnDestroy()
    {
        control?.OnDispose();
    }
}
