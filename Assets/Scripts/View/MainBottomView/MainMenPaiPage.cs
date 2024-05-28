using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenPaiPage : AMainBottomBase
{
    public override int GetIndex() => 4;

    public override void OnSelect()
    {
        base.OnSelect();

        UIMgr.Instance.ShowUI(UIPanelName.UnionView);
    }

    public override void NoSelect()
    {
        base.NoSelect();

        UIMgr.Instance.CloseUI(UIPanelName.UnionView);
    }
}
