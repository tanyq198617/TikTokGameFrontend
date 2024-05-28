using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickedBase : IClear
{
    float _time = 0;

    public object TickObj { get; set; }

    public int Priority { get; set; }

    public float TickLength { get; set; }

    public Action OnTicked { set; get; }

    public Action<object> OnTickedObj { set; get; }

    public TickedBase() { IsPause = false; }

    public static TickedBase Create(float tickTime, Action tickBack, bool isPause = true, bool isTickMgr = false)
    {
        TickedBase tickBase = new TickedBase();
        tickBase.TickLength = tickTime;
        tickBase.OnTicked = tickBack;
        tickBase.IsPause = isPause;
        if (isTickMgr) TickedMgr.Instance.Add(tickBase);
        return tickBase;
    }

    public void Reset()
    {
        IsPause = false;
        _time = 0;
    }

    public bool IsPause { set; get; }

    public void SetExcuteImme()
    {
        IsPause = false;
        OnTicked();
        _time = 0;
    }

    public void Clear()
    {
        OnTicked = null;
        OnTickedObj = null;
        IsPause = false;
        _time = 0;
    }

    public void FixedUpdate()
    {
        if (!IsPause)
        {
            if (MathUtility.IsOutTime(ref _time, TickLength))
            {
                OnTicked();
                //OnTickedObj(TickObj);
                _time = 0;
            }
        }
    }
}
