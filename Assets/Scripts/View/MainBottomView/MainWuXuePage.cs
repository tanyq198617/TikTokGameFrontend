using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWuXuePage : AMainBottomBase
{
    public override int GetIndex() => 3;

    public override void OnSelect()
    {
        base.OnSelect();

        UIMgr.Instance.ShowUI(UIPanelName.SkillView);
    }

    public override void NoSelect()
    {
        base.NoSelect();

        UIMgr.Instance.CloseUI(UIPanelName.SkillView);
    }
}
