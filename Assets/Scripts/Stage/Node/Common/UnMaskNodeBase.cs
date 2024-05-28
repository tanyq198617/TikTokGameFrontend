using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnMaskNodeBase : AStateNode
{
    protected override void Begin()
    {
        UIMgr.Instance.CloseUI(UIPanelName.LoadingView);
        IsComplete = true;
    }
    public override void SysUpdate()
    {
        if (IsComplete)
            RunNextNode();
    }

    protected abstract void RunNextNode();
}
