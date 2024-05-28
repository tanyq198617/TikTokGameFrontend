using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPillBall : APillBall
{
    protected override int GetSelfLayer() => Layer.Red_PillBall;
    protected override int GetAttackLayer() => Layer.BlueBall;
    public override void Recycle() => BallFactory.Recycle(this);
}
