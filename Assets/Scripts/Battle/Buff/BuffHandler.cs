using System;
using System.Collections;
using System.Collections.Generic;
using HotUpdateScripts;
using UnityEngine;

/// <summary>
/// BUFF处理器类
/// 对应一种BUFF
/// </summary>
public class BuffHandler
{
    /// <summary> BUFFid </summary>
    public int BuffID { get; private set; }
    
    /// <summary> BUFF配置 </summary>
    public TBuffData Config { get; private set; }
    
    /// <summary> 阵营 </summary>
    public CampType CampType { get; private set; }

    /// <summary> buff集合 </summary>
    protected readonly ListMgrT<Buff> buffs = new ListMgrT<Buff>();

    public void AddBuff()
    {
        if (Config.max > 0 && buffs.Count > Config.max)
        {
            if (Config.time <= 0)
                return;
            //刷新第一个BUFF的时间
            var buff = buffs.m_list[0];
            buff.UpdateTime();
            buffs.m_list.Sort(SortByTime);
        }
        else
        {
            buffs.Add(Buff.Create(Config, this));
        }
    }

    private int SortByTime(Buff b1, Buff b2)
    {
        var f1 = b1.LeftTimne();
        var f2 = b2.LeftTimne();
        if (f1 > f2)
            return 1;
        if (f1 < f2)
            return -1;
        return 1;
    }

    public bool IsComplete()
    {
        return buffs.Count <= 0;
    }

    public void Update()
    {
        buffs.Update();
    }

    /// <summary>
    /// 获得攻击力 
    /// </summary>
    public int GetAttack()
    {
        return buffs.Count * (Config?.attack ?? 0);
    }
    
    /// <summary>
    /// 获得血量
    /// </summary>
    public int GetHp()
    {
        return buffs.Count * (Config?.hp ?? 0);
    }

    public int Count()
    {
        return buffs.Count;
    }

    /// <summary>
    /// 获得血量
    /// </summary>
    public int GetSkin()
    {
        return Config?.skin ?? 0;
    }

    public void Clear()
    {
        buffs.PutBackObj();
        Config = null;
        BuffID = 0;
    }

    public static BuffHandler Create(CampType campType, int buffId)
    {
        return new BuffHandler()
        {
            BuffID = buffId,
            CampType = campType,
            Config = TBuffDataManager.Instance.GetItem(buffId)
        };
    }
}
