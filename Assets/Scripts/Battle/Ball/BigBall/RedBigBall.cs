using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBigBall : ABigBall
{
    protected override int GetSelfLayer() => Layer.Red_BigBall;
    protected override int GetAttackLayer() => Layer.BlueBall;
    public override void Recycle() => BallFactory.Recycle(this);
}
