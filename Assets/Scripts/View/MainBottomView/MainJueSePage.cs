using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainJueSePage : AMainBottomBase
{
    public override int GetIndex() => 1;

    public override void OnSelect()
    {
        base.OnSelect();

        UIMgr.Instance.ShowUI(UIPanelName.RoleView);
    }

    public override void NoSelect()
    {
        base.NoSelect();

        UIMgr.Instance.CloseUI(UIPanelName.RoleView);
    }
}
