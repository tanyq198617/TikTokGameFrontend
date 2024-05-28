using System.Collections;
using System.Collections.Generic;
using HotUpdateScripts;
using UnityEngine;

/// <summary>
/// 普通章节关卡数据
/// </summary>
public class GeneralLevelDataBase : ALevelDataBase
{
    // private TMapStage _stage;
    //
    // protected override TMapSence GetMapSence(int regionId)
    // {
    //     _stage = TMapStageManager.Instance.GetItem(regionId);
    //     return TMapSenceManager.Instance.GetItem(_stage.Sence);
    // }

    protected override void Release()
    {
        // _stage = null;
    }
}
