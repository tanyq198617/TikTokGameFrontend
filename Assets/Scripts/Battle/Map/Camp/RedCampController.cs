using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红方阵营管理器
/// </summary>
public class RedCampController : ACampControllerBase
{
    public static RedCampController Instance => Singleton<RedCampController>.Instance;

    protected override CampType CampType { get; } = CampType.红;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        resultPoint = UIUtility.CreatePageNoClone<RedResultPoint>(Trans, "result");
        shotPointEffectController = UIUtility.CreateItemNoClone<RedShotPointEffectController>(m_gameobj);
    }
    
}
