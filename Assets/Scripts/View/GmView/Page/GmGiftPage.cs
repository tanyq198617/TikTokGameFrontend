using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GmGiftPage : AItemPageBase
{
    private TMP_InputField giftIdField;
    private TMP_InputField giftNumInputField;
    private GameObject clickBtn;
    private GameObject closeBtn;
    private GameObject closeGmButton;
    private UILayoutGroup<GmPlayerInfoItem, PlayerInfo> gmPlayerItemGroup;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        giftIdField = UIUtility.GetComponent<TMP_InputField>(RectTrans, "giftIdInputField");
        giftNumInputField = UIUtility.GetComponent<TMP_InputField>(RectTrans, "giftNumInputField");
        clickBtn = UIUtility.BindClickEvent(RectTrans, "Button", OnClick);
        closeBtn = UIUtility.BindClickEvent(RectTrans, "closeButton", OnClick);
        closeGmButton = UIUtility.BindClickEvent(RectTrans, "closeGmButton", OnClick);
        gmPlayerItemGroup = new UILayoutGroup<GmPlayerInfoItem, PlayerInfo>();
        gmPlayerItemGroup.OnInit(UIUtility.Control("Content",m_gameobj));
    }

    public override void Refresh()
    {
        base.Refresh();
        /*List<PlayerInfo> infos = PlayerModel.Instance.GetCampPlayers(SimulateModel.Instance.CampType.ToInt());
        gmPlayerItemGroup.DefaultRefresh(infos);*/
    }

    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);
        if (obj == clickBtn)
        {
            string giftId = giftIdField.text;
            long giftNUm = giftNumInputField.text.ToInt();
            int camp = SimulateModel.Instance.CampType.ToInt();
            var player = PlayerModel.Instance.RandomPlayer(camp);
            SimulateModel.Instance.Simulate_Send_douyin_gift(player, giftId, giftNUm);
        }else if (obj == closeBtn)
        {
            IsActive = false;
        }
        else if (obj == closeGmButton)
        {
            UIMgr.Instance.CloseUI(UIPanelName.GmView);
        }
    }
}