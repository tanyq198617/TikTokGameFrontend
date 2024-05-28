using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePillBall : APillBall
{
    protected override int GetSelfLayer() => Layer.Blue_PillBall;
    protected override int GetAttackLayer() => Layer.RedBall;
    public override void Recycle() => BallFactory.Recycle(this);
}
