using System.Collections;
using System.Collections.Generic;
using HotUpdateScripts;
using UnityEngine;

/// <summary>
/// 礼物影响发射器
/// </summary>
public class GiftEffectMgr : Singleton<GiftEffectMgr>
{
    /// <summary>
    /// 影响发射器
    /// </summary>
    public static void OnEffect(CampType campType, string gift_id, long gift_num)
    {
        var effects = TOperateEffectManager.Instance.GetEffects(gift_id);
        if (effects == null || effects.Count <= 0) return;
        var controller = campType == CampType.红 ? (ACampControllerBase)RedCampController.Instance : (ACampControllerBase)BlueCampController.Instance;
        for (int i = 0; i < effects.Count; i++)
        {
            controller.OnFireAcceleSpeed(effects[i].fireIndex,(float)effects[i].acceleTime * gift_num); 
        }
    }
}
