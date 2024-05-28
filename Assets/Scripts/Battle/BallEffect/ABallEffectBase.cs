using System;
using DG.Tweening;
using UnityEngine;

public abstract class ABallEffectBase : APoolGameObjectBase
{
    /// <summary> 特效播放时间 </summary>
    public abstract float effectTime { get; }

    /// <summary> 回收 </summary>
    public abstract void Recycle();

    public Sequence tween;

    /// <summary>
    /// 挂载在特效上获取特效时长
    /// </summary>
    protected ParticlePlayable particle;

    /// <summary> 特效播完的回调 /// </summary>
    public Action backAction;

    public override void setObj(GameObject gameObject)
    {
        base.setObj(gameObject);
        particle ??= m_gameobj.GetOrAddComponent<ParticlePlayable>();
    }

    /// <summary> 加载特效设置回收特效 /// </summary>
    public virtual void OnInit(Vector3 point, float effectTime)
    {
        Trans.position = point;
        tween?.Kill(false);
        tween = DOTween.Sequence();
        float _effectTime = effectTime == 0 ? particle.GetDelay() : effectTime;
        tween.AppendInterval(_effectTime);
        tween.OnComplete(Recycle);
    }

    /// <summary>
    /// 设置特效大小
    /// </summary>
    /// <param name="Scale"></param>
    public virtual void SetLocalScale(Vector3 Scale)
    {
        Trans.localScale = Scale;
    }
    
    public override void OnDisable()
    {
        tween?.Kill(false);
        tween = null;
        
        Trans.localScale = Vector3.one;
       
        if (backAction.IsNotNull())
        {
            backAction();
            backAction = null;
        }
        
        base.OnDisable();
    }
}