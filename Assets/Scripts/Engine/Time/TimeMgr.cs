using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public abstract class DelayActionBase
{
    protected Action _callBack;
    public bool IsEnd;
    public abstract void Update();
}

public class DelayFrameAction : DelayActionBase
{
    private int _startFrame;
    private int _maxFrame;

    public void Init(int maxFrame, Action callBack)
    {
        this._maxFrame = maxFrame;
        this._callBack = callBack;
        this._startFrame = TimeMgr.GameFrameTime;
        this.IsEnd = false;
    }

    public override void Update()
    {
        if (!IsEnd && TimeMgr.GameFrameTime >= (_startFrame + _maxFrame))
        {
            IsEnd = true;
            if (_callBack != null)
                _callBack();
        }
    }
}

public unsafe class DelayTimeAction : DelayActionBase
{
    private float _delayTime;

    private float _runTime;
    
    private float* _realTimePointer;

    public void Init(float* realTimePointer, float delayTime, Action callBack)
    {
        this._delayTime = delayTime;
        this._callBack = callBack;
        this._runTime = *realTimePointer; // 使用解引用来获取_realTime的值   
        this._realTimePointer = realTimePointer;
        IsEnd = false;
    }

    public void SetLength(float time) => _delayTime = time;
    public float GetLength() => _delayTime;
    public float GetLeftTime() => _runTime + _delayTime - *_realTimePointer;

    public override void Update()
    {
        if (!IsEnd && *_realTimePointer >= (_runTime + _delayTime))
        {
            IsEnd = true;
            if (_callBack != null)
                _callBack();
        }
    }
}

public unsafe class TimeMgr : Singleton<TimeMgr>
{
    private static float _realTime;
    public static float RealTime => _realTime;
    
    private static float* _realTimePointer;
    
    public static int GameFrameTime;
    public float AccNum { private set; get; }
    
    public float DeltaTime => Time.deltaTime * AccNum;
    
    private readonly List<DelayActionBase> delayList = new List<DelayActionBase>(); 

    public TimeMgr()
    {
        AccNum = 1.0f;
        
        fixed (float* ptr = &_realTime)
        {
            _realTimePointer = ptr;
        }
    }

    public void Init()
    {
        TimeScale = 1.0f;
    }

    public void Clear()
    {
        TimeScale = 1.0f;
        IsPause = false;
    }

    public bool IsPause
    {
        set => Time.timeScale = value ? 0 : AccNum;
        get => (Time.timeScale == 0);
    }

    public float TimeScale
    {
        set
        {
            AccNum = value;
            Time.timeScale = value;
        }

        get => Time.timeScale;
    }

    /// <summary>
    /// 延迟多少秒开始执行
    /// </summary>
    /// <param name="time">延迟多少秒</param>
    /// <param name="callBack">延迟(秒)触发事件</param>
    public DelayTimeAction Delay(float time, Action callBack)
    {
        DelayTimeAction delay = new DelayTimeAction();
        delay.Init(_realTimePointer, time, callBack);
        delayList.Add(delay);
        return delay;
    }

    /// <summary>
    /// 延迟多少帧开始执行
    /// </summary>
    /// <param name="maxFrame">延迟多少帧</param>
    /// <param name="callBack">延迟(帧)触发事件</param>
    public DelayFrameAction DelayFrame(int maxFrame, Action callBack)
    {
        DelayFrameAction delay = new DelayFrameAction();
        delay.Init(maxFrame, callBack);
        delayList.Add(delay);
        return delay;
    }

    public void ClearDelay(DelayActionBase delay)
    {
        if (delay != null && delayList.Count != 0)
        {
            for (int i = delayList.Count - 1; i >= 0; i--)
            {
                if (delayList[i] == delay)
                {
                    delayList.RemoveAt(i);
                    return;
                }
            }
        }
    }

    public void ClearAllDelay()
    {
        if (delayList != null) delayList.Clear();
    }

    public void FixedUpdate(float deltaTime)
    {
        GameFrameTime++;
        
        if (!IsPause)
        {
            _realTime += deltaTime;
            var count = delayList.Count;
            if (count != 0)
            {
                for (int i = count - 1; i >= 0; i--)
                {
                    if (i < count)
                    {
                        var action = delayList[i];
                        if (action != null) action.Update();
                        if (action is { IsEnd: true })
                        {
                            delayList.RemoveAt(i);
                            count--;
                        }
                    }
                }
            }
        }
    }
}

public static class DelayTimeExtensions
{
    public static void Kill(this DelayActionBase act, bool isClear = false)
    {
        if (act == null)
            return;
        act.IsEnd = true;
        if (isClear) act = null;
    }
    
    public static void Recycle(this DelayActionBase act)
    {
        if (act == null)
            return;
        TimeMgr.Instance.ClearDelay(act);
        act = null;
    }
}
