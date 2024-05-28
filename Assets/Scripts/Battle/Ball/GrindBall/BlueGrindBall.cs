using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueGrindBall : AGrindBall
{
    protected override int GetSelfLayer() => Layer.Blue_GrindBall;
    protected override int GetAttackLayer() => Layer.RedBall;
    public override void Recycle() => BallFactory.Recycle(this);
}
