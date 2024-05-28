using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GmPlayerInfoItem : ALayoutItem
{
    private TextMeshProUGUI tx_playerName;
    private GameObject gameObject_click;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        tx_playerName = UIUtility.GetComponent<TextMeshProUGUI>(m_gameobj, "tx_playerName");
        gameObject_click = UIUtility.BindClickEvent(RectTrans, "btn_bj", OnClick);
    }

    public override void Refresh<T>(T data)
    {
        base.Refresh(data);
        var playerInfo = data as PlayerInfo;
        UIUtility.Safe_UGUI(ref tx_playerName,playerInfo.nickname);
    }

    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);
        if (obj == m_gameobj)
        {
            Debuger.LogError("受到点击");
        }
    }
}