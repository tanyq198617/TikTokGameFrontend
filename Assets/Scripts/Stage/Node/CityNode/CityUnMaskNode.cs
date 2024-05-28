using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityUnMaskNode : AStateNode
{
    protected override void Begin()
    {
        //MainBottomView.Instance.OpenPage(2, Call);
        UIMgr.Instance.ShowUICall(UIPanelName.BattleView, Call);
    }

    private void Call()
    {
        UIMgr.Instance.CloseUI(UIPanelName.LoadingView);
        UIMgr.Instance.ShowUI(UIPanelName.GiftSideView);
        UIMgr.Instance.ShowUI(UIPanelName.BuffView);
        UIMgr.Instance.ShowUI(UIPanelName.SettingView);
        IsComplete = true;
    }

    public override void SysUpdate()
    {
        if (IsComplete)
            _machine.RunNextNode(this);
    }
}
