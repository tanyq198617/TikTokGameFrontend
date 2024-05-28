using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红色蛋仔球
/// </summary>
public class RedDanzaiBall : RedBigBall
{
    public override void Recycle() => BallFactory.Recycle(this);
}
