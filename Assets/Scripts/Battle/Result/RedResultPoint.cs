using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红方结算点
/// </summary>
public class RedResultPoint : AResultPointBase
{
    protected override int GetResultLayer() => Layer.BlueBall;
    protected override CampType camp => CampType.红;
    protected override AHpAndWinningStreakBase CreatePageNoClone() => UIUtility.CreatePageNoClone<RedHpAndWinningStreak>(Trans, "object_texts");
}
