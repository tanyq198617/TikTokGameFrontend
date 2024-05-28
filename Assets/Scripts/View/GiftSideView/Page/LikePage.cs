using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary> 点赞提示控制脚本 /// </summary>
public class LikePage : GiftSideCenterPageBase
{
    private Dictionary<CampType, PlayerTipItemBase> playItemIng;
    private Queue<PlayerInfo> redPlayerInfoQueue;
    private Queue<PlayerInfo> bluePlayerInfoQueue;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        uiitem_Tip = UIUtility.Control("uiitem_playerLikeTip", m_gameobj);
        playItemIng = new Dictionary<CampType, PlayerTipItemBase>();
        uiitem_Tip.SetActiveEX(false);
        redPlayerInfoQueue = new Queue<PlayerInfo>();
        bluePlayerInfoQueue = new Queue<PlayerInfo>();
    }

    public void RefreshSideTip(PlayerInfo info)
    {
        if (IsPlayLikeTip(info.campType))
        {
            if (info.campType == CampType.红)
                redPlayerInfoQueue.Enqueue(info);
            else
                bluePlayerInfoQueue.Enqueue(info);
        }
        else
        {
            PlayTip(info);
        }
    }

    private void PlayTip(PlayerInfo info)
    {
        var sideTip = GetSmallGiftBase<PlayerLikeTipItem>();
        sideTip.RefreshContent(info, RecycleSide);
        _hashSet.Add(sideTip);
        playItemIng[info.campType] = sideTip;     
    }
    
    /// <summary> 判断是否播放点赞提示 /// </summary>
    private bool IsPlayLikeTip(CampType camp)
    {
        return playItemIng.TryGetValue(camp, out _);
    }

    private void CheckPlayTip(CampType camp)
    {
        if (camp == CampType.红)
        {
            if (redPlayerInfoQueue.Count <= 0) return;
            PlayerInfo info = redPlayerInfoQueue.Dequeue();
            PlayTip(info);
        }
        else
        {
            if (bluePlayerInfoQueue.Count <= 0) return;
            PlayerInfo info = bluePlayerInfoQueue.Dequeue();
            PlayTip(info);
        }
    }
    
    /// <summary> 回收玩家播放送礼完成后的预制体和脚本 /// </summary>
    private void RecycleSide(PlayerTipItemBase tip)
    {
        CampType camp = tip.info.campType;
        tipQueue.Enqueue(tip);
        _hashSet.Remove(tip);
        playItemIng.Remove(camp);
        tip.IsActive = false;
        CheckPlayTip(camp);
    }

    public override void Close()
    {
        base.Close();
        redPlayerInfoQueue.Clear();
        bluePlayerInfoQueue.Clear();
        
        if (playItemIng != null)
        {
            List<PlayerTipItemBase> balls = playItemIng.Values.ToList();

            for (var i = balls.Count - 1; i >= 0; i--)
            {
                balls[i].IsActive = false;
            }
            playItemIng.Clear();
        }
    }

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener<PlayerInfo>(BattleEvent.Battle_PlayLikeTip, RefreshSideTip);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener<PlayerInfo>(BattleEvent.Battle_PlayLikeTip, RefreshSideTip);
    }
}
