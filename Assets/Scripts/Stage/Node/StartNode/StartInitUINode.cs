using HotUpdateScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartInitUINode : AStateNode
{
    protected override void Begin()
    {
        //初始化UI系统
        UIRegister.Initialize();
    }

    public override void SysUpdate()
    {
        if (UIRegister.IsCompleted)
            _machine.RunNextNode(this);
    }
}
