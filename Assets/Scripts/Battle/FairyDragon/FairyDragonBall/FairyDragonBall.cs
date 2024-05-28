using System.Collections.Generic;
using DG.Tweening;
using HotUpdateScripts;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

/// <summary>
/// 神龙炸弹,播两个timeling动画,一个出场动画,一个循环动画
/// 固定播10次,对方全部场上兵掉血
/// </summary>
public class FairyDragonBall : AItemBase
{
    /// <summary> 所属角色 </summary>
    public PlayerInfo info;

    /// <summary> 阵营，1=红，2=蓝 </summary>
    public bool isRed;

    /// <summary> 攻击力 </summary>
    public int Attack { get; private set; }

    private AFairyDragonEffectBase chuChangEffect;
    protected Sequence _tween;

    private TickedBase _tick;

    /// <summary> 这个ball结束后是否清除在工厂里面的当前正在播的记录 /// </summary>
    public bool isCloseBall;
    //攻击结算时间间隔
    private float attackInterval = 0.117f;

    public void OnInit(PlayerInfo info)
    {
        this.isRed = info.campType == CampType.红;
        this.info = info;
        Attack = TGlobalDataManager.Instance.fairyDragonBallCfg.harm;
        attackInterval = TGlobalDataManager.Instance.fairyDragonBallCfg.attackInterval;
        isCloseBall = true;
        OnBegin();
    }

    /// <summary>
    /// 开始攻击特效,先播一个出场特效
    /// 再播一个循环特效,循环攻击十次,总时长有特效时长控制,攻击间隔也有代码配合特效
    /// </summary>
    public virtual void OnBegin()
    {
        ClearTween();
        chuChangEffect = FairyDragonEffectFactory.GetOrCreate(isRed);
        _tween = DOTween.Sequence();
        _tween.AppendInterval(0.3f);
        _tween.AppendCallback(() => { AudioMgr.Instance.PlaySoundName(13); });
        _tween.AppendInterval(1.5f);

        _tween.AppendCallback(AddAttackTickedMgr);
        int index = 0;
        for (int i = 0; i < 10; i++)
        {
            _tween.AppendCallback(() =>
            {
                EventMgr.Dispatch(BattleEvent.Battle_Camera_Shake, 2);
                AudioMgr.Instance.PlaySoundName(5);
                index++;
                if (index == 10)
                {
                    CheckPlayBall();
                }
            });
            _tween.AppendInterval(1.17f);
        }

        _tween.AppendCallback(() =>
        {
            _tick.IsPause = true;
            FairyDragonBallFactory.Recycle(this);
        });
        _tween.Play();
    }

    /// <summary> 检测是否播放下一个神龙炸弹 /// </summary>
    private void CheckPlayBall()
    {
        isCloseBall = FairyDragonBallFactory.CheckPlayFairyDragon(info.campType);
    }
    
    /// <summary> 添加攻击结算计算方法,开始攻击前想加一个震屏和音效 /// </summary>
    private void AddAttackTickedMgr()
    {
        _tick ??= TickedBase.Create(attackInterval, SetAttack, false, true);
    }
    
    /// <summary>
    /// 攻击,对面全部掉血
    /// </summary>
    private void SetAttack()
    {
        var balls = BallFactory.GetAllBall(isRed ? CampType.蓝 : CampType.红);
        for (int i = balls.Count - 1; i >= 0; i--)
            balls[i]?.OnResult(Attack);
    }

    private void ClearTween()
    {
        if (_tween != null)
        {
            _tween.Kill(false);
            _tween = null;
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

    /// <summary> 回收 </summary>
    public void Recycle()
    {
        if (chuChangEffect != null)
        {
            chuChangEffect.Recycle();
            chuChangEffect = null;
        }

        ClearTick();
        ClearTween();
        info = null;
    }
}