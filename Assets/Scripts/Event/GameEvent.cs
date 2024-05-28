using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent
{
    public const string Version_NewMessage = "VersionNewMessage";

    /// <summary> 状态机-->准备改变状态 </summary>
    public const string NodeStartChange = "NodeStartChange";

    /// <summary> 状态机-->状态改变完成 </summary>
    public const string NodeChanged = "NodeChanged";

    #region 登录

    /// <summary> 登录-->登录成功 </summary>
    public const string Login_Succeed = "LoginSucceed";
    
    /// <summary> 登录-->登录失败 </summary>
    public const string Login_Fail = "LoginFail";
    
    /// <summary> 网络-->网络重连 </summary>
    public const string Game_TryConnectServer = "GameTryConnectServer";
    
    /// <summary> 网络-->网络发送密码本 </summary>
    public const string Game_Socket_SendClientKey = "GameSocketSendClientKey";

    #endregion
    
}
