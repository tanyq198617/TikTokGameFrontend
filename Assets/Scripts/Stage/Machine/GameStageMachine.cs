using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStageMachine
{
    private static StateMachine _machine;

    static GameStageMachine()
    {
        _machine = new StateMachine(null);
        _machine.AddNode<StartStage>();
        _machine.AddNode<LoginStage>();
        _machine.AddNode<CityStage>();
        _machine.AddNode<BattleStage>();
    }

    public static string GetCurrentStage() => _machine?.CurrentNode;

    public static bool InStage<TNode>() where TNode : GameStage 
    {
        var nodeType = typeof(TNode);
        var nodeName = nodeType.FullName;
        return nodeName.Equals(_machine.CurrentNode);
    }

    public static void ChangeState<TNode>(bool force = false) where TNode : GameStage
    {
        if (!force && InStage<TNode>()) 
        {
            Debuger.LogWarning($"[切换舞台] 当前已在【{_machine.CurrentNode}】舞台中...");
            return;
        }
        _machine?.ChangeState<TNode>();
    }

    public static void Run() => _machine.Run<StartStage>(); 
    public static void SysUpdate() => _machine?.SysUpdate();
    public static void FixedUpdate() => _machine?.FixedUpdate();
    public static void LateUpdate() => _machine?.LateUpdate();
}
