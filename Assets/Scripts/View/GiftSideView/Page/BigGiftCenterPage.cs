using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 玩家送大礼物的弹窗
/// </summary>
public class BigGiftCenterPage : AItemPageBase
{
    private GameObject uiitem_bigGiftTip;
    private Queue<PlayerBigGiftTip> bigGiftQueue;
    private HashSet<PlayerBigGiftTip> _hashSet;


    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        uiitem_bigGiftTip = UIUtility.Control("uiitem_bigGiftTip", m_gameobj);
        bigGiftQueue = new Queue<PlayerBigGiftTip>();
        _hashSet = new HashSet<PlayerBigGiftTip>();
        uiitem_bigGiftTip.SetActiveEX(false);
    }

    public void RefreshSideTip(string openId, string giftId, long giftCount)
    {
        var sideTip = GetSmallGiftBase();
        _hashSet.Add(sideTip);
        sideTip.IsActive = true;
        sideTip.RefreshContent(openId, giftId, giftCount, RecycleSide);
    }

    /// <summary> 获取一个玩家送小礼物的脚本 /// </summary>
    private PlayerBigGiftTip GetSmallGiftBase()
    {
        PlayerBigGiftTip tip = null;
        if (bigGiftQueue.Count > 0)
        {
            tip = bigGiftQueue.Dequeue();
        }
        else
        {
            tip = UIUtility.CreatePage<PlayerBigGiftTip>(uiitem_bigGiftTip, Trans);
        }

        return tip;
    }

    /// <summary> 回收玩家播放送礼完成后的预制体和脚本 /// </summary>
    private void RecycleSide(PlayerBigGiftTip tip)
    {
        _hashSet.Remove(tip);
        tip.IsActive = false;
        bigGiftQueue.Enqueue(tip);
    }

    public override void Close()
    {
        base.Close();
        foreach (var item in _hashSet)
        {
            item.IsActive = false;
            bigGiftQueue.Enqueue(item);
        }

        _hashSet.Clear();
    }
}