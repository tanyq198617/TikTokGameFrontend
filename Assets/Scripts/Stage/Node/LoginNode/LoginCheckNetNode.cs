using System.Collections;
using System.Collections.Generic;
using GameNetwork;
using UnityEngine;

public class LoginCheckNetNode : AStateNode
{
    private int frame = 1;
    
    protected override void Begin()
    {
        frame = 3;
        if (NetMgr.Instance.IsConnected)
            OnConnected();
        else
            OnConnectFailed();
    }

    private void OnConnectFailed() 
    {
        frame = 3;
        AlterView.Instance.ShowAlter(AlterType.Ok_Cancel, "当前无法连接到服务器", NetMgr.Instance.ConnectServer, Boot.Quit, "网络提示", "重试", "退出");
    }

    private void OnConnected()
    {
        frame -= 3;
    }
    
    public override void SysUpdate()
    { 
        frame -= 1;
        if (frame <= 0)
        {
            _machine.RunNextNode(this);
        }
    }

    private void OnCreateConnect(string address) => OnConnected();

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener<string>(SocketEvent.Socket_CreateConnect, OnCreateConnect);
        EventMgr.AddEventListener(SocketEvent.Socket_Failed, OnConnectFailed);
        EventMgr.AddEventListener(SocketEvent.Socket_Disconnect, OnConnectFailed);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener<string>(SocketEvent.Socket_CreateConnect, OnCreateConnect);
        EventMgr.RemoveEventListener(SocketEvent.Socket_Failed, OnConnectFailed);
        EventMgr.RemoveEventListener(SocketEvent.Socket_Disconnect, OnConnectFailed);
    }
}
