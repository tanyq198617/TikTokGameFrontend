using System;
using System.Collections;
using System.Collections.Generic;
using HotUpdateScripts;
using UnityEngine;

/// <summary>
/// 这是一个BUFF
/// </summary>
public class Buff : ICompleted
{
    /// <summary> BUFF配置 </summary>
    private TBuffData Config;

    private BuffHandler Owner; 

    private bool IsComplete;

    private float timer;
    private float interval;

    private void OnInit(TBuffData data, BuffHandler handler)
    {      
        this.Config = data;
        this.Owner = handler;
        this.interval = (float)data.time;
        this.IsComplete = false;
        this.timer = 0;
    }
    
    public void Update()
    {
        if (interval > 0 && MathUtility.IsOutTime(ref timer, interval))
        {
            IsComplete = true;
        }
    }

    public float LeftTimne()
    {
        return interval - (TimeMgr.RealTime - timer);
    }

    public bool IsCompleted()
    {
        return IsComplete;
    }

    public void Destory()
    {
        Config = null;
    }

    public void PutBackObj()
    {
        EventMgr.Dispatch(BattleEvent.Battle_BUFF_Recycle, Owner.CampType, Owner);
        ClassFactory.Recycle(this);
        Config = null;
        Owner = null;
    }

    public void UpdateTime()
    {
        timer = 0;
    }

    public static Buff Create(TBuffData data, BuffHandler handler)
    {
        var buff = ClassFactory.GetOrCreate<Buff>();
        buff.OnInit(data, handler);
        return buff;
    }
}
