using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑洞球球生成点
/// </summary>
public class ShotHolePoint : AShotPointBase
{
    public override void Recycle() => GameObjectFactory.Recycle(this);
}
