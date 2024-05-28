using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 蓝色小爆炸球
/// </summary>
public class BlueSmallBombBall : BlueBombBall
{
    public override void Recycle() => BallFactory.Recycle(this);

    protected override void CameraShake() { }

    protected override void OnEndAudio()
    {
        AudioMgr.Instance.PlaySoundName(16);
    }
}
