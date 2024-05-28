using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 界面修改为每周排行榜
/// </summary>
public class RankWorldRankRootPage : AItemPageBase
{
    private RankWorldRankRootThreePage threePage;
    private RankWorldRankRootTwoPage twoPage;
    private RankWorldRankRootOnePage onePage;

    private UILayoutGroup<AWorldRankItem, RankInfo> rankLayoutGroup;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        /*threePage = UIUtility.CreatePageNoClone<RankWorldRankRootThreePage>(RectTrans, "uiitem_three");
        twoPage = UIUtility.CreatePageNoClone<RankWorldRankRootTwoPage>(RectTrans, "uiitem_two");
        onePage = UIUtility.CreatePageNoClone<RankWorldRankRootOnePage>(RectTrans, "uiitem_one");*/ 

        rankLayoutGroup = new UILayoutGroup<AWorldRankItem, RankInfo>();
        rankLayoutGroup.OnInit(UIUtility.Control("Content", m_gameobj));
    }

    public override void Close()
    {
        rankLayoutGroup.Clear();
    }

    private void ResetRankData(int rankType)
    {
        rankLayoutGroup.OnReposition();
        rankLayoutGroup.DefaultRefresh(RankModel.Instance.GetRankList(rankType));
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