using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 普通大球生成点
/// </summary>
public class ShotBigPoint : AShotPointBase
{
    public override void Recycle() => GameObjectFactory.Recycle(this);
}
