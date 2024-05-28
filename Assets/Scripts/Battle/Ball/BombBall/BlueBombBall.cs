using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBombBall : ABombBall
{
    protected override int GetSelfLayer() => Layer.Blue_NoColliderBall;
    protected override int GetAttackLayer() => Layer.RedBall;
    public override void Recycle() => BallFactory.Recycle(this);
    protected override void OnEndAudio()
    {
        base.OnEndAudio();
        AudioMgr.Instance.PlaySoundName(4);
    }
}
