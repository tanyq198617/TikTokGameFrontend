using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 碾压球球生成点
/// </summary>
public class ShotGrindPoint : AShotPointBase
{
    public override void Recycle() => GameObjectFactory.Recycle(this);
}
