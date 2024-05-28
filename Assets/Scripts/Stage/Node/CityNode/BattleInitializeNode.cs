using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 战斗数据初始化
/// </summary>
public class BattleInitializeNode : AStateNode
{
    protected override void Begin()
    {
        CityGlobal.Instance.Release();
        PlayerModel.Instance.initialize();
        FirstGiftMgr.Instance.initialize();
        UIMgr.Instance.ShowUI(UIPanelName.MapView);
    }

    private void OnGameStart()
    {
        IsComplete = true;
        GameStartAnimatorView.Instance.OpenUI();
    }
    
    public override void SysUpdate()
    {
        if (IsComplete)
            _machine.RunNextNode(this);
    }

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener<int>(BattleEvent.Battle_Map_Selected, OnMapSelect);
        EventMgr.AddEventListener(BattleEvent.Battle_GameIsStart, OnGameStart);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener<int>(BattleEvent.Battle_Map_Selected, OnMapSelect);
        EventMgr.RemoveEventListener(BattleEvent.Battle_GameIsStart, OnGameStart);
    }

    private void OnMapSelect(int mapId)
    {
        Debuger.LogWarning($"初始化开始");
        LevelGlobal.Instance.Init<GeneralLevelDataBase>(mapId, null);
        NetMgr.GetHandler<BattleHandler>()?.ReqGameStart();
    }
}
