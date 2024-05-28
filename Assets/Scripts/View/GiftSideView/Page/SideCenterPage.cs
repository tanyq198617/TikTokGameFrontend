using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 红蓝方玩家坐下弹窗脚本
/// </summary>
public class SideCenterPage : AItemPageBase
{
    private GameObject uiitem_blueplayerSideTip;
    private Transform redSideTweenStart;
    private Transform redSideTweenEnd;
    private Transform blueSideTweenStart;
    private Transform blueSideTweenEnd;
    private Queue<PlayerSideTip> sideQueue;

    private HashSet<PlayerSideTip> _hashSet;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        uiitem_blueplayerSideTip = UIUtility.Control("uiitem_playerSideTip", m_gameobj);
        redSideTweenStart = UIUtility.Control("redSideTweenStartPoint", Trans);
        redSideTweenEnd = UIUtility.Control("redSideTweenEndPoint", Trans);
        blueSideTweenStart = UIUtility.Control("blueSideTweenStartPoint", Trans);
        blueSideTweenEnd = UIUtility.Control("blueSideTweenEndPoint", Trans);
        sideQueue = new Queue<PlayerSideTip>();
        uiitem_blueplayerSideTip.SetActiveEX(false);
        _hashSet = new HashSet<PlayerSideTip>();
    }

    public void RefreshSideTip(PlayerInfo info)
    {
        PlayerSideTip sideTip = GetSideBase();
        sideTip.IsActive = true;
        sideTip.RefreshContent(info, RecycleSide);
        _hashSet.Add(sideTip);
    }

    /// <summary> 获取一个玩家坐下的脚本 /// </summary>
    private PlayerSideTip GetSideBase()
    {
        PlayerSideTip tip = null;
        if (sideQueue.Count > 0)
        {
            tip = sideQueue.Dequeue();
        }
        else
        {
            tip = UIUtility.CreatePage<PlayerSideTip>(uiitem_blueplayerSideTip, Trans);
        }
        tip.SetTweenPoint(redSideTweenStart.localPosition, redSideTweenEnd.localPosition, blueSideTweenStart.localPosition,
            blueSideTweenEnd.localPosition);
        return tip;
    }

    /// <summary> 回收玩家坐下完成后的预制体和脚本 /// </summary>
    private void RecycleSide(PlayerSideTip tip)
    {
        tip.IsActive = false;
        sideQueue.Enqueue(tip);
        _hashSet.Remove(tip);
    }

    public override void Close()
    {
        base.Close();
        foreach (var item in _hashSet)
        {
            item.IsActive = false;
            sideQueue.Enqueue(item);
        }
        _hashSet.Clear();
    }
}