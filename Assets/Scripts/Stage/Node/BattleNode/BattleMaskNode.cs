using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗遮罩
/// </summary>
public class BattleMaskNode : MaskNodeBase
{
    protected override void RunNextNode() => _machine.RunNextNode(this);
}
