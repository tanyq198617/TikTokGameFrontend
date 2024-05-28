using GameNetwork;
using SprotoType;
using UnityEngine;

[TcpMsgHandle]
public class GmHandler : ATcpHandler
{
    public override void OnRegister()
    {
    }

  

    public void ReqSetRoomId()
    {
        ReqGm("sri 100","设置房间号");
    }

    public void Req_douyin_new_user()
    {
        ReqGm("douyin_new_user","生成新玩家");
    }

    public void Req_add_score()
    {
        ReqGm("douyin_add_score 100000","加分");
    }

    public void Req_douyin_top_acc()
    {
        ReqGm("douyin_top_acc","榜一大哥刷礼物");
    }
 
    /// <summary> 评论 /// </summary>
    public void Req_douyin_content(string openid,string content)
    {
        ReqGm($"douyin_content {openid} {content}","模拟gm评论");
    }
    
    /// <summary> 设置玩家连胜/// </summary>
    public void Req_douyin_win_combo(string openid,string wiwnNum)
    {
        ReqGm($"set_win_combo {openid} {wiwnNum}","模拟gm设置玩家连胜");
    }
    
    private void ReqGm(string cmd,string log)
    {
        void C2S_ReqLogin(gm.request req)
        {
            req.cmd = cmd;
        }
        
        NetMgr.Instance.SendTo<gm.request, C2SProtocol.gm>(C2S_ReqLogin,
            (data) =>
            {
                gm.response data1 = data as gm.response;
                Debuger.Log($"{log}{data1.error_code}");
            });
    }
    

}
