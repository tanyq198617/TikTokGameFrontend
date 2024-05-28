using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

/// <summary>
/// 地图控制器
/// </summary>
public class MapControl : AItemPageBase
{
    private ACampControllerBase redController;
    private ACampControllerBase blueController;
    
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        blueController = UIUtility.BindPageNoClone(BlueCampController.Instance, Trans, "blue");
        redController = UIUtility.BindPageNoClone(RedCampController.Instance, Trans, "red");
        Object.DontDestroyOnLoad(m_gameobj);
    }

    public override void Refresh()
    {
        base.Refresh();
        redController.IsActive = true;
        blueController.IsActive = true;
    }

    public override void Close()
    {
        base.Close();
        redController.IsActive = false;
        blueController.IsActive = false;
    }

    public Transform GetRichManTrans(CampType camp, int i)
    {
        return camp == CampType.红 ? redController.GetRichManTrans(i) : blueController.GetRichManTrans(i);
    }
}
