using System;
using DG.Tweening;
using HotUpdateScripts;
using UnityEngine;

public abstract class ThunderboltEffectBase : ABallEffectBase
{
      /// <summary> 攻击半径 /// </summary>
    private float attackRadius;

    private float timer;
    private int damage;
    private int resultDamage;

    protected abstract int ballLayerMask { get; }
    
    protected abstract CampType campType { get; }
    
    private readonly Collider[] results = new Collider[500];

    private Vector3 point;
    public override void OnInit(Vector3 _point, float _effectTime)
    {
        Trans.position = _point;
        attackRadius =  TGlobalDataManager.Instance.thunderboltStruct.attackRadius;
        damage = TGlobalDataManager.Instance.thunderboltStruct.damage;
        resultDamage = TGlobalDataManager.Instance.thunderboltStruct.resultDamage;
        this.point = _point;
        Array.Clear(results, 0, results.Length);
       // PlayTween(effectTime);
        PlayTween(3);
    }

    /// <summary> 雷霆一击的生命时间控制 /// </summary>
    private void PlayTween(float _effectTime)
    {
        tween?.Kill(false);
        tween = DOTween.Sequence();
        //float _effectTime = effectTime == 0 ? particle.GetDelay() : effectTime;
        tween.AppendInterval(0.4f);
        tween.AppendCallback(Attack);
        tween.AppendInterval(_effectTime - 0.4f);
        tween.OnComplete(Recycle);
        tween.Play();
    }
    
    private void Attack()
    {
        EventMgr.Dispatch(BattleEvent.Battle_Camera_Shake, 5);
        
        AudioMgr.Instance.PlaySoundName(12);
        
        var ballSize = Physics.OverlapSphereNonAlloc(point, attackRadius, results, ballLayerMask);
        for (int i = 0; i < results.Length; i++)
        {
            var collider = results[i];
            if (collider == null) break;
            if (Layer.IsReslut(collider.gameObject.layer))
                AttackResultPoint();
            else
            {
                var ball = BallFactory.Find(collider.gameObject);
                if (ball == null) continue;

                ball.OnResult(damage);
            }
        }
        Array.Clear(results, 0, ballSize);
    }

    /// <summary> 攻击结算点 /// </summary>
    private void AttackResultPoint()
    {
        if (campType == CampType.蓝)
        {
            RedResultPoint  result = RedCampController.Instance.GetReslut<RedResultPoint>();
            result.SmallExplosionAttack(resultDamage);
        }
        else
        {
            BlueResultPoint  result = BlueCampController.Instance.GetReslut<BlueResultPoint>();
            result.SmallExplosionAttack(resultDamage);
        }
    }
}