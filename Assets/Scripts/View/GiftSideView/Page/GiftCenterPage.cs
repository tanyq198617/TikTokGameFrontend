using System;
using System.Collections;
using System.Collections.Generic;
using Org.BouncyCastle.Utilities.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 红蓝方送小礼物弹窗脚本
/// </summary>
public class GiftCenterPage : AItemPageBase
{
    private GameObject uiitem_blueplayerGiftTip;
    private Transform redGiftTweenStart;
    private Transform redGiftTweenEnd;
    private Transform blueGiftTweenStart;
    private Transform blueGiftTweenEnd;
    
    
    
    private Queue<PlayerGiftTip> giftTipQueue;

    private HashSet<PlayerGiftTip> _hashSet;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        uiitem_blueplayerGiftTip = UIUtility.Control("uiitem_blueplayerGiftTip", m_gameobj);
        redGiftTweenStart = UIUtility.Control("redGiftTweenStartPoint", Trans);
        redGiftTweenEnd = UIUtility.Control("redGiftTweenEndPoint", Trans);
        blueGiftTweenStart = UIUtility.Control("blueGiftTweenStartPoint", Trans);
        blueGiftTweenEnd = UIUtility.Control("blueGiftTweenEndPoint", Trans);
        giftTipQueue = new Queue<PlayerGiftTip>();
        uiitem_blueplayerGiftTip.SetActiveEX(false);
        _hashSet = new HashSet<PlayerGiftTip>();
    }

    public void RefreshSideTip(string openId, string giftId, long giftCount)
    {
        PlayerGiftTip sideTip = GetSmallGiftBase();
        sideTip.IsActive = true;
        sideTip.RefreshContent(openId,giftId,giftCount,RecycleSide);
        _hashSet.Add(sideTip);
    }

    /// <summary> 获取一个玩家送小礼物的脚本 /// </summary>
    private PlayerGiftTip GetSmallGiftBase()
    {
        PlayerGiftTip tip = null;
        if (giftTipQueue.Count > 0)
        {
            tip = giftTipQueue.Dequeue();
        }
        else
        {
            tip = UIUtility.CreatePage<PlayerGiftTip>(uiitem_blueplayerGiftTip, Trans);
        }
        tip.SetTweenPoint(redGiftTweenStart.localPosition, redGiftTweenEnd.localPosition, blueGiftTweenStart.localPosition,
            blueGiftTweenEnd.localPosition);
        return tip;
    }

    /// <summary> 回收玩家播放送礼完成后的预制体和脚本 /// </summary>
    private void RecycleSide(PlayerGiftTip tip)
    {
        tip.IsActive = false;
        giftTipQueue.Enqueue(tip);
        _hashSet.Remove(tip);
    }

    public override void Close()
    {
        base.Close();
        foreach (var item in _hashSet)
        {
            item.IsActive = false;
            giftTipQueue.Enqueue(item);
        }
        _hashSet.Clear();
    }
}