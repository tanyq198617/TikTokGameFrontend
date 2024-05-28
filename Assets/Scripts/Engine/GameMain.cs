using HotUpdateScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using GameNetwork;
using UnityEngine;

/// <summary>
/// 热更新程序集主入口
/// </summary>
public class GameMain : MonoBehaviour
{
    public static GameMain Instance { get; private set; }

    /// <summary> 当前运行平台 </summary>
    public static RuntimePlatform CurrentPlatform => Application.platform;

    private float _accumilatedTime = 0f;


    private void Awake()
    {
        Instance = this;
        this.name = nameof(GameMain);
        GameObject.DontDestroyOnLoad(this);
    }

    private void Start()
    {
        //防止锁屏
        var lv = Boot.GetValue<int>("LogLevel");
        Debuger.Initialize(true, lv, true);
        NetDebug.OnShowLog(Boot.IsNetLog());
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        // Debuger.LogWarning($"[GameMain] 热更程序集加载成功...");
        Debuger.LogWarning($"[GameMain] 程序集加载成功...");
        GameStageMachine.Run();
    }

    private void Update()
    {
        // #region 固定帧频
        // _accumilatedTime += Time.deltaTime;
        // while (_accumilatedTime > GameConst.FixedFrameTime)
        // {
        //     Game.FixedUpdate();
        //     _accumilatedTime -= GameConst.FixedFrameTime;
        // }
        // #endregion
        Game.Update();
    }

    private void FixedUpdate()
    {
        Game.FixedUpdate();
    }

    private void LateUpdate() => Game.LateUpdate();

    private void OnDestroy()
    {
        Game.OnDestroy();
    }

    #region 切后台事件
    bool _isPause = false;
    float _pauseTime = 0;

    private void OnEnable() => _isPause = false;

    public void OnApplicationPause(bool pause)
    {
        if (!_isPause && pause)
        {
            _pauseTime = Time.realtimeSinceStartup;
            Game.OnBackStage();
            _isPause = true;
        }
        else if (_isPause && !pause)
        {
            var leaveTime = Time.realtimeSinceStartup - _pauseTime;
            Game.OnFrontStage(leaveTime);
            _pauseTime = 0;
            _isPause = false;
        }
    }
    #endregion
}
