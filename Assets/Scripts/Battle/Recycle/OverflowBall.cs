using System.Collections;
using System.Collections.Generic;
using HotUpdateScripts;
using UnityEngine;

/// <summary>
/// 溢出的球球
/// </summary>
public struct OverflowBall
{
    public CampType Camp;
    public TBallData Config;
    public PlayerInfo Player;
    public Vector3 Position;
    public bool IsShowHead;

    public static OverflowBall Create(ABallBase ball)
    {
        return new OverflowBall()
        {
            Camp = (CampType)ball.Camp,
            Config = ball.Config,
            Player = ball.Owner,
            IsShowHead = ball.IsShowHead,
            Position = ball.GetTransform().position
        };
    }
}
