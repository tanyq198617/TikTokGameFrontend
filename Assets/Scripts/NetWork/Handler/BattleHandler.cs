using System.Collections;
using System.Collections.Generic;
using GameNetwork;
using Sproto;
using SprotoType;
using UnityEngine;

/// <summary>
/// 战斗相关消息
/// </summary>
[TcpMsgHandle]
public class BattleHandler : ATcpHandler
{
    public override void OnRegister()
    {
        Register<send_douyin_choose_side.request>(S2CProtocol.send_douyin_choose_side.Tag, Send_douyin_choose_side);
        Register<send_douyin_comment.request>(S2CProtocol.send_douyin_comment.Tag, Send_douyin_comment);
        Register<send_douyin_gift.request>(S2CProtocol.send_douyin_gift.Tag, Send_douyin_gift);
        Register<send_douyin_like.request>(S2CProtocol.send_douyin_like.Tag, Sen_douyin_like);
        Register<send_douyin_king_spec.request>(S2CProtocol.send_douyin_king_spec.Tag, S2CKingWeatherSpec);
        Register<send_douyin_family_spec.request>(S2CProtocol.send_douyin_family_spec.Tag, Send_douyin_family_spec);
        Register<send_douyin_win_combo_content.request>(S2CProtocol.send_douyin_win_combo_content.Tag, Send_douyin_win_combo_content);
    }

    /// <summary>
    /// 请求开始游戏
    /// </summary>
    public void ReqGameStart()
    {
        NetMgr.Instance.SendTo<game_start.request, C2SProtocol.game_start>((req) => { }, S2C_RetGameStart);
    }

    /// <summary>
    /// 服务器返回 开始游戏
    /// </summary>
    private void S2C_RetGameStart(SprotoTypeBase data)
    {
        CityGlobal.Instance.IsOver = false;
        (data as game_start.response).LogMessageMembers($"game_start.response"); 
        EventMgr.Dispatch(BattleEvent.Battle_GameIsStart);
    }

    /// <summary>
    /// [S2C] 选择阵营 
    /// </summary>
    public void Send_douyin_choose_side(send_douyin_choose_side.request msg)
    {
        if(CityGlobal.Instance.IsOver) return;
        PlayerModel.Instance.S2C_Choose_Side(msg.list, false);
    }

    public void Simulate_douyin_choose_side(send_douyin_choose_side.request msg)
    {
        if(CityGlobal.Instance.IsOver) return;
        PlayerModel.Instance.S2C_Choose_Side(msg.list, true);
    }

    /// <summary>
    /// [S2C] 用户评论 
    /// </summary>
    public void Send_douyin_comment(send_douyin_comment.request msg)
    {
        if(CityGlobal.Instance.IsOver) return;
        PlayerModel.Instance.S2C_OnComment(msg.list);
    }

    /// <summary>
    /// [S2C] 送礼物 
    /// </summary>
    public void Send_douyin_gift(send_douyin_gift.request msg)
    {
        if(CityGlobal.Instance.IsOver) return;
        PlayerModel.Instance.S2C_OnGift(msg.list);
    }

    /// <summary>
    /// [S2C] 玩家点赞 
    /// </summary>
    public void Sen_douyin_like(send_douyin_like.request msg)
    {
        if(CityGlobal.Instance.IsOver) return;
        PlayerModel.Instance.S2C_OnLike(msg.list);
    }

    /// <summary>
    /// [S2C] 连胜数据 
    /// </summary>
    public void Send_douyin_win_combo_content(send_douyin_win_combo_content.request msg)
    {
        //TODO 连胜内容  msg.content
        if(CityGlobal.Instance.IsOver) return;
        PlayerModel.Instance.S2C_WinComboContent(msg.openid, msg.win_combo);
        msg.LogMessageMembers("send_douyin_win_combo_content.request");
    }

    /// <summary>
    /// [S2C] 天气
    /// 皇帝特殊内容
    /// </summary>
    public void S2CKingWeatherSpec(send_douyin_king_spec.request msg)
    {
    }

    /// <summary>
    /// [S2C] 家族 
    /// </summary>
    public void Send_douyin_family_spec(send_douyin_family_spec.request msg)
    {
    }
}
