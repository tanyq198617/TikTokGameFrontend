using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class WatchTime
{
    private static Stopwatch _startTime = null;

    public static void Start()
    {
        if (_startTime == null)
            _startTime = new Stopwatch();
        _startTime.Reset();
        _startTime.Start();
    }

    public static void ShowTime(string sign)
    {
        if (_startTime != null)
        {
            _startTime.Stop();
            Debuger.LogError($"{sign} 测试时间为 {_startTime.ElapsedMilliseconds} ms");
        }
    }
}
