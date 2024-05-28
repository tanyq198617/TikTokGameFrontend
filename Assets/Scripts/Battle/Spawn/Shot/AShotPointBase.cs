using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HotUpdateScripts;
using UnityEngine;

public struct BallInfo
{
    public long OnlyID;
    public TBallData Config;
    public PlayerInfo Player;
    public bool IsShowHead;

    public static long onlyID { get; private set; }
    public static BallInfo Create(TBallData data, PlayerInfo info, bool showHead = false)
    {
        return new BallInfo()
        {
            OnlyID = onlyID++,
            Config = data,
            Player = info,
            IsShowHead = showHead
        };
    }
}

/// <summary>
/// 发射点
/// </summary>
public abstract class AShotPointBase : APoolGameObjectBase
{
    /// <summary> 炮口配置表 </summary>
    protected TFireData MuzzleConfig;
    
    /// <summary> 发射口配置 </summary>
    protected TFirePoint ShotConfig;

    /// <summary> 炮口朝向 </summary>
    protected Transform MuzzleRoot;
    
    public int ID => ShotConfig?.Id ?? 0;
    public bool IsRed { get; private set; }

    /// <summary> 队列数量 </summary>
    protected readonly Queue<BallInfo> queue = new Queue<BallInfo>();

    protected TickedBase _tick;

    protected readonly List<Transform> firePoints = new List<Transform>();

    public override void setObj(GameObject gameObject)
    {
        base.setObj(gameObject);
        foreach (Transform trans in Trans)
            firePoints.Add(trans);
    }

    public void OnInit(bool isRed, TFireData fire, TFirePoint point, Transform root)
    {
        this.IsRed = isRed;
        this.MuzzleConfig = fire;
        this.ShotConfig = point;
        this.MuzzleRoot = root;
        initialize();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    protected void initialize()
    {
        Attach(MuzzleRoot);
        this.Trans.localPosition = new Vector3(0, 0, (float)ShotConfig.distance);
        _tick ??= TickedBase.Create((float)ShotConfig.fireInterval, Update, false, true);
        _tick.IsPause = false;
    }

    /// <summary>
    /// 设置间隔
    /// </summary>
    public void SetAccele(bool accele)
    {
        if(_tick == null)
            return;
        _tick.TickLength = accele ? (float)ShotConfig.acceleInterval : (float)ShotConfig.fireInterval;
    }

    /// <summary>
    /// 射击
    /// </summary>
    public void OnShot(BallInfo ball, long count = 1, int headCount = 0, int ratios = 0)
    {
        var bigOrSmallBallTypr = ball.Config.bigOrSmallBallTypr;
        BallInfo nb = default;
        if (headCount > 0) nb = BallInfo.Create(ball.Config, ball.Player, true);
        int round = 0;
        for (int i = 0; i < count; i++)
        {
            //如果显示头像，按周期添加
            if (headCount > 0)
            {
                //计算是否一个周期内
                if (i == 0 || i % ratios == 0)
                    round = headCount;

                //一个周期内添加个数
                if (round > 0)
                {
                    //要克隆一个
                    queue.Enqueue(nb);
                    round--;
                    continue;
                }
            }

            queue.Enqueue(ball);
        }
        EventMgr.Dispatch(BattleEvent.Battle_Ball_ValueChanged, IsRed ? 2 : 1, bigOrSmallBallTypr, queue.Count);
    }

    public void Update()
    {
        if (queue.Count > 0)
        {
            for (int i = 0; i < firePoints.Count; i++)
            {
                if (queue.Count <= 0) return;
                var info = queue.Dequeue();
                var ball = BallFactory.GetOrCreate(IsRed, info.Config, info.Player, info.IsShowHead);
                ball.OnBorn(firePoints[i].position, MuzzleRoot.forward);
                EventMgr.Dispatch(BattleEvent.Battle_Ball_ValueChanged, IsRed ? 2 : 1, ball.Config.bigOrSmallBallTypr, queue.Count);
            }
        }
    }

    public void Stop()
    {
        if (_tick != null)
        {
            _tick.IsPause = true;
        }
    }

    public void Clear()
    {
        this.MuzzleConfig = null;
        this.ShotConfig = null;
        this.MuzzleRoot = null;
        queue?.Clear();
        ClearTick();
    }
  
    private void ClearTick()
    {
        if (_tick != null)
            TickedMgr.Instance.Remove(_tick);
        _tick = null;
    }

    public abstract void Recycle();
}
