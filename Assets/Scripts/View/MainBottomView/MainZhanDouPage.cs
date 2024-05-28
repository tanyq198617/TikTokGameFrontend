using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainZhanDouPage : AMainBottomBase
{
    public override int GetIndex() => 2;

    public override void OnSelect()
    {
        base.OnSelect();

        UIMgr.Instance.ShowUI(UIPanelName.FightView);
    }

    public override void NoSelect()
    {
        base.NoSelect();

        UIMgr.Instance.CloseUI(UIPanelName.FightView);
    }
}
