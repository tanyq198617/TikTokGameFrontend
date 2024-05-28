using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankWinningStreakTwoPage : AItemPageBase
{

    private TextMeshProUGUI tx_name;
    private TextMeshProUGUI tx_winningStreakCount;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        tx_name = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_name");
        tx_winningStreakCount = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_winningStreakCount");

    }

    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);


    }
}
