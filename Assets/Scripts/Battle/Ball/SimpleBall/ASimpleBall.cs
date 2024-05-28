using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 简单球球
/// </summary>
public abstract class ASimpleBall : ABallBase
{
    protected Tween _tween;

    protected override void OnBegin()
    {
    }

    protected override void OnEnd()
    {
        ClearTween();
    }

    private void OnCollisionStay(Collision obj)
    {
        CheckState(obj);
    }

    private void OnCollisionEnter(Collision obj)
    {
        CheckState(obj);
    }

    protected void CheckState(Collision obj)
    {
        if ((GetAttackLayer() & 1 << obj.gameObject.layer) != 0)
        {
            ClearTween();
            gravity.SetColliderActive(false);
            _tween = Trans.DOScale(0, 0.5f);
            _tween.OnComplete(Recycle);
        }
    }

    private void ClearTween()
    {
        if (_tween != null)
        {
            _tween.Kill(false);
            _tween = null;
        }
    }
}
