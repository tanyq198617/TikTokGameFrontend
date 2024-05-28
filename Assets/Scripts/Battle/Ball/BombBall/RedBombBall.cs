using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBombBall : ABombBall
{
    protected override int GetSelfLayer() => Layer.Red_NoColliderBall;
    protected override int GetAttackLayer() => Layer.BlueBall;
    public override void Recycle() => BallFactory.Recycle(this);
    protected override void OnEndAudio()
    {
        base.OnEndAudio();
        AudioMgr.Instance.PlaySoundName(4);
    }
}
