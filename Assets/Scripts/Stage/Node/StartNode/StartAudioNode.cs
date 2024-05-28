using HotUpdateScripts;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StartAudioNode : AStateNode
{
    protected override void Begin()
    {
        //初始化声音
        //BGMManager.Initialize();
        //SEManager.Initialize();
        DOTween.SetTweensCapacity(tweenersCapacity:2000, sequencesCapacity:3000);
        IsComplete = true;
    }

    public override void SysUpdate()
    {
        if (IsComplete)
        {
            _machine.RunNextNode(this);
        }
    }
}
