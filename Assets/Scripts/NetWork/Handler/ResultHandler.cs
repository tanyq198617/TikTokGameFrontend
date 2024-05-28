using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameNetwork;
using Sproto;
using SprotoType;
using UnityEngine;

/// <summary>
/// 结算处理器
/// </summary>
[TcpMsgHandle]
public class ResultHandler : ATcpHandler
{
    /// <summary> 单条消息上报最大个数 </summary>
    private const int onceCount = 100;
    
    public override void OnRegister()
    {
    }

    /// <summary>
    /// 上报击杀积分
    /// </summary>
    public async UniTask ReqKillRecord(List<Record> records)
    {
        int msgCount = Mathf.CeilToInt(records.Count * 1.0f / onceCount);
        int num = msgCount;
        
        Debuger.Log($"上报数量：{records.Count}, 分{msgCount}段上传, 每次{onceCount}个");
        
        for (int i = 0; i < msgCount; i ++)
        {
            NetMgr.Instance.SendTo<kill_record.request, C2SProtocol.kill_record>(
                (req) =>
                {
                    req.msg_list = new List<Record>();
                    int k = 0;
                    for (int j = i * onceCount; j < records.Count; j++)
                    {
                        req.msg_list.Add(records[j]);
                        k++;
                        if (req.msg_list.Count >= onceCount)
                            break;
                    }
                    Debuger.Log($"[kill_record.request] 第{i+1}次上报个数: {req.msg_list.Count}");
                    
                },
                (data) =>
                {
                    num--;
                    Debuger.Log($"[S2C] [上报回调] kill_record");
                });
            await UniTask.Yield();
        }
        
        if (num > 0)
        {
            var waitNumTask = UniTask.WaitUntil(() => num <= 0);
            var wait3SecondsTask = UniTask.Delay(TimeSpan.FromSeconds(3));
            await UniTask.WhenAny(waitNumTask, wait3SecondsTask);
        }
        EventMgr.Dispatch(BattleEvent.Battle_S2C_RetKillRecord);
    }

    /// <summary>
    /// 上报游戏胜负结果
    /// </summary>
    public void ReqGameResult()
    {
        NetMgr.Instance.SendTo<game_result.request, C2SProtocol.game_result>(
            (req) => { req.result = LevelGlobal.Instance.IsRedWin ?CampType.红.ToInt() : CampType.蓝.ToInt(); },
            S2C_RetGameResult);
    }

    /// <summary>
    /// 服务器返回游戏胜负结果
    /// </summary>
    private void S2C_RetGameResult(SprotoTypeBase data)
    {
        var response = data as game_result.response;
        if (response.error_code == 100000)
        {
            RankModel.Instance.SetDelay(Mathf.Max(response.family_rank_t, response.world_rank_t));
            PlayerModel.Instance.S2C_RetGameResultWinCombo(response.win_comboes);
        }
        EventMgr.Dispatch(BattleEvent.Battle_S2C_RetGameResult);
        // Debuger.Log($"[S2C] [胜负消息] game_result");
        response.LogMessageMembers("game_result.response");
    }
}
