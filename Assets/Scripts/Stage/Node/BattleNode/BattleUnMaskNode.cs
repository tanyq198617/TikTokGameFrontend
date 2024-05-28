using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnMaskNode : UnMaskNodeBase
{
    protected override void RunNextNode() => _machine.RunNextNode(this);
}
