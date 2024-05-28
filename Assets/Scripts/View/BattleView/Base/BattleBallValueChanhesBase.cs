using TMPro;
using UnityEngine;

public class BattleBallValueChanhesBase : AItemPageBase
{
    private TextMeshProUGUI tx_smallqiuText;
    private TextMeshProUGUI tx_bigqiuText;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        tx_smallqiuText = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_smallqiuText");
        tx_bigqiuText = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_bigqiuText");
    }

    public override void Refresh()
    {
        base.Refresh();
        UIUtility.Safe_UGUI(ref tx_smallqiuText, $"小球:{0}");
        UIUtility.Safe_UGUI(ref tx_bigqiuText, $"小精灵:{0}");
    }

    public void RefreshQiuCount(int qiuType, int qiuCount)
    {
        switch (qiuType)
        {
            case 1:
                UIUtility.Safe_UGUI(ref tx_smallqiuText, $"小球:{qiuCount}");
                break;
            case 2:
                UIUtility.Safe_UGUI(ref tx_bigqiuText, $"小精灵:{qiuCount}");
                break;
            default:
                break;
        }
    }
}