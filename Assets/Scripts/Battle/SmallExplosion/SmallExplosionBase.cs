using System;
using System.Collections.Generic;
using DG.Tweening;
using HotUpdateScripts;
using UnityEngine;

public abstract class SmallExplosionBase
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

    /// <summary> 自己boss坐标 /// </summary>
    protected Vector3 myBossPoint;

    /// <summary> 攻击目标坐标 /// </summary>
    protected Vector3 targetPoint;

    /// <summary> 检测射线的起始坐标 /// </summary>
    protected Vector3 checkRaystartPoint;

    protected bool isRed;
    public static int seed = 0;

    /// <summary> 小爆炸的特效 /// </summary>
    protected SmallExplosionEffectBase effect;

    /// <summary> 小爆炸拖尾的特效 /// </summary>
    protected SmallExplosionTrailBase traileffect;


    /// <summary> 特效播完的回调 /// </summary>
    public Action<SmallExplosionBase> backAction;

    /// <summary> 小爆炸玩家数据 /// </summary>
    private PlayerInfo info;

    private bool isRunning = false;

    //范围捏检测到的所有预制体的集合
    private List<Transform> rayAll = new List<Transform>();

    //单条射线检测到的目标
    private RaycastHit[] hits = new RaycastHit[20];

    //检测攻击目标z轴结束点
    private float attackCheckEndZ;

    /// <summary> 添加拖尾特效 /// </summary>
    protected virtual void GetExplosionTrailer()
    {
    }

    /// <summary> 加载小爆炸爆炸特效 /// </summary>
    protected virtual void GetExplosionClass()
    {
    }

    private Sequence tween;

    public virtual void Init(CampType camp)
    {
        int totalTime = TGlobalDataManager.Instance.smallExplosionStruct.totalTime;
        if (totalTime <= 0)
            delayTime = 0;
        else
            delayTime = MathUtility.GetRandomNum(totalTime * 100, Guid.NewGuid().GetHashCode() + ++seed) / (float)100;

        detectionRadius = TGlobalDataManager.Instance.smallExplosionStruct.detectionRadius;

        rayAll.Clear();
        
        tween?.Kill(false);
        tween = DOTween.Sequence();
   
        tween.AppendInterval(delayTime);
        tween.AppendCallback(CheckZuiJInTarget);
        tween.Play();

    }

    public void AddPlayerInfo(PlayerInfo _info, Action<SmallExplosionBase> _backAction)
    {
        this.backAction = _backAction;
        this.info = _info;
        this.isRunning = true;
        Init(_info.campType);
        attackCheckEndZ = 0;
    }

    /// <summary> 检测最近目标,检测到最近目标开始范围随机检测 /// </summary>
    private void CheckZuiJInTarget()
    {
        //检测最近的目标
        RaycastHit hit;

        for (int i = 0; i <= 25; i++)
        {
            if (isRed)
                checkRaystartPoint.z += 1;
            else
                checkRaystartPoint.z -= 1;

            Ray ray = new Ray(checkRaystartPoint, Vector3.left);
#if UNITY_EDITOR
            Debug.DrawRay(ray.origin, ray.direction * 20, Color.red);
#endif

            if (Physics.Raycast(ray, out hit, 20, attackLayerMask))
            {
                checkRaystartPoint.z = hit.transform.position.z;
                CheckAttackTarget(checkRaystartPoint);
                return;
            }
        }

        //未检测到最近目标攻击对面炮台
        PlayAttackEffect(bossPoint);
    }

    /// <summary> 随机攻击目标攻击目标/// </summary>
    private void CheckAttackTarget(Vector3 rayStartPoint)
    {
        RaycastNonAlloc(rayStartPoint);
        
        for (int i = 1; i <= detectionRadius; i++)
        {
            if (isRed)
                rayStartPoint.z += 1;
            else
                rayStartPoint.z -= 1;
            RaycastNonAlloc(rayStartPoint);
        }

        if (rayAll.Count > 0)
        {
            int rand = MathUtility.GetRandomNum(rayAll.Count, Guid.NewGuid().GetHashCode() + ++seed);
            var ball = rayAll[rand];
            PlayAttackEffect(ball.transform.position);
        }
        else
        {
            PlayAttackEffect(bossPoint);
        }
    }

    /// <summary> 射线检测所有目标 /// </summary>
    private void RaycastNonAlloc(Vector3 rayStartPoint)
    {
        Ray ray = new Ray(rayStartPoint, Vector3.left);
#if UNITY_EDITOR
        Debug.DrawRay(ray.origin, ray.direction * 20, Color.yellow);
#endif
        Array.Clear(hits, 0, hits.Length);

        var size = Physics.RaycastNonAlloc(ray, hits, 20, attackLayerMask);

        for (int j = 0; j < hits.Length; j++)
        {
            if (hits[j].transform != null)
                rayAll.Add(hits[j].transform);
        }
    }

    /// <summary> 开始小爆炸流程 /// </summary>
    private void PlayAttackEffect(Vector3 _point)
    {
        targetPoint = _point;
        PlayExplosionTrail();
        tween?.Kill(false);
        tween = null;
        tween = DOTween.Sequence();

        tween.AppendInterval(TGlobalDataManager.smallExplosionTraliTime - 0.2f);
        tween.OnComplete(PlayExplosionEffect);
    }

    /// <summary> 播放拖尾 /// </summary>
    private void PlayExplosionTrail()
    {
        GetExplosionTrailer();
        traileffect.OnInit(targetPoint, TGlobalDataManager.smallExplosionTraliTime);
    }

    /// <summary> 播放爆炸特效,对敌掉血 /// </summary>
    private void PlayExplosionEffect()
    {
        traileffect?.Recycle();
        traileffect = null;
        GetExplosionClass();
        effect.OnInit(targetPoint, 1);
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
        if (traileffect != null)
        {
            traileffect.Recycle();
            traileffect = null;
        }

        effect?.Recycle();
        effect = null;
        ClearTick();
        backAction = null;
        RecycleClass();
    }

    protected virtual void RecycleClass()
    {
        this.isRunning = false;
        tween?.Kill(false);
        tween = null;
    }
}