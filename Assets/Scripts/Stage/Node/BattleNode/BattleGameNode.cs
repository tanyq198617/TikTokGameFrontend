using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗游戏执行逻辑
/// </summary>
public class BattleGameNode : AStateNode
{
    protected override void Begin()
    {
    }

    protected override void End()
    {
    }
    
    public override void SysUpdate()
    {
        if (IsComplete)
            _machine.RunNextNode(this);
    }
}
