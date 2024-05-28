using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Loading遮罩
/// </summary>
public class CityMaskNode : MaskNodeBase
{
    protected override void Begin() => base.Call();
    protected override void RunNextNode() => _machine.RunNextNode(this);
}
