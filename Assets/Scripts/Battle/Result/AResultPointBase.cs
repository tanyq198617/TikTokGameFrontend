using System;
using System.Collections;
using System.Collections.Generic;
using HotUpdateScripts;
using TMPro;
using UnityEngine;

/// <summary>
/// 结算点
/// </summary>
public abstract class AResultPointBase : AItemPageBase
{
    protected ResultComponent resultComponent;
    protected CapsuleCollider collision;

    /// <summary> 阵营血量和连胜数据显示 /// </summary>
    protected AHpAndWinningStreakBase hpAndWinningStreak;
   
    public int MaxHP { get; private set; }
    
    public int HP { get; private set; }
    
    public int ShieldHP { get; private set; }
    
    public int Attack { get; private set; }

    protected bool IsRed => camp == CampType.红;

    /// <summary> 结算点阵营 /// </summary>
    protected abstract CampType camp { get; }
    protected abstract int GetResultLayer();

    protected event Action<int, int> onValueChanged;
    public void OnValueChanged(Action<int, int> callback) => onValueChanged += callback;
    public void RemoveListener() => onValueChanged = null;

    private TMapData _mapData;
    private int[][] hurts;

    protected abstract AHpAndWinningStreakBase CreatePageNoClone();

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        resultComponent = Trans.GetOrAddComponent<ResultComponent>();
        resultComponent.AddCollision(OnCollision);
        collision = resultComponent.GetComponent<CapsuleCollider>();
        hpAndWinningStreak = CreatePageNoClone();
    }

    public override void Refresh()
    {
        base.Refresh();
        _mapData = LevelGlobal.Instance.DataBase.MapSence;
        hurts = _mapData.hurt;
        resultComponent.SetResultLayer(GetResultLayer(), _mapData.interval.Float());
        this.HP = MaxHP = LevelGlobal.Instance.DataBase.GetMaxHP();
        this.Attack = LevelGlobal.Instance.DataBase.GetAttack();
        this.ShieldHP = 0;
        OnValueChanged(hpAndWinningStreak.RefreshHp);
        hpAndWinningStreak.IsActive = true;
    }

    /// <summary>
    /// 增加护盾
    /// </summary>
    public void AddShield(int count)
    {
        ShieldHP += count;
        
        //最大护盾
        if (ShieldHP >= TGlobalDataManager.maxShield)
            ShieldHP = TGlobalDataManager.maxShield;
        
        if (onValueChanged != null) 
            onValueChanged(HP, ShieldHP);
    }

    /// <summary>
    /// 结算 
    /// </summary>
    protected virtual void OnCollision(Collision other)
    {
        if (HP <= 0)
        {
            EventMgr.Dispatch(BattleEvent.Battle_GameIsOver, camp);
            return;
        }

        var ball = BallFactory.Find(other.gameObject);
        if(ball == null) return;
        if(_mapData == null) return;
        bool isResult = ball.OnBossCollision(Attack);
        if (!isResult) return;
        OnResult(GetHurt());
        if (onValueChanged != null) onValueChanged(HP, ShieldHP);
        EventMgr.Dispatch(BattleEvent.Battle_BOSS_HP_Changed, HP, ShieldHP);
    }

    public int GetHurt()
    {
        if (hurts == null || hurts.Length <= 0)
            return 1;
        for (int i = 0; i < hurts.Length; i++)
        {
            if (HP > hurts[i][0])
            {
                return hurts[i][1];
            }
        }
        return 1;
    }

    protected void OnResult(int attack)
    {
        if (ShieldHP > 0)
        {
            //所收攻击大于护盾
            if (attack > ShieldHP)
            {
                //需要扣除所有护盾，和部分血量
                var ahp = attack - ShieldHP;
                ShieldHP = 0;
                HP -= ahp;
                if(HP < 0) HP = 0;
            }
            else
            {
                //护盾大于等于受到的伤害
                ShieldHP -= attack;
            }
            return;
        }
        HP -= attack;
        if(HP < 0) HP = 0;
    }

    /// <summary>
    /// 小爆炸对结算点攻击 
    /// </summary>
    public void SmallExplosionAttack(int attack)
    {
        if (HP <= 0)
        {
            EventMgr.Dispatch(BattleEvent.Battle_GameIsOver, camp);
            return;
        }
        OnResult(attack);
        if (onValueChanged != null) onValueChanged(HP, ShieldHP);
        EventMgr.Dispatch(BattleEvent.Battle_BOSS_HP_Changed, HP, ShieldHP);
    }

    public override void Close()
    {
        base.Close();
        _mapData = null;
        RemoveListener();
        resultComponent?.Clear();
        hpAndWinningStreak.IsActive = false;
    }
}
