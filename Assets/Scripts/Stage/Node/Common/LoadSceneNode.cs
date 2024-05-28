using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

/// <summary>
/// 通用场景加载状态机
/// </summary>
public class LoadSceneNode : AStateNode
{
    protected GameStage stage;
    protected string scendName;

    public override void OnCreate(StateMachine machine)
    {
        base.OnCreate(machine);
        stage = machine.Owner as GameStage;
    }

    protected override void Begin()
    {
        LoadScene(stage.GetSceneName());
    }

    public async void LoadScene(string scendName)
    {
        if (string.IsNullOrEmpty(scendName))
        {
            IsComplete = true;
            return;
        }

        this.scendName = scendName;
        await stage.LoadSceneAsync(scendName);
        IsComplete = true;
    }

    protected override void End()
    {
        scendName = string.Empty;
    }

    public override void SysUpdate()
    {
        if (IsComplete)
            stage.OnLoadSceneSucceed(scendName);
    }
}
