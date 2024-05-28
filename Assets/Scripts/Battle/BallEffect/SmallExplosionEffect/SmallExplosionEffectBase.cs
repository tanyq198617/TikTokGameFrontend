using System;
using HotUpdateScripts;
using UnityEngine;

public abstract class SmallExplosionEffectBase : ABallEffectBase
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
    public override void OnInit(Vector3 _point, float effectTime)
    {
        base.OnInit(_point, effectTime);
        
        attackRadius =  TGlobalDataManager.Instance.smallExplosionStruct.attackRadius;
        damage = TGlobalDataManager.Instance.smallExplosionStruct.damage;
        resultDamage = TGlobalDataManager.Instance.smallExplosionStruct.resultDamage;
        this.point = _point;
        Array.Clear(results, 0, results.Length);
        Attack();
    }

    private void Attack()
    {
        EventMgr.Dispatch(BattleEvent.Battle_Camera_Shake, 4);
        AudioMgr.Instance.PlaySoundName(15);
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