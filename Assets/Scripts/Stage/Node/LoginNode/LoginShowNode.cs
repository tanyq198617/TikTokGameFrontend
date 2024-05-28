using HotUpdateScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginShowNode : AStateNode
{
    protected override void Begin()
    {
        //打开登录界面
        SDKMgr.Instance.OnAutoLogin(OpenViewSuccess);
    }

    private void OpenViewSuccess()
    {
        //打开遮罩界面后, 回收主工程界面
        UIRoot.Instance.ReleaseMainUI();
        UIMgr.Instance.CloseUI(UIPanelName.LoginMaskView);
        UIMgr.Instance.PreLoadUI(UIPanelName.MapView);
        UIMgr.Instance.PreLoadUI(UIPanelName.GameStartAnimatorView);
        UIMgr.Instance.PreLoadUI(UIPanelName.BattleView);
    }


    private void OnRetLoginSucceed()
    {
        // GameStageMachine.ChangeState<CityStage>();
    }

    private void OnRetLoginFail()
    {
        _machine.ChangeState<LoginCheckNetNode>();
    }

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener(ServerEvent.S2C_RetLogin_Succeed, OnRetLoginSucceed);
        EventMgr.AddEventListener(ServerEvent.S2C_RetLogin_Fail, OnRetLoginFail);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener(ServerEvent.S2C_RetLogin_Succeed, OnRetLoginSucceed);
        EventMgr.RemoveEventListener(ServerEvent.S2C_RetLogin_Fail, OnRetLoginFail);
    }
}
