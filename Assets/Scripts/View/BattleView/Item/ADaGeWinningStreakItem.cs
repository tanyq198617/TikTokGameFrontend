using TMPro;
using UnityEngine;

/// <summary>
/// 红蓝方三个大哥连胜显示
/// </summary>
public class ADaGeWinningStreakItem : AItemPageBase
{
    private TextMeshProUGUI tx_winningstreak;
    private WorldPointToUIPointComponent worldToUiPointComponent;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        tx_winningstreak = UIUtility.GetComponent<TextMeshProUGUI>(m_gameobj, "tx_winningstreak");
       
    }

    public override void Close()
    {
        
    }
    
}