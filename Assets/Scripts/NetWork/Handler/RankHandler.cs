using GameNetwork;
using Sproto;
using SprotoType;
using UnityEngine;

[TcpMsgHandle]
public class RankHandler : ATcpHandler
{
    public override void OnRegister()
    {
        Register<send_rank_data_start.request>(S2CProtocol.send_rank_data_start.Tag, Send_rank_data_start);
        Register<send_rank_data.request>(S2CProtocol.send_rank_data.Tag, Send_rank_data);
        Register<send_rank_data_end.request>(S2CProtocol.send_rank_data_end.Tag, Send_rank_data_end);
    }

    /// <summary>
    /// 服务器 开始发送排行榜信息
    /// </summary>
    private void Send_rank_data_start(send_rank_data_start.request msg)
    {
        // Debuger.Log($"[S2C] [排行榜] Send_rank_data_start");
        msg.LogMessageMembers("send_rank_data_start.request");
        RankModel.Instance.S2C_UpdateRankStart(msg.rank_type); 
        
        //广播【开始接收服务器排行榜消息】
        WaitCircleView.Instance.OnLock();
        EventMgr.Dispatch(BattleEvent.Battle_S2C_Rank_Start, (int)msg.rank_type);
        msg.LogMessageMembers("[S2C] send_rank_data_start.request");
    }

    /// <summary>
    /// 服务器 发送排行榜信息
    /// </summary>
    private void Send_rank_data(send_rank_data.request msg)
    {
        Debuger.Log($"[S2C] 排行榜类型:{msg.rank_type},排行榜个数:{msg.rank_data.Count},[排行榜] send_rank_data...");
        RankModel.Instance.S2C_UpdateRank(msg.rank_type, msg.rank_data);
    }

    /// <summary>
    /// 服务器 结束发送排行榜信息
    /// </summary>
    private void Send_rank_data_end(send_rank_data_end.request msg)
    {
        // Debuger.Log($"[S2C] [排行榜] Send_rank_data_end");
        msg.LogMessageMembers("send_rank_data_end.request");
        RankModel.Instance.S2C_UpdateRankEnd(msg.rank_type);
        WaitCircleView.Instance.UnLock();
        EventMgr.Dispatch(BattleEvent.Battle_S2C_Rank_End, (int)msg.rank_type);
    }

    /// <summary>
    /// 请求服务器排行榜数据
    /// </summary>
    public void ReqRank(ENUM_RANK_TYPE rankType)
    {
        if (RankModel.Instance.IsReqRank(rankType.ToInt()))
        {
            EventMgr.Dispatch(BattleEvent.Battle_S2C_Rank_End, rankType);
            return;
        }

        void C2S_ReqRankInfo(req_rank_info.request req)
        {
            req.rank_type = rankType.ToInt();
        }

        NetMgr.Instance.SendTo<req_rank_info.request, C2SProtocol.req_rank_info>(C2S_ReqRankInfo, S2C_RetRankInfo);
    }

    /// <summary>
    /// 服务器 返回排行榜数据
    /// </summary>
    private void S2C_RetRankInfo(SprotoTypeBase data)
    {
        // Debuger.Log($"[S2C] [排行榜]: req_rank_info");
        (data as req_rank_info.response)?.LogMessageMembers("req_rank_info.response");
    }
}