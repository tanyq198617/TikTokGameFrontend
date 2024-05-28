using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 爆炸球球
/// </summary>
public abstract class ABombBall : ABallBase
{
    private readonly Collider[] results = new Collider[500];
    private bool isResulting;
    protected ABallEffectBase _effect;
    protected virtual void OnEndAudio() { }
    protected override void OnBegin()
    {
        isResulting = false;
        Array.Clear(results, 0, results.Length);
    }
    
    protected override void OnEnd()
    {
        _effect = null;
        isResulting = false;
    }

    public override void OnCollision(Collision other)
    {
        if(isResulting)
            return;
        isResulting = true;
        OnResultEffect();
    }

    public override bool OnBossCollision(int attack)
    {
        if(isResulting)
            return false;
        isResulting = true;
        OnResultEffect();
        return false;
    }
    
    public override void OnResult(int damage) { }

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

    protected virtual void CameraShake()
    {
        EventMgr.Dispatch(BattleEvent.Battle_Camera_Shake,1);
    }

    /// <summary>
    /// 爆炸球表现
    /// </summary>
    protected override void OnResultEffect()
    {
        gravity.Stop();
        gravity.SetTriggle(true);
        var size = Physics.OverlapSphereNonAlloc(Trans.position, (float)Config.effectRange, results, GetAttackLayer());
        for (int i = 0; i < results.Length; i++)
        {
            var collider = results[i];
            if(collider == null) break;
            var ball = BallFactory.Find(collider.gameObject);
            if(ball == null) continue;
            if(ball.Config.type == Config.type)
                continue;
            
            //爆炸球不伤害黑洞球
            if(ball.Config.type == 3)
                continue;
            
            ball.OnResult(Attack);
        }
        var effect = BallEffectFactory.GetOrCreate(Camp == CampType.红.ToInt(), 3, Trans.position, 1f);
        effect.SetLocalScale(Vector3.one * (float)Config.effectScale);
        CameraShake();
        OnDeathNow();
        OnEndAudio();
    }
}