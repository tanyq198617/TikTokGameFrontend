using System.Collections;
using System.Collections.Generic;
using HotUpdateScripts;
using UnityEngine;

public class MuzzleControl : AMuzzleControlBase
{
    /// <summary> 炮口数据 </summary>
    protected TFireData Config;
    
    /// <summary> 子弹炮点, key=ID, value=炮点数据 </summary>
    protected readonly Dictionary<int, AShotPointBase> shotDict = new Dictionary<int, AShotPointBase>();

    /// <summary> true=红方, false=蓝方 </summary>
    public bool IsRedCamp { get; private set; }

    private bool isAcceling = false;
    private DelayTimeAction _timer;

    /// <summary> 发射器加速最大时间 </summary>
    private float acceleMaxTime;

    public void OnInit(TFireData data, bool mirror)
    {
        this.Config = data;
        this.IsRedCamp = mirror;
        this.isAcceling = false;
        InitializeMuzzle(mirror);
        InitializeShotPoint();
        acceleMaxTime = (float)Config.acceleMaxTime;
    }

    /// <summary>
    /// 初始化炮口
    /// </summary>
    private void InitializeMuzzle(bool mirror)
    {
        float fy = (float)Config.angle;
        float ty = mirror ? -fy * 0.5f : 180 - fy * 0.5f;
        var from = new Vector3(0, fy, 0);  
        var to = new Vector3(0, ty, 0);   
        fire.Init(to, from, (float)Config.speed); 
#if UNITY_EDITOR
        fire.SetName(Config.name);
#endif
    }

    /// <summary>
    /// 初始化发射点
    /// </summary>
    private void InitializeShotPoint()
    {
        var arr = TFirePointManager.Instance.GetAllItem();
        for (int i = 0; i < arr.Length; i++)
        {
            if (Config.Id == arr[i].mountType)
            {
                var shotPoint = ShotFactory.GetOrCreate(arr[i].Id, IsRedCamp);
                shotPoint.OnInit(IsRedCamp, Config, arr[i], Trans);
                shotDict[arr[i].Id] = shotPoint;
            }
        }
    }

    /// <summary>
    /// 设置加速
    /// </summary>
    public void SetAccele(float interval)
    {
        //如果没加速，则进行加速处理
        if (!isAcceling)
        {
            isAcceling = true;
            if (interval > acceleMaxTime) interval = acceleMaxTime;
            // Debuger.Log($"[发射器加速开始时间]={TimeMgr.RealTime}");
            _timer = TimeMgr.Instance.Delay(interval, ClearAccele);
            fire.SetTweenSpeed(Config.acceleSpeed.Float());
            foreach (var kv in shotDict)
                kv.Value.SetAccele(true);
        }
        else
        {
            var len = _timer.GetLength();
            var left = _timer.GetLeftTime();
            if (left + interval > acceleMaxTime)
                interval = acceleMaxTime - left;
            len += interval;
            _timer.SetLength(len);
            // Debuger.Log($"[发射器加速增加时间] 总时间={len}秒");
        }
    }

    public void ClearAccele()
    {
        _timer = null;
        isAcceling = false;
        // Debuger.Log($"[发射器加速结束时间]={TimeMgr.RealTime}");
        fire.SetTweenSpeed(Config.speed.Float());
        foreach (var kv in shotDict)
            kv.Value.SetAccele(false);
    }

    protected void RecyleShotPoint()
    {
        foreach (var kv in shotDict)
            ShotFactory.Recycle(kv.Value);
        shotDict.Clear();
    }

    private void ClearTimer()
    {
        if (_timer != null)
        {
            TimeMgr.Instance.ClearDelay(_timer);
            _timer = null;
        }
    }

    protected override void AutoClear()
    {
        base.AutoClear();
        ClearTimer();
        RecyleShotPoint();
        acceleMaxTime = 0;
        Config = null;
    }
}
