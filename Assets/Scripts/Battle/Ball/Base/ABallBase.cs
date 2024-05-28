using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HotUpdateScripts;
using UnityEngine;

/// <summary>
/// 所有球体的基类
/// </summary>
public abstract class ABallBase : APoolGameObjectBase,IEvent
{
    protected GravityController gravity;
    public GravityController Controller => gravity;
    protected SkinComponent skin;
    protected HeadComponent head;

    /// <summary> 球球的配置 </summary>
    public TBallData Config;

    /// <summary> 所属角色 </summary>
    public PlayerInfo Owner;

    /// <summary> 显示角色头像 </summary>
    public bool IsShowHead;
    
    /// <summary> 阵营，1=红，2=蓝 </summary>
    public int Camp;
    
    /// <summary> 自由落体速度 </summary>
    protected Vector3 velocity;
    
    /// <summary> 出膛速度 </summary>
    protected float fireSpeed;
    
    /// <summary> 生命值 </summary>
    protected int HP;
    
    /// <summary> BUFF生命值 </summary>
    protected int BuffHP;
    
    /// <summary> 最大生命值 </summary>
    protected int MaxHP;

    /// <summary> 攻击力 </summary>
    public int Attack => (Config?.attack ?? 0) + BuffAttack;
    
    /// <summary> BUFF攻击力 </summary>
    protected int BuffAttack;
    
    protected Tween _tween;
    protected Sequence _sequeue;
    protected float timer;
    protected float interval;
    protected bool deathing;

    /// <summary> 获得自己的Layer </summary>
    protected abstract int GetSelfLayer();
    
    /// <summary> 获得攻击的Layer </summary>
    protected abstract int GetAttackLayer();

    /// <summary> 回收 </summary>
    public abstract void Recycle();
    
    protected virtual void Init() { }
    protected virtual void Release() { }
    protected virtual void OnBegin() { }
    protected virtual void OnEnd() { }
    
    /// <summary>
    /// 触发碰撞了
    /// </summary>
    public virtual void OnCollision(Collision other)
    {
        if(deathing) return;
        if (MathUtility.IsOutTime(ref timer, interval))
        {
            timer = 0;
            var target = other.gameObject;
            var ball = BallFactory.Find(target);
            if(ball == null) return;
            var attack = ball.Attack;
            OnResult(attack);
        }
    }

    public virtual bool OnBossCollision(int attack)
    {
        if(deathing) return false;
        if (MathUtility.IsOutTime(ref timer, interval))
        {
            timer = 0;
            OnResult(attack);
            return true;
        }
        return false;
    }

    public virtual void OnResult(int damage)
    {
        if(deathing) return;
        this.HP -= damage;
        OnHitEffect();
        if (HP <= 0)
            OnDeath();
        else
            OnResultEffect();
    }

    /// <summary>
    /// 给球一个瞬时力
    /// </summary>
    public void AddForce(Vector3 forward)
    {
        // 通过AddForce方法添加力
        gravity.AddForce(forward * Config.effectParam.Float());
    }

    /// <summary>
    /// 死亡
    /// </summary>
    protected virtual void OnDeath()
    {
        if(deathing) return;
        deathing = true;
        ClearTween();
        gravity.SetColliderActive(false);
        _sequeue = DOTween.Sequence();
        _sequeue.AppendInterval(0.1f);
        _sequeue.AppendCallback(Recycle);
        _sequeue.Play();
    }

    /// <summary>
    /// 立即死亡
    /// </summary>
    protected void OnDeathNow()
    {
        if(deathing) return;
        deathing = true;
        ClearTween();
        gravity.SetColliderActive(false);
        Recycle();
    }

    /// <summary>
    /// 结算表现
    /// </summary>
    protected virtual void OnResultEffect()
    {
        var hp = Mathf.Min(HP, MaxHP);
        float rate = hp * 1.0f / MaxHP;
        float minsize = Config.size.Float() * rate;
        if (minsize < Config.minsize.Float()) minsize = Config.minsize.Float();
        var localScale = Vector3.one * minsize;
        var size = Trans.localScale;
        if (Math.Abs(localScale.x - size.x) < 0.01f)
            return;
        ClearTween();
        _tween = DOTween.To(() => Trans.localScale, x => Trans.localScale = x, localScale, 0.1f);
    }

    /// <summary>
    /// 受击表现
    /// </summary>
    protected virtual void OnHitEffect()
    {
        this.skin.FlashOutLine();
    }

    /// <summary>
    /// 升级表现
    /// </summary>
    protected virtual void OnUpradeEffect()
    {
        
    }

    /// <summary> 球出生播放音效 /// </summary>
    protected virtual void OnPlayBornSound()
    {
        if (Config.beginSoundIndex > 0)
            AudioMgr.Instance.PlaySoundName(Config.beginSoundIndex);
    }
    
    /// <summary>
    /// 游戏结束清空场上的球,缩小时间1秒
    /// </summary>
    public void GameOverDieTween()
    {
        if (deathing) return;
        deathing = true;
        ClearTween();
        _tween = Trans.DOScale(Vector3.zero, 1).OnComplete(() => { 
            gravity.SetColliderActive(false);
            Recycle();
        });
    }
    
    #region 生命周期

    /// <summary>
    /// 绑定
    /// </summary>
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        this.Trans.position = Vector3.zero;
        this.Trans.localEulerAngles = Vector3.zero;
        this.gravity = Trans.GetComponent<GravityController>();
        this.skin = UIUtility.GetComponent<SkinComponent>(Trans, "Skin");
        this.head = UIUtility.CreatePageNoClone<HeadComponent>(Trans, "Head");
        this.Init();
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public override void OnDestroy()
    {
        Release();
        gravity = null;
        head = null;
        base.OnDestroy();
    }

    /// <summary>
    /// 取出显示
    /// </summary>
    public override void OnEnable()
    {
        base.OnEnable();
        if (head != null)
            head.IsActive = true;
        gravity.AddCollisionEnter(OnCollision);
        AddEventListener();
        OnBegin();
    }

    /// <summary>
    /// 回收隐藏
    /// </summary>
    public override void OnDisable()
    {
        RemoveEventListener();
        if (head != null)
            head.IsActive = false;
        gravity.RemoveCollisionEnter(OnCollision);
        skin.Clear();
        skin.CloseOutLine();
        skin.ResetDefaultSkin();
        BuffHP = 0;
        BuffAttack = 0;
        Config = null;
        gravity.Clear();
        Trans.position = Vector3.one * 1000;
        OnEnd();
        ClearTween();
        _sequeue?.Kill(false);
        _sequeue = null;
        deathing = false;
        IsShowHead = false;
        base.OnDisable();
    }

    #endregion
    
    public void OnInit(bool isRed, TBallData ballData, PlayerInfo player, bool isShowHead)
    {
        this.Config = ballData;
        this.Owner = player;
        this.IsShowHead = isShowHead;
        this.Camp = isRed ? CampType.红.ToInt() : CampType.蓝.ToInt(); //1=红,2=蓝
        this.interval = Config.resultInterval.Float();
        this.timer = TimeMgr.RealTime + interval;
        this.gravity.SetLayer(GetSelfLayer(), GetAttackLayer());
        this.gravity.Schedule(isRed, Config.noColliderTime.Float(), Config.noColliderRange.Float(),Config.size.Float(), Config.mass.Float());
        this.gravity.SetMaxSpeed(Config.maxSpeed.Float());
        this.velocity = new Vector3(0, 0, isRed ? ballData.moveSpeed.Float() : -ballData.moveSpeed.Float());
        this.fireSpeed = ballData.bornSpeed.Float();
        this.HP = this.MaxHP = ballData.hp;
        this.RenderHead(isShowHead, player);

        var buff = BuffMgr.GetBuff(isRed ? CampType.红 : CampType.蓝, 1001);
        if (buff != null)
        {
            SetAttrByBuff(buff);
            if (buff.GetSkin() > 0)
                this.skin.SetAdvanceSkin();
        }
        this.skin.CloseOutLine();
    }

    private void RenderHead(bool isShowHead, PlayerInfo player)
    {
        if (head == null)
            return;
        this.head.IsActive = isShowHead && player.TexHead != null;
        if (isShowHead)
            head.RenderHead(player.TexHead);
    }

    public void OnBorn(Vector3 position, Vector3 forward)
    {
        this.Trans.forward = forward;
        this.gravity.OnBorn(position, velocity, forward, fireSpeed);
        OnPlayBornSound();
    }
    
    public void OnOverflowBorn(Vector3 position, Vector3 forward)
    {
        this.Trans.forward = forward;
        this.gravity.OnOverflowBorn(position, velocity, forward);
    }

    public void Stop()
    {
        if (IsActive)
            this.gravity?.Stop();
    }
    
    
    private void ClearTween()
    {
        if (_tween != null)
        {
            _tween.Kill(false);
            _tween = null;
        }
    }

    public void AddEventListener()
    {
    }

    public void RemoveEventListener()
    {
    }

    public void OnBuffChanged(BuffHandler handler)
    {
        if(Config == null) return;
        var count = handler.Count();
        if (count <= 0)
        {
            BuffAttack = 0;
            BuffHP = 0;
            skin.ResetDefaultSkin();
            OnUpradeEffect();
            return;
        }

        SetAttrByBuff(handler);
        
        var skinID = handler.GetSkin();
        if (skinID > 0)
            skin.SetAdvanceSkin();

        OnUpradeEffect();
    }

    private void SetAttrByBuff(BuffHandler handler)
    {
        if(handler == null)
            return;
        
        var count = handler.Count();
        var buffattack = handler.GetAttack();
        var buffhp = handler.GetHp();

        var selfBuffAtc = Config.buffAttack * count;
        var selfBuffhp = Config.buffHP * count;

        var _attack = buffattack + selfBuffAtc;
        var _hp = buffhp + selfBuffhp;
        
        BuffAttack = _attack;
        
        var addhp = _hp - BuffHP;
        if (addhp < 0)
        {
            if (HP > MaxHP)
            {
                if (_hp <= 0)
                    HP = MaxHP;
                else
                    HP += addhp;
            }
            else
            {
                //不改变血量
            }
        }
        else
            HP += addhp;
        BuffHP = _hp;
    }
}

