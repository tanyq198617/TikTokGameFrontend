using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

/// <summary>
/// 登录舞台, 切换账号时到这个舞台
/// </summary>
public class LoginStage : GameStage
{
    public override string GetSceneName() => SceneConst.Stage_Login;
    public override void SysUpdate() => _machine?.SysUpdate();

    private StateMachine _machine;

    public override void Begin()
    {
        if (_machine == null)
        {
            _machine = new StateMachine(this);
            _machine.AddNode<LoginMaskNode>();
            _machine.AddNode<LoadSceneNode>();
            _machine.AddNode<LoginPreloadNode>();
            _machine.AddNode<LoginCheckNetNode>();
            _machine.AddNode<LoginShowNode>();
        }
        _machine.Run<LoginMaskNode>();
    }
    public override void OnLoadSceneSucceed(string sceneName)
    {
        if (GetSceneName().Equals(sceneName))
            _machine.ChangeState<LoginPreloadNode>();
    }

    public override void End()
    {
        SoundMgr.Instance.Default_BGM();
    }
    
    public override async UniTask LoadSceneAsync(string scendName)
    {
        string path = PathConst.GetScenePath(scendName);
        var async = YooAssets.LoadSceneAsync(path);
        await async.ToUniTask(this);
    }
}
