using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

/// <summary>
/// 游戏主城场景
/// </summary>
public class CityStage : GameStage
{
    public override string GetSceneName() => LevelGlobal.Instance.DataBase.GetMapName();

    public override void SysUpdate() => _machine?.SysUpdate();

    private StateMachine _machine;

    /// <summary> 当前地图路径 </summary>
    private string curMapName = string.Empty;

    public override void Begin()
    {
        if (_machine == null)
        {
            _machine = new StateMachine(this);
            _machine.AddNode<CityMaskNode>();
            _machine.AddNode<BattleInitializeNode>();
            _machine.AddNode<LoadSceneNode>();
            _machine.AddNode<BattleSceneNode>();
            _machine.AddNode<CityUnMaskNode>();
            _machine.AddNode<CityGameNode>();
            _machine.AddNode<CityReqRecordNode>();
            _machine.AddNode<BattleResultNode>();
        }
        _machine.Run<CityMaskNode>();
    }
    public override void OnLoadSceneSucceed(string sceneName)
    {
        if (GetSceneName().Equals(sceneName))
        {
            curMapName = sceneName;
            _machine.ChangeState<BattleSceneNode>();
        }
    }

    public override void End()
    {
        //房间数据
        RoomModel.Instance.Clear();
        CityGlobal.Instance.Release();
        
        SoundMgr.Instance.Default_BGM();
    }

    public override async UniTask LoadSceneAsync(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debuger.LogFatal($"地图路径为空!!!");
            return;
        }

        //如果是同地图则不重新加载
        if (path.Equals(curMapName))
            return;

        // string path = PathConst.GetArtScenePath(scendName);
        MapMgr.Instance.OnDestory();
        var async = YooAssets.LoadSceneAsync(path);
        await async.ToUniTask(this);
    }

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener(GameEvent.Game_Socket_SendClientKey, OnGameSocketReconnect);
        EventMgr.AddEventListener(GameEvent.Login_Succeed, OnReLoginSucceed);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener(GameEvent.Game_Socket_SendClientKey, OnGameSocketReconnect);
        EventMgr.RemoveEventListener(GameEvent.Login_Succeed, OnReLoginSucceed);
    }

    private void OnGameSocketReconnect()
    {
        NetMgr.GetHandler<LoginHandler>().ReqLogin();
    }

    private void OnReLoginSucceed()
    {
        // if (!_machine.InNode<CityGameNode>())
        // {
        //     _machine?.ChangeState<CityMaskNode>(); 
        // }
    }
}
