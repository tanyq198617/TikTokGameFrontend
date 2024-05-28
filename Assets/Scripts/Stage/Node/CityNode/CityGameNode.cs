using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主界面战斗开始
/// </summary>
public class CityGameNode : AStateNode
{
    private CameraController _cameraController;
    protected override void Begin()
    {
        SmallExplosionController.Instance.OnInit();
        _cameraController = CameraMgr.Instance.cameraController;
        EventMgr.Dispatch(BattleEvent.Battle_Game_Begin);
    }

    protected override void End()
    {
    }
    
    public override void SysUpdate()
    {
        if (IsComplete)
            _machine.RunNextNode(this);
        BuffMgr.Update();
        _cameraController.SysUpdate();
        AutoShotControl.SysUpdate();
    }

    private void OnGameOver(CampType campType)
    {
        LevelGlobal.Instance.SetLost(campType);
        GiftTimeLineFactory.RecycleAll();
        AutoShotControl.Release();
        CityGlobal.Instance.IsOver = true;
        BallSoundMgr.Instance.Recycle();
        IsComplete = true;
    }

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener<CampType>(BattleEvent.Battle_GameIsOver, OnGameOver);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener<CampType>(BattleEvent.Battle_GameIsOver, OnGameOver);
    }
}
