using System.Collections.Generic;
using HotUpdateScripts;
using UnityEngine;

/// <summary> 连胜点弹窗界面 /// </summary>
public class WinningPointPage : GiftSideCenterPageBase
{
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        uiitem_Tip = UIUtility.Control("uiitem_winningPointTip", m_gameobj);
        uiitem_Tip.SetActiveEX(false);
    }

    private void RefreshWinningPointTip(PlayerInfo info, TWinningPointExpend expend)
    {
        var tipItem = GetSmallGiftBase<WinningPointTipItem>();
        tipItem.RefreshWinningPointContent(info, expend, RecycleSide);
        _hashSet.Add(tipItem);
    }

    /// <summary> 回收玩家播放送礼完成后的预制体和脚本 /// </summary>
    private void RecycleSide(PlayerTipItemBase tip)
    {
        CampType camp = tip.info.campType;
        tipQueue.Enqueue(tip);
        _hashSet.Remove(tip);
        tip.IsActive = false;
    }

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener<PlayerInfo, TWinningPointExpend>(BattleEvent.Battle_WinningPoint_ExpendTip,
            RefreshWinningPointTip);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener<PlayerInfo, TWinningPointExpend>(BattleEvent.Battle_WinningPoint_ExpendTip,
            RefreshWinningPointTip);
    }
}