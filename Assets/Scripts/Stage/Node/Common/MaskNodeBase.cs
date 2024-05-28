using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MaskNodeBase : AStateNode
{
    protected override void Begin()
    {
        //打开遮罩界面后, 回收主工程界面
        UIMgr.Instance.ShowUICall(UIPanelName.LoadingView, Call);
    }

    protected void Call()
    {
        IsComplete = true;
        UIMgr.Instance.CloseAllUI();
    }

    public override void SysUpdate()
    {
        if (IsComplete)
            RunNextNode();
    }

    protected abstract void RunNextNode();
}
