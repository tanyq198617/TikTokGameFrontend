using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红色小爆炸球
/// </summary>
public class RedSmallBombBall : RedBombBall
{
    public override void Recycle() => BallFactory.Recycle(this);
    protected override void CameraShake() { }
    protected override void OnEndAudio()
    {
        AudioMgr.Instance.PlaySoundName(16);
    }
}
