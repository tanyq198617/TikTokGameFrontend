using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红色简单球球
/// </summary>
public class RedSimpleBall : ASimpleBall
{
    protected override int GetSelfLayer() => Layer.Red_SmallBall;
    protected override int GetAttackLayer() => Layer.BlueBall;
    public override void Recycle() => BallFactory.Recycle(this);
}
