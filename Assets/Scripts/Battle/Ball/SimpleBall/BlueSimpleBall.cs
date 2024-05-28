using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 蓝色简单球球
/// </summary>
public class BlueSimpleBall : ASimpleBall
{
    protected override int GetSelfLayer() => Layer.Blue_SmallBall;
    protected override int GetAttackLayer() => Layer.RedBall;
    public override void Recycle() => BallFactory.Recycle(this);
}
