using System;
using System.Collections.Generic;
using DG.Tweening;
using HotUpdateScripts;
using UnityEngine;

/// <summary> 雷霆一击基类 /// </summary>
public abstract class ThunderboltBase
{
    private TickedBase _tick;
    private readonly Collider[] results = new Collider[100];
    private readonly List<Collider> tempList = new List<Collider>();

    /// <summary> 延迟播放特效时间 /// </summary>
    private float delayTime;

    /// <summary> 探测半径 /// </summary>
    private float detectionRadius;

    private float timer;

    protected int attackLayerMask;

    /// <summary> 对面boss坐标 /// </summary>
    protected Vector3 bossPoint;

    /// <summary> 攻击目标坐标 /// </summary>
    protected Vector3 targetPoint;

    protected bool isRed;
    public static int seed = 0;

    /// <summary> 小爆炸的特效 /// </summary>
    protected ThunderboltEffectBase effect;


    /// <summary> 特效播完的回调 /// </summary>
    public Action<ThunderboltBase> backAction;

    /// <summary> 小爆炸玩家数据 /// </summary>
    private PlayerInfo info;

    private bool isRunning = false;


    /// <summary> 加载小爆炸爆炸特效 /// </summary>
    protected virtual void GetExplosionClass()
    {
    }

    private Sequence tween;

    public virtual void Init(CampType camp)
    {
        int totalTime = TGlobalDataManager.Instance.smallExplosionStruct.totalTime;
        delayTime = MathUtility.GetRandomNum(totalTime * 100, Guid.NewGuid().GetHashCode() + ++seed) / (float)100;
        
        detectionRadius = TGlobalDataManager.Instance.smallExplosionStruct.detectionRadius;

        tween = DOTween.Sequence();
        tween.AppendInterval(delayTime);
        tween.OnComplete(RedAttack);
        tween.Play();
        /*_tick ??= TickedBase.Create(0, UpData, false, true);
        _tick.Reset();*/
    }

    public void AddPlayerInfo(PlayerInfo _info, Action<ThunderboltBase> _backAction)
    {
        tween?.Kill(false);
        this.backAction = _backAction;
        this.info = _info;
        this.isRunning = true;
        Init(_info.campType);
    }

    /*private void UpData()
    {
        if (MathUtility.IsOutTime(ref timer, delayTime))
        {
            timer = 0;
            RedAttack();
            _tick.IsPause = true;
        }
    }*/

    private void RedAttack()
    {
        Array.Clear(results, 0, results.Length);
        var size = Physics.OverlapSphereNonAlloc(Vector3.zero, detectionRadius, results, attackLayerMask);

        bool isChecked = results[0] != null;

        if (isChecked)
        {
            tempList.Clear();

            //存所有检测到的数据
            for (int i = 0; i < results.Length; i++)
            {
                if (results[i] == null)
                    break;
                tempList.Add(results[i]);
            }


            if (tempList.Count <= 0)
            {
                PlayAttackEffect(bossPoint);
            }
            else
            {
                int rand = MathUtility.GetRandomNum(tempList.Count, Guid.NewGuid().GetHashCode() + ++seed);
                var ball = tempList[rand];
                PlayAttackEffect(ball.transform.position);
                tempList.Remove(ball);
            }
        }
        else
        {
            PlayAttackEffect(bossPoint);
        }
    }

    /// <summary> 开始小爆炸流程 /// </summary>
    private void PlayAttackEffect(Vector3 _point)
    {
        targetPoint = _point;
        GetExplosionClass();
        CameraMgr.Instance.Shake(5);
        effect.OnInit(targetPoint, 2);
        effect.backAction = EffectBackAct;
    }

    /// <summary> 清除计时 /// </summary>
    private void ClearTick()
    {
        if (_tick != null)
            TickedMgr.Instance.Remove(_tick);
        _tick = null;
    }

    /// <summary> 特效播完的回调 /// </summary>
    protected virtual void EffectBackAct()
    {
        backAction?.Invoke(this);
        backAction = null;
        effect = null;
        ClearTick();
        RecycleClass();
    }

    /// <summary> 回收 /// </summary>
    public void Recycle()
    {
        if (!isRunning) return;

        effect?.Recycle();
        effect = null;
        ClearTick();
        backAction = null;
        RecycleClass();
        tween?.Kill(false);
        tween = null;
    }

    protected virtual void RecycleClass()
    {
        this.isRunning = false;
    }
}