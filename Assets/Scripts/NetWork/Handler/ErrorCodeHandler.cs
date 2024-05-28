using System.Collections;
using System.Collections.Generic;
using GameNetwork;
using SprotoType;
using UnityEngine;

[TcpMsgHandle]
public class ErrorCodeHandler : ATcpHandler
{
    public override void OnRegister()
    {
        EventMgr.AddEventListener(SocketEvent.Socket_Disconnect, OnDisconnect);
        EventMgr.AddEventListener(SocketEvent.Socket_TimeOut, OnSocketTimeOut);
        EventMgr.AddEventListener<string>(SocketEvent.Socket_CreateConnect, OnSocketSuccess);
        EventMgr.AddEventListener(SocketEvent.Socket_Connection_Closed, OnConnectionClosed);
        EventMgr.AddEventListener(SocketEvent.Socket_Failed, OnSocketFailed);
        
        Register<kick_user.request>(S2CProtocol.kick_user.Tag, OnKickUser);
    }

    private void OnKickUser(kick_user.request msg)
    {
        GameConst.IsLoginGame = false;
        Debuger.LogError($"玩家将被踢下线!,原因：{msg.reason}");
        Loom.QueueOnMainThread(_ => KickUserAlter(), null);
    }
    
    private void KickUserAlter()
    {
        AlterView.Instance.ShowAlter(AlterType.OK, $"玩家被踢下线", () =>
        {
            GameStageMachine.ChangeState<LoginStage>();
        });
    }

    /// <summary> 网络链接失败 </summary>
    private void OnSocketFailed()
    {
        GameConst.IsLoginGame = false;
        Debuger.LogError($"网络链接被关闭!");
        Loom.QueueOnMainThread(_ => SocketFailedAlter(), null);
    }

    private void SocketFailedAlter()
    {
        AlterView.Instance.ShowAlter(AlterType.OK, $"网络链接被断开了", () =>
        {
            WaitCircleView.Instance.OnLock();
            EventMgr.Dispatch(GameEvent.Game_TryConnectServer);
            NetMgr.Instance.ConnectServer();
        });
    }

    /// <summary> 网络链接被关闭 </summary>
    private void OnConnectionClosed()
    {
        GameConst.IsLoginGame = false;
        Debuger.LogError($"网络链接被关闭!");
    }

    /// <summary> 成功连接网络 </summary>
    private void OnSocketSuccess(string address)
    {
        Debuger.LogWarning($"成功连接网络! address={address}"); 
        WaitCircleView.Instance.UnLock();
        NetMgr.Instance.SendClientKey();
        EventMgr.Dispatch(GameEvent.Game_Socket_SendClientKey);
    }

    /// <summary> 网络链接断开 </summary>
    private void OnDisconnect()
    {
        GameConst.IsLoginGame = false;
        Debuger.LogError($"网络链接断开");
    }
    
    /// <summary> 网络链接超时 </summary>
    private void OnSocketTimeOut()
    {
        GameConst.IsLoginGame = false;
        Debuger.LogError($"网络链接超时");
    }
}
