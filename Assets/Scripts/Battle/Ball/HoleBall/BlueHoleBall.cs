using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueHoleBall : AHoleBall
{
    protected override int GetSelfLayer() => Layer.Blue_NoColliderBall;
    protected override int GetAttackLayer() => Layer.RedBall;
    public override void Recycle() => BallFactory.Recycle(this);
}
