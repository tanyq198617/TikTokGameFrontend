using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 结算节点
/// </summary>
public class BattleResultNode : AStateNode
{
    protected override void Begin()
    {
        UIMgr.Instance.ShowUI(UIPanelName.RankView);
    }

    protected override void End()
    {
        UIMgr.Instance.CloseUI(UIPanelName.RankView);
    }

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener(BattleEvent.Battle_RestartGame, OnRestartGame);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener(BattleEvent.Battle_RestartGame, OnRestartGame);
    }

    private void OnRestartGame()
    {
        _machine.ChangeState<BattleInitializeNode>();
    }
}
