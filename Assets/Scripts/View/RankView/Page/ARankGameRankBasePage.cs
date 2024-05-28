using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 排行榜前三的Item
/// </summary>
public class ARankGameRankBasePage : AItemPageBase
{
    private TextMeshProUGUI tx_winningStreakText;
    private TextMeshProUGUI tx_wonComboChange;
    private TextMeshProUGUI tx_bluename;
    private TextMeshProUGUI tx_bluekillNum;
    private TextMeshProUGUI tx_blueworldRank;
    private TextMeshProUGUI tx_orangename;
    private TextMeshProUGUI tx_orangekillNum;
    private TextMeshProUGUI tx_orangeworldRank;
    private TextMeshProUGUI tx_getScore;

    private RawImage playerIcon;
    private GameObject object_blueInfo;
    private GameObject object_orangeInfo;

    private GameObject object_add;
    private GameObject object_jian;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        tx_winningStreakText = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_winningStreakText");
        tx_wonComboChange = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_wonComboChange");
        tx_bluename = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_bluename");
        tx_bluekillNum = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_bluekillNum");
        tx_blueworldRank = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_blueworldRank");
        tx_orangename = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_orangename");
        tx_orangekillNum = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_orangekillNum");
        tx_orangeworldRank = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_orangeworldRank");
        tx_getScore = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_getScore");
        playerIcon = UIUtility.GetComponent<RawImage>(RectTrans, "playerIcon");

        object_blueInfo = UIUtility.Control("object_blueInfo", m_gameobj);
        object_orangeInfo = UIUtility.Control("object_orangeInfo", m_gameobj);
        object_add = UIUtility.Control("object_add", m_gameobj);
        object_jian = UIUtility.Control("object_jian", m_gameobj);
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

        long wonComboChangeChange = info.info.win_combo - info.player.win_combo;
        bool isAddChange = wonComboChangeChange >= 0;
        string wonComBoChangeStr = isAddChange ? "+" : "";
        UIUtility.Safe_UGUI(ref tx_wonComboChange,
            $"{wonComBoChangeStr}{wonComboChangeChange.Number4Chinese()}连胜");
        object_add.SetActiveEX(isAddChange);
        object_jian.SetActiveEX(!isAddChange);

        UIUtility.Safe_UGUI(ref tx_getScore, $"【瓜分积分池:{info.player.poolScore.Number4Chinese()}】");

        bool isRedCamp = info.player.campType == CampType.红;
        object_orangeInfo.SetActiveEX(isRedCamp);
        object_blueInfo.SetActiveEX(!isRedCamp);

        TextureMgr.Instance.Set(ref playerIcon, info.info.avatar_url);
        if (isRedCamp)
        {
            UIUtility.Safe_UGUI(ref tx_orangename, info.info.nickname);
            UIUtility.Safe_UGUI(ref tx_orangekillNum, $"积分{(long.Parse(info.score)).ToMoney()}");
            UIUtility.Safe_UGUI(ref tx_orangeworldRank, $"世界排名{info.info.rank_end}");
        }
        else
        {
            UIUtility.Safe_UGUI(ref tx_bluename, info.info.nickname);
            UIUtility.Safe_UGUI(ref tx_bluekillNum, $"积分{(long.Parse(info.score)).ToMoney()}");
            UIUtility.Safe_UGUI(ref tx_blueworldRank, $"世界排名{info.info.rank_end}");
        }
    }

    /// <summary>
    /// 当排行榜无数据时的状态
    /// </summary>
    private void SetInfoIsNullState()
    {
        IsActive = false;
    }
}