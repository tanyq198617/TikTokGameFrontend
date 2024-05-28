using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankWinningStreakPage : AItemPageBase
{
    private RankWinningStreakOnePage threePage;
    private RankWinningStreakOnePage twoPage;
    private RankWinningStreakOnePage onePage;

    private UILayoutGroup<AWinningStreakRankItem, RankInfo> rankLayoutGroup;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        threePage = UIUtility.CreatePageNoClone<RankWinningStreakOnePage>(RectTrans, "uiitem_three");
        twoPage = UIUtility.CreatePageNoClone<RankWinningStreakOnePage>(RectTrans, "uiitem_two");
        onePage = UIUtility.CreatePageNoClone<RankWinningStreakOnePage>(RectTrans, "uiitem_one");
        rankLayoutGroup = new UILayoutGroup<AWinningStreakRankItem, RankInfo>();
        rankLayoutGroup.OnInit(UIUtility.Control("Content", m_gameobj));
    }

    public override void Refresh()
    {
        threePage.IsActive = false;
        twoPage.IsActive = false;
        onePage.IsActive = false;
        base.Refresh();
    }

    public override void Close()
    {
        rankLayoutGroup.Clear();
    }

    private void ResetRankData(int rankType)
    {
        onePage.RefreshContent(RankModel.Instance.FindRankInfo(rankType, 0));
        twoPage.RefreshContent(RankModel.Instance.FindRankInfo(rankType, 1));
        threePage.RefreshContent(RankModel.Instance.FindRankInfo(rankType, 2));
        rankLayoutGroup.DefaultRefresh(RankModel.Instance.GetRankListWithoutThree(rankType));
    }

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener<int>(BattleEvent.Battle_S2C_Rank_End, ResetRankData);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener<int>(BattleEvent.Battle_S2C_Rank_End, ResetRankData);
    }
}