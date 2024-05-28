using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 蓝方结算点
/// </summary>
public class BlueResultPoint : AResultPointBase
{
    protected override int GetResultLayer() => Layer.RedBall;
    protected override CampType camp => CampType.蓝;
    protected override AHpAndWinningStreakBase CreatePageNoClone() => UIUtility.CreatePageNoClone<BlueHpAndWinningStreak>(Trans, "object_texts");
}
