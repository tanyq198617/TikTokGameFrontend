using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankWinningStreakOnePage : AItemPageBase
{
    private TextMeshProUGUI tx_name;
    private TextMeshProUGUI tx_winningStreakCount;
    private RawImage playerIcon;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        tx_name = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_name");
        tx_winningStreakCount = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_winningStreakCount");
        playerIcon = UIUtility.GetComponent<RawImage>(RectTrans, "playerIcon");
    }


    public void RefreshContent(RankInfo info)
    {
        if (info is null)
            SetInfoIsNullState();
        else
            SetInfoState(info);
    }

    /// <summary>
    /// 当有排行榜数据时的显示
    /// </summary>
    private void SetInfoState(RankInfo info)
    {
        IsActive = true;
        UIUtility.Safe_UGUI(ref tx_winningStreakCount, $"连胜数:{info.info.win_combo.Number4Chinese()}");

        TextureMgr.Instance.Set(ref playerIcon, info.info.avatar_url);
        UIUtility.Safe_UGUI(ref tx_name, info.info.nickname);
    }

    /// <summary>
    /// 当排行榜无数据时的状态
    /// </summary>
    private void SetInfoIsNullState()
    {
        IsActive = false;
    }
}