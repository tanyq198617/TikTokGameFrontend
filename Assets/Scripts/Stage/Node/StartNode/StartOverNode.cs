using Cysharp.Threading.Tasks;
using HotUpdateScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartOverNode : AStateNode
{
    protected override void Begin()
    {
        GameConst.IsFristLogin = true;
        
        //加载hud图集
        //HudSpriteMgr.Instance.LoadSpriteInfos("atlas_hud");
        GameObjectFactory.Register();
        // FactoryRegister.Initialize();

        //切到登录场景
        GameStageMachine.ChangeState<LoginStage>();
    }
}
