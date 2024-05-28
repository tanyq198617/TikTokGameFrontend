using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleRightPage : AItemPageBase
{
    private TextMeshProUGUI tx_scoreText;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        tx_scoreText = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_scoreText");
    }

    public override void Refresh()
    {
        base.Refresh();
        UIUtility.Safe_UGUI(ref tx_scoreText, PlayerModel.Instance.totalScore.Number4Chinese());
    }

    /// <summary>
    /// 刷新积分池显示数据
    /// </summary>
    private void RestScoreText(long score)
    {
        UIUtility.Safe_UGUI(ref tx_scoreText,score.Number4Chinese());
    }


    #region 注册,释放Event

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener<long>(UIEvent.BattleView_RestScore, RestScoreText);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener<long>(UIEvent.BattleView_RestScore, RestScoreText);
    }

    #endregion
}