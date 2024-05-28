using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SDKEvent
{
    /// <summary> 登录成功 </summary>  
    public const string SDK_Login_Succeed = "SDKLoginSucceed";

    /// <summary> 帐号密码登录 </summary>  
    public const string Custom_Account_Login = "CustomAccountLogin";

    /// <summary> 腾讯邮件登录 </summary>  
    public const string Tencent_Enail_Login = "TencentEnailLogin";

    /// <summary> 腾讯邮件找回密码 </summary> 
    public const string Tencent_Enail_FindPwd = "TencentEnailFindPwd";

    /// <summary> 腾讯手机获取验证码 </summary> 
    public const string Tencent_Phone_GetKey = "TencentPhoneGetKey";

    /// <summary> 腾讯手机获取验证码返回 </summary> 
    public const string Tencent_Phone_GetCode_CallBack = "TencentPhoneGetCodeCallBack";

    /// <summary> 腾讯手机登录 </summary> 
    public const string Tencent_Phone_Login = "TencentPhoneLogin";

    /// <summary> 腾讯匿名登录 </summary> 
    public const string Tencent_Anonymity_Login = "TencentAnonymityLogin";



    /// <summary> 极光登录 </summary> 
    public const string JiGuang_Login = "JiGuangLogin";

    /// <summary> 极光登录手机获取验证码 </summary> 
    public const string JiGuang_Phone_GetKey = "JiGuangPhoneGetKey";

    /// <summary> 极光登录获取验证码返回 </summary> 
    public const string JiGuang_Phone_GetCode_CallBack = "JiGuangPhoneGetCodeCallBack";

    /// <summary> 极光手机登录 </summary> 
    public const string JiGuang_Phone_Login = "JiGuangPhoneLogin";
}
