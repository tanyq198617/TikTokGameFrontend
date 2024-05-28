using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using HotUpdateScripts;
using SprotoType;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 本地模拟服务器数据
/// </summary>
public class SimulateModel : Singleton<SimulateModel>
{

    /// <summary> 选择的阵营 </summary>
    public CampType CampType = CampType.蓝;
    
    /// <summary>
    /// 模拟玩家入座 
    /// </summary>
    public void Simulate_send_douyin_choose_side(int count)
    {
        var msg = new send_douyin_choose_side.request
        {
            list = new List<douyin_choose_side>()
        };
        
        for (int i = 0; i < count; i++)
        {
            var key = $"{CampType.ToString()}_{StringUtility.CreateRadmonStr(8)}";
            var robotData = TRobotDataManager.Instance.GetRobotData();
            msg.list.Add(new douyin_choose_side()
            {
                openid = key,
                content = CampType.ToInt().ToString(),
                avatar_url = robotData.avatar_url,
                nickname = robotData.nickname,
                king = 0,
                small_king = 0,
                world_score = 0,
                world_rank = 0,
                win_combo = Random.Range(0, 11),
                family = string.Empty,
                win_lock = 0,
                rejoin_acc = 0,
            });
        }

        NetMgr.GetHandler<BattleHandler>().Simulate_douyin_choose_side(msg);
    }
    
    /// <summary>
    /// 模拟玩家送礼 
    /// </summary>
    public void Simulate_Send_douyin_gift(PlayerInfo info, string gift_id, long count)
    {
        if(info == null) return;
        var msg = new send_douyin_gift.request
        {
            list = new List<douyin_gift>
            {
                new douyin_gift()
                {
                    openid = info.openid,
                    avatar_url = info.avatar_url,
                    nickname = info.nickname,
                    king = info.king,
                    small_king = info.small_king,
                    family = info.family,
                    win_combo = info.win_combo,
                    gift_id = gift_id,
                    gift_num = count,
                    gift_total = info.GetTotal(gift_id) + count,
                }
            }
        };

        NetMgr.GetHandler<BattleHandler>().Send_douyin_gift(msg);
    }
    

    /// <summary>
    /// 模拟玩家点赞
    /// </summary>
    public void Simulate_Sen_douyin_like(PlayerInfo info, int count = 1)
    {
        if(info == null) return;
        var msg = new send_douyin_like.request
        {
            list = new List<douyin_like>
            {
                new douyin_like()
                {
                    openid = info.openid,
                    avatar_url = info.avatar_url,
                    nickname = info.nickname,
                    king = info.king,
                    small_king = info.small_king,
                    family = info.family,
                    win_combo = info.win_combo,
                    like_num = count
                }
            }
        };

        NetMgr.GetHandler<BattleHandler>().Sen_douyin_like(msg);
    }
    
    /// <summary>
    /// 模拟玩家评论
    /// </summary>
    public void Simulate_Send_douyin_comment(PlayerInfo info, string content)
    {
        if(info == null) return;
        long win = info.win_combo - 1;
        var msg = new send_douyin_comment.request
        {
            list = new List<douyin_comment>
            {
                new douyin_comment()
                {
                    openid = info.openid,
                    avatar_url = info.avatar_url,
                    nickname = info.nickname,
                    king = info.king,
                    small_king = info.small_king,
                    family = info.family,
                    win_combo = win,
                    content = content,
                }
            }
        };
        NetMgr.GetHandler<BattleHandler>().Send_douyin_comment(msg);
    }
}
