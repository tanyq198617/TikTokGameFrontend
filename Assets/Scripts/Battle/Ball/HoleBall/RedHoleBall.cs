using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHoleBall : AHoleBall
{
    protected override int GetSelfLayer() => Layer.Red_NoColliderBall;
    protected override int GetAttackLayer() => Layer.BlueBall;
    public override void Recycle() => BallFactory.Recycle(this);
}
