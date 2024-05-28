using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 药丸球球,羊羊球
/// </summary>
public abstract class APillBall : ABallBase
{
    protected ABallEffectBase _effect;

    protected override void OnPlayBornSound()
    {
        BallSoundMgr.Instance.PlayPillBallBegin(Config.beginSoundIndex);
    }

    protected override void OnEnd()
    {
        _effect = null;
    }

    protected override void OnUpradeEffect()
    {
        if(_effect == null)
            return;
        _effect = BallEffectFactory.GetOrCreate(Camp == CampType.红.ToInt(), 5, Trans.position, 1f);
        _effect.backAction = () =>
        {
            _effect = null;
        };
    }
}