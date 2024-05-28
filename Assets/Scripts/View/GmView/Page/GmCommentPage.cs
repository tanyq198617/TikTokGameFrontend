
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GmCommentPage : AItemPageBase
{
    private TMP_InputField commentInputField;
    private TMP_InputField winningInputField;
    private GameObject clickBtn;
    private GameObject closeBtn;
    private GameObject closeGmButton;
    private GameObject winningButton;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        commentInputField = UIUtility.GetComponent<TMP_InputField>(RectTrans, "commentInputField");
        winningInputField = UIUtility.GetComponent<TMP_InputField>(RectTrans, "winningInputField");
        clickBtn = UIUtility.BindClickEvent(RectTrans, "Button", OnClick);
        winningButton = UIUtility.BindClickEvent(RectTrans, "winningButton", OnClick);
        closeBtn = UIUtility.BindClickEvent(RectTrans, "closeButton", OnClick);
        closeGmButton = UIUtility.BindClickEvent(RectTrans, "closeGmButton", OnClick);
    }
    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);
        if (obj == clickBtn)
        {
            string comment = commentInputField.text;
            if (comment == "")
                return;
            int camp = SimulateModel.Instance.CampType.ToInt();
            var player = PlayerModel.Instance.RandomPlayer(camp);
            if (player == null)
                return;
            if(player.isFake)
                SimulateModel.Instance.Simulate_Send_douyin_comment(player, comment);
            else
                NetMgr.GetHandler<GmHandler>().Req_douyin_content(player.openid, comment);
        }
        else if (obj == winningButton)
        {
            var winningCount = winningInputField.text;
            if (winningCount == "") return;
            int camp1 = SimulateModel.Instance.CampType.ToInt();
            var player = PlayerModel.Instance.RandomPlayer(camp1);
            if (player == null)
                return;
            if (!player.isFake)
                NetMgr.GetHandler<GmHandler>().Req_douyin_win_combo(player.openid, winningCount);
        }
        else if (obj == closeBtn)
        {
            IsActive = false;
        }
        else if (obj == closeGmButton)
        {
            UIMgr.Instance.CloseUI(UIPanelName.GmView);
        }
    }
}
