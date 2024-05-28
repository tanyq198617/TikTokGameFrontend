using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleCenterItemBase : AItemPageBase
{
    protected TextMeshProUGUI tx_winningStreak;
    protected TextMeshProUGUI tx_hp;

    protected GameObject object_winningStreak;
    protected WorldPointToUIPointComponent pointComponent;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        tx_winningStreak = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_winningStreak");
        tx_hp = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_hp");
        object_winningStreak = UIUtility.Control("winningStreak", m_gameobj);
        pointComponent = object_winningStreak.GetOrAddComponent<WorldPointToUIPointComponent>();
    }

    protected virtual float GetWinningStreakScore()
    {
        return 0;
    }
    
    protected virtual List<int> GetThreeYuanZhongDataList()
    {
        return new List<int>();
    }

    /// <summary>
    /// 刷新红蓝方的连胜池
    /// </summary>
    public void RestWinningStreakScore()
    {
        float score = GetWinningStreakScore();
        UIUtility.Safe_UGUI(ref tx_winningStreak, $"(连胜池{score})");
    }

    public virtual void SetBossHP(int hp)
    {
        UIUtility.Safe_UGUI(ref tx_hp, hp);
    }

    /// <summary>
    /// 连胜池上方三个大冤种的连胜ui刷新方法
    /// </summary>
    public void RestThreeYuanZhongUI()
    {
        List<int> datas = GetThreeYuanZhongDataList();
    }
}