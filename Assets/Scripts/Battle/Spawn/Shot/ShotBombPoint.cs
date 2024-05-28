using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 爆炸球球发射点
/// </summary>
public class ShotBombPoint : AShotPointBase
{
    public override void Recycle() => GameObjectFactory.Recycle(this);
}
