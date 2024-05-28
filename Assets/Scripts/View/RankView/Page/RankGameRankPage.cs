using System;
using System.Collections;
using System.Collections.Generic;
using SprotoType;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankGameRankPage : AItemPageBase
{
    private UILayoutGroup<AGameRankItem, RankInfo> rankItemGroup;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        rankItemGroup = new UILayoutGroup<AGameRankItem, RankInfo>();
        rankItemGroup.OnInit(UIUtility.Control("Content",m_gameobj));
    }
    
    public override void Close()
    {
        rankItemGroup.Clear();
    }
    
    private void ResetRankData(int rankType)
    {
        rankItemGroup.OnReposition();
        rankItemGroup.DefaultRefresh(RankModel.Instance.GetRankList(rankType));
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
