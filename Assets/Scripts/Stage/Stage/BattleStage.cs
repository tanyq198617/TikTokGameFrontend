using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗舞台
/// </summary>
public class BattleStage : GameStage
{
    public override string GetSceneName() => SceneConst.Stage_City;

    public override void SysUpdate() => _machine?.SysUpdate();

    private StateMachine _machine;

    public override void Begin()
    {
        if (_machine == null)
        {
            _machine = new StateMachine(this);
            _machine.AddNode<BattleMaskNode>();
            _machine.AddNode<BattleSceneNode>();
            _machine.AddNode<BattleUnMaskNode>();
            _machine.AddNode<BattleGameNode>();
        }
        _machine.Run<BattleMaskNode>();
    }
    public override void OnLoadSceneSucceed(string sceneName)
    {
        if (GetSceneName().Equals(sceneName))
            _machine.ChangeState<BattleUnMaskNode>();
    }

    public override void End()
    {
        SoundMgr.Instance.Default_BGM();
    }
}
