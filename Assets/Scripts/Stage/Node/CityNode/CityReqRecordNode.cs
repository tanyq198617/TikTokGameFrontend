using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 上报战斗信息
/// </summary>
public class CityReqRecordNode : AStateNode
{
    protected float delay = 0;
    protected float timer = 0;

    /// <summary> 上报积分成功 /// </summary>
    private bool reqKillRecord;

    /// <summary> 结算动画播放完成 /// </summary>
    private bool playAnimatorEnd;
    
    protected override void Begin()
    {
        reqKillRecord = false;
        playAnimatorEnd = false;
        
        TimeMgr.Instance.Clear();
        BallFactory.Stop();
        BallEffectFactory.RecycleAll();
        BallTimeLineFactory.RecycleAll();
        ShotFactory.Stop();
        FairyDragonBallFactory.RecycleAll();
        SmallExplosionController.Instance.Recycle();
        ThunderboltController.Instance.Recycle();
   
        UIMgr.Instance.CloseUI(UIPanelName.GiftSideView);
        UIMgr.Instance.CloseUI(UIPanelName.BuffView);
        UIMgr.Instance.CloseUI(UIPanelName.SettingView);
        Debuger.LogWarning($"游戏结束，客户端准备上报数据...");
        NetMgr.GetHandler<ResultHandler>()?.ReqKillRecord(PlayerModel.Instance.GetRecords()).Forget();
        
        GameOverInterludeAnimation.Instance.OnBegin();
    }

    /// <summary>
    /// 上报积分成功
    /// </summary>
    private void OnRetKillRecord()
    {
        NetMgr.GetHandler<ResultHandler>()?.ReqGameResult();
    }

    /// <summary>
    /// 上报结算结果成功 
    /// </summary>
    private void OnGameResult()
    {
        delay = RankModel.Instance.delay;
        timer = 0;
        reqKillRecord = true;
        // IsComplete = true;
    }

    protected override void End()
    {
        reqKillRecord = false;
        playAnimatorEnd = false;
    }
    
    public override void SysUpdate()
    {
        if (!IsComplete)
        {
            if (reqKillRecord && playAnimatorEnd)
                IsComplete = true;
        }
        
        if (IsComplete)
        {
            if (MathUtility.IsOutTime(ref timer, delay))
            {
                _machine.RunNextNode(this);
            }
        }
    }

    /// <summary> 结算动画结束 /// </summary>
    private void AnimatorEnd()
    {
        playAnimatorEnd = true;
    }
    
    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener(BattleEvent.Battle_S2C_RetKillRecord, OnRetKillRecord);
        EventMgr.AddEventListener(BattleEvent.Battle_S2C_RetGameResult, OnGameResult);
        EventMgr.AddEventListener(BattleEvent.Battle_GameOverAnimatorEnd, AnimatorEnd);
    }
    
    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener(BattleEvent.Battle_S2C_RetKillRecord, OnRetKillRecord);
        EventMgr.RemoveEventListener(BattleEvent.Battle_S2C_RetGameResult, OnGameResult);
        EventMgr.RemoveEventListener(BattleEvent.Battle_GameOverAnimatorEnd, AnimatorEnd);
    }
}
