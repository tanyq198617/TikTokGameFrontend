using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 登录遮罩
/// </summary>
public class LoginMaskNode : AStateNode
{
    protected override void Begin()
    {
        //打开遮罩界面后, 回收主工程界面
        UIMgr.Instance.ShowUICall(UIPanelName.LoginMaskView, Call);
    }

    private void Call()
    {
        UIRoot.Instance.ReleaseMainUI();
        UIMgr.Instance.CloseAllUI();
        IsComplete = true;
    }

    public override void SysUpdate()
    {
        if (IsComplete)
            _machine.RunNextNode(this);
    }
}
