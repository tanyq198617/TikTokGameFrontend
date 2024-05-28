using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HotUpdateScripts;
using UnityEngine;

/// <summary>
/// 黑洞球球
/// </summary>
public abstract class AHoleBall : ABallBase
{
    private readonly Collider[] results = new Collider[500];
    private TickedBase _tick;
    private ABallEffectBase effect;
    private float timer;
    private float interval;
    private float life;
    private bool isResulting;
    private float moveSpeed;
    
    protected override void OnBegin()
    {
        isResulting = false;

        moveSpeed = TGlobalDataManager.Instance.GetByKey<float>(TGlobal.HoleBallEffectSpeed); 
        Array.Clear(results, 0, results.Length);
        
    }
    
    protected override void OnEnd()
    {
        ClearTick();
        isResulting = false;
        effect?.Recycle();
        effect = null;
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

    /// <summary>
    /// 爆炸球表现
    /// </summary>
    protected override void OnResultEffect()
    {
        timer = 0;
        interval = (float)Config.resultInterval;
        life = TimeMgr.RealTime;
        // gravity.Stop();
        gravity.SetColliderActive(false);
        gravity.ChangeVelocity(moveSpeed);
        gravity.SetTriggle(true);
        _tick ??= TickedBase.Create(0, OnHoleUpdate, false, true);
        effect ??= BallEffectFactory.GetOrCreate(Camp == CampType.红.ToInt(), 4, Trans.position, (float)Config.effectTime);
        UIUtility.Attach(Trans, effect.GetGameObject());
        effect.SetLocalScale(Vector3.one * (float)Config.effectScale);
        EventMgr.Dispatch(BattleEvent.Battle_Camera_Shake, 3);
    }

    private void OnHoleUpdate()
    {
        var size = Physics.OverlapSphereNonAlloc(Trans.position, (float)Config.effectRange, results, GetAttackLayer());
        for (int i = 0; i < results.Length; i++)
        {
            var collider = results[i];
            if(collider == null) break;
            var ball = BallFactory.Find(collider.gameObject);
            if(ball == null) continue;
            if(ball.Config.type == Config.type)
                continue;
            
            //黑洞球不吸爆炸球
            if(ball.Config.type == 1)
                continue;
            
            var ballTrans = ball.GetTransform();
            var forward = (ballTrans.position - Trans.position).normalized;
            ball.AddForce(forward);
        }

        //间隔时间产生一次结算
        if (MathUtility.IsOutTime(ref timer, interval))
        {
            timer = 0;
            for (int i = 0; i < results.Length; i++)
            {
                var collider = results[i];
                if(collider == null) break;
                var ball = BallFactory.Find(collider.gameObject);
                if(ball == null) continue;
                if(ball.Config.type == Config.type)
                    continue;
                
                //黑洞球不对爆炸球
                if(ball.Config.type == 1)
                    continue;
                
                ball.OnResult(Attack);
            }
        }

        if (TimeMgr.RealTime - life >= Config.effectTime)
        {
            ClearTick();
            OnDeathNow();
        }
    }

    private void ClearTick()
    {
        if (_tick != null)
        {
            TickedMgr.Instance.Remove(_tick);
            _tick = null;
        }
    }
}