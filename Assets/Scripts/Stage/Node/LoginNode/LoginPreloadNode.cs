using HotUpdateScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 登录前预
/// </summary>
public class LoginPreloadNode : AStateNode
{
    private GameStage stage;

    public override void OnCreate(StateMachine machine)
    {
        base.OnCreate(machine);
        stage = machine.Owner as GameStage;
    }

    protected override void Begin()
    {
        Preload();
        IsComplete = true;
    }

    private void Preload()
    {
        UIMgr.Instance.PreLoadUI(UIPanelName.WaitcircleView);
        UIMgr.Instance.PreLoadUI(UIPanelName.LoadingView);
    }

    public override void SysUpdate()
    {
        if (IsComplete)
            _machine.RunNextNode(this);
    }
}
