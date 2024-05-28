using System.Collections;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

/// <summary>
/// 某条排行榜数据
/// </summary>
public class RankInfo
{
    /// <summary> 分数 </summary>
    public string score;
    
    /// <summary> 排名 </summary>
    public long rank;
    
    /// <summary> 玩家数据 </summary>
    public PlayerInfo player;
    
    /// <summary> 用户信息 </summary>
    public open_info info;
    
    /// <summary> 家族 </summary>
    public string family;

    /// <summary> 当前榜的世界积分 /// </summary>
    public long world_score;
    
    public static RankInfo Create(rank_info msg)
    {
        var info = ClassFactory.GetOrCreate<RankInfo>();
        info.score = msg.score;
        info.rank = msg.rank;
        info.player = PlayerModel.Instance.FindPlayer(msg.openid);
        info.info = msg.open_info;
        info.family = msg.family;
        info.world_score = info.info.world_score != null ? long.Parse(info.info.world_score) : 0;
        return info;
    }

    public void Clear()
    {
        score = string.Empty;
        rank = 0;
        player = null;
        info = null;
        family = string.Empty;
        world_score = 0;
        ClassFactory.Recycle(this);
    }
}
