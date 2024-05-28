using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainShangchengPage : AMainBottomBase
{
    public override int GetIndex() => 0;

    public override void OnSelect()
    {
        base.OnSelect();

        UIMgr.Instance.ShowUI(UIPanelName.ShopView);
    }

    public override void NoSelect()
    {
        base.NoSelect();

        UIMgr.Instance.CloseUI(UIPanelName.ShopView);
    }
}
