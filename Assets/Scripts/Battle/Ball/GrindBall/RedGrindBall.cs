using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGrindBall : AGrindBall
{
    protected override int GetSelfLayer() => Layer.Red_GrindBall;
    protected override int GetAttackLayer() => Layer.BlueBall;
    public override void Recycle() => BallFactory.Recycle(this);
}
