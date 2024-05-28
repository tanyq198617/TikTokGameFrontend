using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 小球球生成点
/// </summary>
public class ShotSmallPoint : AShotPointBase
{
    public override void Recycle() => GameObjectFactory.Recycle(this);
}
