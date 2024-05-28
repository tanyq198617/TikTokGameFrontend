using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    public static Scene Scene { get; private set; } = new Scene();

    /// <summary> 普通帧刷新 </summary>
    public static void Update()
    {
        GameStageMachine.SysUpdate();
    }

    /// <summary> 固定帧刷新(带补偿) </summary>
    public static void FixedUpdate()
    {
        GameRunTime.GameFrameTime++;
        TimeMgr.Instance.FixedUpdate(Time.fixedDeltaTime);//(GameConst.FixedFrameTime);
        TickedMgr.Instance.FixedUpdate();
        GameStageMachine.FixedUpdate();
    }

    public static void LateUpdate()
    {
        GameStageMachine.LateUpdate();
    }

    /// <summary> 跳出游戏 </summary>
    public static void OnBackStage()
    {
        Debuger.LogWarning($"[跳出游戏] 跳出了界面");
    }

    /// <summary> 回到游戏 </summary>
    public static void OnFrontStage(float leaveTime)
    {
        Debuger.LogWarning($"[回到游戏] 一共离开了{leaveTime}秒");
    }

    public static void OnDestroy()
    {
        Scene.OnDespose();
    }
}
