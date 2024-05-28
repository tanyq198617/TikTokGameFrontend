using GameNetwork;
using Sproto;
using SprotoType;
using UnityEngine;

[TcpMsgHandle]
public class LoginHandler : ATcpHandler
{
    public override void OnRegister()
    {
        Register<send_room_info.request>(S2CProtocol.send_room_info.Tag, OnRetRoomInfo);
    }

    /// <summary>
    /// 获取房间信息
    /// </summary>
    private void OnRetRoomInfo(send_room_info.request msg)
    {
        RoomModel.Instance.S2C_RetRoomInfo(msg.room_id);
        msg.LogMessageMembers($"send_room_info.request");
    }

    public void ReqLogin()
    {
        if (GameConst.IsLoginGame)
        {
            Debuger.LogError($"已经登录了，请不要重复登录游戏");
            return;
        }

        string token = Boot.GetToKen();
        string Channel = Boot.GetChannel();
        if (string.IsNullOrEmpty(token))
        {
            AlterView.Instance.ShowAlter(AlterType.OK,$"传递的Token为空了，请检查相关逻辑和配置", Boot.Quit);
            return;
        }

        token = token.Substring(7, token.Length - 7);
        string openid = SystemInfo.deviceUniqueIdentifier;
        string Appver = Boot.GetAppver("Appver");
        
        void C2S_ReqLogin(login.request req)
        {
            req.account_type = Channel; //"fastlogin";
            req.openid = openid;
            req.token = token;
            req.device_info = "{}";
            req.appver = Appver;
            req.extra = GameConst.IsFristLogin ? "{}" : "{\"reconnect\":\"true\"}";
        }

        GameConst.IsLoginGame = true;
        NetMgr.Instance.SendTo<login.request, C2SProtocol.login>(C2S_ReqLogin, S2C_RetLogin);
    }
    
    /// <summary>
    /// 服务器返回登录
    /// </summary>
    private void S2C_RetLogin(SprotoTypeBase data)
    {
        var msg = data as login.response;
        
        if (!string.IsNullOrEmpty(msg.user_info))
        {
            GameConst.IsLoginGame = true;
            GameConst.IsFristLogin = false;
            Debuger.LogError($"{msg.user_info}\n{msg.token}");
            EventMgr.Dispatch(GameEvent.Login_Succeed);
        }
        else
        {
            Debuger.LogError($"登录服务器失败了! code={msg.error_code} Message={msg.error_msg}");
            GameConst.IsLoginGame = false;
            AlterView.Instance.ShowAlter(AlterType.OK, $"登录失败了,请检查逻辑或者配置!!", () =>
            {
                EventMgr.Dispatch(GameEvent.Login_Fail); 
            });
        }
        msg.LogMessageMembers($"login.response");
    }
}
