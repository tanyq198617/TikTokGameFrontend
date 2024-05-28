using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 常规战斗玩法的控制器
/// </summary>
public class GeneralBattleControl : ABattleControl
{
    public override void OnEnter(int regionId, bool isForced = false, Action callback = null, object args = null)
    {
        LevelGlobal.Instance.Init<GeneralLevelDataBase>(regionId, args);
        GameStageMachine.ChangeState<BattleStage>();
    }

    public override void OnExit(int args, bool isSysJump)
    {
    }

    public override void Clear()
    {
    }
}
