using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SDKEvent
{
    public const string SDK_JiGuang_Result = "SDKJiGuangResult";
    public const string SDK_JiGuang_CodeOk = "SDKJiGuangCodeOk";
    public const string SDK_JiGuang_CodeFail = "SDKJiGuangCodeFail";
}

public class JiGuangSDKHandler : MonoBehaviour
{
    private const string javaClassStr = "com.trainfire.jgaar.JiGuangManager";
    private const string javaActiveStr = "currentActivity";

    private const string androidInitMethod = "Init";
    private const string androidIsVerifyEnableMethod = "IsVerifyEnable";
    private const string androidClearMethod = "Clear";
    private const string androidLoginAuthMethod = "LoginAuth";
    private const string androidPreLoginMethod = "PreLogin";
    private const string androidGetSmsCodeMethod = "GetSmsCode";
    private const string androidGetSMSSDKCodeMethod = "GetSMSSDKCode";
    private const string androidCheckSmsCodeAsynMethod = "CheckSmsCodeAsyn";

    private AndroidJavaClass javaClass;

    private AndroidJavaObject _javaActive;
    protected AndroidJavaObject javaActive
    {
        get
        {
            if (_javaActive == null)
                _javaActive = new AndroidJavaObject(javaClassStr);
            return _javaActive;
        }
    }

    private void Start()
    {
        if (_javaActive == null)
            _javaActive = new AndroidJavaObject(javaClassStr);
    }

    public void Init(bool isDebug)
    {
        object[] objs = new object[] { isDebug, nameof(JiGuangSDKHandler), nameof(JiGuangInitCallback) };
        javaActive?.Call(androidInitMethod, objs);
    }

    public void LoginAuth()
    {
        object[] objs = new object[] { nameof(JiGuangInitCallback) };
        javaActive?.Call(androidLoginAuthMethod, objs);
    }

    public bool IsInitSuccess()
    {
        return javaActive.Call<bool>(androidInitMethod);
    }

    public bool IsVerifyEnable()
    {
        return javaActive.Call<bool>(androidIsVerifyEnableMethod);
    }

    public void GetSmsCode(string phoneNumber)
    {
        object[] objs = new object[] { phoneNumber, null, null };
        javaActive.Call(androidGetSmsCodeMethod, objs);
    }

    public void GetSMSSDKCode(string phoneNumber, string tmep_id = "1")
    {
        object[] objs = new object[] { phoneNumber, tmep_id };
        javaActive.Call(androidGetSMSSDKCodeMethod, objs);
    }

    public void CheckSmsCodeAsyn(string phoneNumber, string code)
    {
        object[] objs = new object[] { phoneNumber, code, nameof(JiGuangCodeOkCallback), nameof(JiGuangCodeFailCallback) };
        javaActive.Call(androidCheckSmsCodeAsynMethod, objs);
    }

    /// <summary>
    /// 极光安卓回调
    /// </summary>
    /// <param name="result"></param>
    public void JiGuangInitCallback(string result)
    {
        Debuger.LogError($"[极光登录][Android2Unity]: {result}");
        EventMgr.Dispatch(SDKEvent.SDK_JiGuang_Result, result);
    }

    public void JiGuangCodeOkCallback(string result)
    {
        Debuger.LogError($"[极光验证成功][Android2Unity]: {result}");
        EventMgr.Dispatch(SDKEvent.SDK_JiGuang_CodeOk, result);
    }

    public void JiGuangCodeFailCallback(string result)
    {
        Debuger.LogError($"[极光验证失败][Android2Unity]: {result}");
        EventMgr.Dispatch(SDKEvent.SDK_JiGuang_CodeFail, result);
    }
}

