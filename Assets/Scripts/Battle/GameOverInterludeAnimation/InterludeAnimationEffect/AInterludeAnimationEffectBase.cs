using DG.Tweening;

public abstract class AInterludeAnimationEffectBase : APoolGameObjectBase
{
    private Sequence tween;
    
    /// <summary> 回收 </summary>
    public abstract void Recycle();
    
    public override void OnEnable()
    {
        base.OnEnable();
        tween = DOTween.Sequence();
        tween.AppendInterval(2);
        tween.OnComplete(Recycle);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        tween?.Kill(false);
        tween = null;
    }
}