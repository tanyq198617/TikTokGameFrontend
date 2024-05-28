using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 蓝方阵营管理器
/// </summary>
public class BlueCampController : ACampControllerBase
{
    public static BlueCampController Instance => Singleton<BlueCampController>.Instance;

    protected override CampType CampType { get; } = CampType.蓝;
    
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        resultPoint = UIUtility.CreatePageNoClone<BlueResultPoint>(Trans, "result");
        shotPointEffectController = UIUtility.CreateItemNoClone<BlueShotPointEffectController>(m_gameobj);
    }
}
