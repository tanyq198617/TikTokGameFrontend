using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 蓝色蛋仔球
/// </summary>
public class BlueDanzaiBall : BlueBigBall
{
    public override void Recycle() => BallFactory.Recycle(this);
}
