using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ARankWorldRankBasePage : AItemPageBase
{
    private TextMeshProUGUI tx_name;
    private TextMeshProUGUI tx_killNum;
    private TextMeshProUGUI tx_worldRank;
    private TextMeshProUGUI tx_winningStreakText;
    private RawImage playerIcon;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        tx_name = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_name");
        tx_killNum = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_killNum");
        tx_worldRank = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_worldRank");
        tx_winningStreakText = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_winningStreakText");
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
        UIUtility.Safe_UGUI(ref tx_winningStreakText, $"{info.info.win_combo.Number4Chinese()}连胜");
        UIUtility.Safe_UGUI(ref tx_worldRank, $"世界排名{info.info.rank_end}");
        UIUtility.Safe_UGUI(ref tx_killNum, $"积分{long.Parse(info.score).Number4Chinese()}");
        UIUtility.Safe_UGUI(ref tx_name, info.info.nickname);
        TextureMgr.Instance.Set(ref playerIcon, info.info.avatar_url);
    }

    /// <summary>
    /// 当排行榜无数据时的状态
    /// </summary>
    private void SetInfoIsNullState()
    {
        IsActive = false;
    }
}