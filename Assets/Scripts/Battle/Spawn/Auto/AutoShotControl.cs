using System.Collections;
using System.Collections.Generic;
using HotUpdateScripts;
using UnityEngine;

/// <summary>
/// 自动发射控制器
/// </summary>
public class AutoShotControl
{
    public static readonly HashSet<string> PlayerHashSet = new HashSet<string>();
    public static readonly ListMgrT<AutoShotTick> ticks = new ListMgrT<AutoShotTick>();

    /// <summary>
    /// 压入自动增加点赞子弹
    /// </summary>
    public static void EnterToAutoShot(PlayerInfo info)
    {
        if(!PlayerHashSet.Add(info.openid))
            return;
        var autoShot = ClassFactory.GetOrCreate<AutoShotTick>();
        autoShot.OnInit(info, TOperateDataManager.Instance.GetItem(TGlobalDataManager.AutoShot_GiftId));
        ticks.Add(autoShot);
    }

    public static void SysUpdate()
    {
        ticks.Update();
    }

    public static void Release()
    {
        ticks.PutBackObj();
        PlayerHashSet.Clear();
    }
}
