using CatJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class CodeInfo
{
    public int code;
    public string msg;
}

[SerializeField]
public class CheckCodeOk
{
    public string code;
    public string msg;
}

public class JiGuangLoginControl : ALoginControl
{
    public override LoginType LoginType => LoginType.极光登录;

    protected JiGuangSDKHandler jiGuangSDK;
    protected bool IsDebugSDK = false;
    protected string JiGuangCode;

    protected override void Init()
    {
        jiGuangSDK = Game.Scene.Register<JiGuangSDKHandler>(true);
        jiGuangSDK.Init(IsDebugSDK);

        bool notFirstLogin = LocalDataMgr.GetBool(LocalKey.NotFirstLogin);
        if (!notFirstLogin)
        {
            LocalDataMgr.DeleteKey(LocalKey.OpenID);
            LocalDataMgr.DeleteKey(LocalKey.PhoneNumber);
        }
    }

    /// <summary> 打开登录面板 </summary>
    public override void ShowLoginView(Action openCall, bool clear)
    {
        if (clear)
        {
            LocalDataMgr.DeleteKey(LocalKey.OpenID);
            LocalDataMgr.DeleteKey(LocalKey.PhoneNumber);
        }
        JiGuangLoginView.Instance.onAutoLogin = null;
        UIMgr.Instance.ShowUI(UIPanelName.JiGuangLoginView, openCall: (p) =>
        {
            openCall?.Invoke();
        });
    }

    /// <summary> 自动登录 </summary>
    public override void OnAutoLogin(Action loginCall)
    {
        //if (!NetMgr.Instance.IsConnected)
        //{
        //    JiGuangLoginView.Instance.onAutoLogin = CheckAutoLogin;
        //    UIMgr.Instance.ShowUI(UIPanelName.JiGuangLoginView, openCall: (p) =>
        //    {
        //        loginCall?.Invoke();
        //        //GameMain.Engine.TryAginTips();
        //    });
        //    return;
        //}

        if (CheckAutoLogin())
            return;
        else
            ShowLoginView(loginCall, true);
    }

    public bool CheckAutoLogin()
    {
        if (Boot.Config.AutoLogin)
        {
            string openId = LocalDataMgr.GetStr(LocalKey.OpenID);
            string phoneNumber = LocalDataMgr.GetStr(LocalKey.PhoneNumber, "13888888888");

            if (!string.IsNullOrEmpty(openId))
            {
                OnReqLoginByCore(openId, openId, openId, phoneNumber);
                return true;
            }
        }
        return false;
    }

    private void OnReqLoginByCore(string openId, string token, string nickName, string phoneNumber)
    {
        //NetMgr.GetHandler<LoginHanlder>().Login(openId, token, nickName, SDKType.ToInt(), phoneNumber).WrapErrors();
    }

    public override bool OnReconnect()
    {
        base.OnReconnect();
        string openId = LocalDataMgr.GetStr(LocalKey.OpenID);
        string phoneNumber = LocalDataMgr.GetStr(LocalKey.PhoneNumber, "13888888888");

        if (!string.IsNullOrEmpty(openId))
        {
            OnReqLoginByCore(openId, openId, openId, phoneNumber);
            return true;
        }
        return false;
    }

    protected override void AddEventListener()
    {
        base.AddEventListener();
        //EventMgr.AddEventListener(LoginEvent.JiGuang_Login, this, "OnJiGuangLogin");
        EventMgr.AddEventListener<string>(SDKEvent.JiGuang_Phone_GetKey, GetSmsCode);
        EventMgr.AddEventListener<string, string>(SDKEvent.JiGuang_Phone_Login, OnJiGuangPhoneLogin);
        EventMgr.AddEventListener<string>(SDKEvent.SDK_JiGuang_Result, OnJiGuangResult);
        EventMgr.AddEventListener<string>(SDKEvent.SDK_JiGuang_CodeOk, OnJiGuangCodeOk);
        EventMgr.AddEventListener<string>(SDKEvent.SDK_JiGuang_CodeFail, OnJiGuangCodeFail);
    }

    protected override void RemoveEventListener()
    {
        base.RemoveEventListener();
        //EventMgr.RemoveEventListener(LoginEvent.JiGuang_Login, this, "OnJiGuangLogin");
        EventMgr.RemoveEventListener<string>(SDKEvent.JiGuang_Phone_GetKey, GetSmsCode);
        EventMgr.RemoveEventListener<string, string>(SDKEvent.JiGuang_Phone_Login, OnJiGuangPhoneLogin);
        EventMgr.RemoveEventListener<string>(SDKEvent.SDK_JiGuang_Result, OnJiGuangResult);
        EventMgr.RemoveEventListener<string>(SDKEvent.SDK_JiGuang_CodeOk, OnJiGuangCodeOk);
        EventMgr.RemoveEventListener<string>(SDKEvent.SDK_JiGuang_CodeFail, OnJiGuangCodeFail);
    }

    /// <summary> 获得验证码 </summary>
    private void GetSmsCode(string phoneNumber)
    {
        jiGuangSDK?.GetSMSSDKCode(phoneNumber);
    }

    private void OnJiGuangResult(string result)
    {
        CodeInfo map = result?.ParseJson<CodeInfo>();
        //3000 代表获取验证码成功，
        //8000 初始化成功
        //7000 代表preLogin获取成功
        //6000 代表 loginToken 获取成功，6001 代表 loginToken 获取失败
        Debuger.LogError($"code={map.code}, |||| result={result}");

        if (map.code == 3000)
        {
            JiGuangCode = map.msg;
            EventMgr.Dispatch(SDKEvent.JiGuang_Phone_GetCode_CallBack, map.code, JiGuangCode);
        }
    }

    private void OnJiGuangCodeOk(string result)
    {
        CheckCodeOk map = result?.ParseJson<CheckCodeOk>();
        Debuger.LogError($"[极光登录成功] code={map.code}, phonenum={map.msg}, result={result}");

        string openId = map.msg;
        string phoneNumber = map.msg;
        LocalDataMgr.SetStr(LocalKey.OpenID, openId);
        LocalDataMgr.SetStr(LocalKey.PhoneNumber, phoneNumber);
        LocalDataMgr.SetBool(LocalKey.NotFirstLogin, true);
        OnReqLoginByCore(openId, openId, openId, phoneNumber);
    }

    private void OnJiGuangCodeFail(string result)
    {
        CodeInfo map = result?.ParseJson<CodeInfo>();
        Debuger.LogError($"[极光登录失败] code={map.code}, result={result}");
        PopView.Instance.Show(map.msg?.ToString());
    }

    /// <summary> 验证验证码 </summary>
    private void OnJiGuangPhoneLogin(string phoneNumber, string code)
    {
        jiGuangSDK?.CheckSmsCodeAsyn(phoneNumber, code);
    }
}
