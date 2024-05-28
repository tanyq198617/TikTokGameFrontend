using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AWinningStreakRankItem : ALayoutItem
{
    private TextMeshProUGUI tx_rank;
    private TextMeshProUGUI tx_name;
    private TextMeshProUGUI tx_winningStreakNum;
    private TextMeshProUGUI tx_rank2;

    private RawImage playerIcon;
    private GameObject object_up;
    private GameObject object_dn;
    private GameObject object_blue;
    private GameObject object_orange;
    private GameObject object_add;
    private GameObject object_jian;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
    }

    public override void Refresh<T>(T data)
    {
        RankInfo info = data as RankInfo;
        if (info == null)
            return;
    }
}