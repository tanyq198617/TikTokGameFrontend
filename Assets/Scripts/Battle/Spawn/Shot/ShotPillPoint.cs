using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 药丸球球生成点
/// </summary>
public class ShotPillPoint : AShotPointBase
{
    public override void Recycle() => GameObjectFactory.Recycle(this);
}
