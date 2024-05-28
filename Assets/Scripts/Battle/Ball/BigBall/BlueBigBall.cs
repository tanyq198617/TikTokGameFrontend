using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBigBall : ABigBall
{
    protected override int GetSelfLayer() => Layer.Blue_BigBall;
    protected override int GetAttackLayer() => Layer.RedBall;
    public override void Recycle() => BallFactory.Recycle(this);
}
