using System.Security.Policy;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary> 小爆炸拖尾特效基类 /// </summary>
public abstract class SmallExplosionTrailBase : ABallEffectBase
{
    private Sequence tween;

    /// <summary> 特效的初始坐标 /// </summary>
    protected abstract Vector3 initPoint { get; }

    protected ParabolicMovement movement;

    public override void OnEnable()
    {
        base.OnEnable();
        Trans.position = initPoint;
    }

    public override void OnInit(Vector3 targetPoint, float time)
    {
        movement ??= Trans.GetOrAddComponent<ParabolicMovement>();
        movement.enabled = true;
        movement.Init(initPoint, targetPoint, time, 5);
        
        tween = DOTween.Sequence();
        tween.AppendInterval(time);
        tween.OnComplete(Recycle);
        tween.Play();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        tween?.Kill(false);
        tween = null;
        if (movement != null)
            movement.enabled = false;
    }
}