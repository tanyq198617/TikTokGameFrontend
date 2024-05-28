using System.Collections;
using System.Collections.Generic;
using HotUpdateScripts;
using UnityEngine;

/// <summary>
/// 额外赠送
/// </summary>
public class GiftExtraMgr
{
    /// <summary> key=openid value={ key=extraid, value=送了几次 } </summary>
    private static readonly Dictionary<string, Dictionary<int, long>> extraDict = new Dictionary<string, Dictionary<int, long>>();
    
    public static void OnExtra(PlayerInfo info, string gift_id, long total)
    {
        //pretotal 上次礼物总数
        //total 当前礼物总数
        var list = TOperateExtraManager.Instance.GetExtras(gift_id);
        if (list == null || list.Count <= 0)
            return;

        if (!extraDict.TryGetValue(info.openid, out var dict))
            extraDict.Add(info.openid,  dict = new Dictionary<int, long>());
        
        for (int i = 0; i < list.Count; i++)
        {
            var item = list[i];

            if (!dict.TryGetValue(item.Id, out var value))
                dict.Add(item.Id, value = 0);

            //增加的个数
            var add = total - value * item.compare;

            var count = add / item.compare;

            if (count > 0)
            {
                dict[item.Id] += count; 
                ShotFactory.OnShotBall(info, item.ballid, item.number * count);
                Debuger.Log($"[{info.nickname}({info.openid})] 触发额外赠送：赠送球球Type={item.ballid} 赠送球球数量={item.number * count}, 上次赠送数量={value}, 增加赠送数量={count}, 当前礼物总数量={{total}}"); 
            }
        }
    }

    public static void Clear()
    {
        extraDict.Clear();
    }
}
