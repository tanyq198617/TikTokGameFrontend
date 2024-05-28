using System.Net.NetworkInformation;
using HotUpdateScripts;
using TMPro;
using UnityEngine;

public class GiftSideCenterPage : AItemPageBase
{
    private GiftCenterPage giftCenterPage;
    private SideCenterPage sideCenterPage;
    private BigGiftCenterPage bigGiftCenterPage;
    private DaGeRuChangPage daGeRuChangPage;
    private LikePage likePage;
    private WinningPointPage winningPointPage;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        giftCenterPage = UIUtility.CreatePageNoClone<GiftCenterPage>(Trans, "giftPage");
        sideCenterPage = UIUtility.CreatePageNoClone<SideCenterPage>(Trans, "sidePage");
        bigGiftCenterPage = UIUtility.CreatePageNoClone<BigGiftCenterPage>(Trans, "bigGiftPage");
        daGeRuChangPage = UIUtility.CreatePageNoClone<DaGeRuChangPage>(Trans, "daGeRuChangPage");
        likePage = UIUtility.CreatePageNoClone<LikePage>(Trans, "likePage");
        winningPointPage= UIUtility.CreatePageNoClone<WinningPointPage>(Trans, "page_winningPoint");
    }

    public override void Refresh()
    {
        base.Refresh();
        giftCenterPage.IsActive = true;
        sideCenterPage.IsActive = true;
        bigGiftCenterPage.IsActive = true;
        daGeRuChangPage.IsActive = true;
        likePage.IsActive = true;
        winningPointPage.IsActive = true;
    }

    public override void Close()
    {
        base.Close();
        giftCenterPage.IsActive = false;
        sideCenterPage.IsActive = false;
        bigGiftCenterPage.IsActive = false;
        daGeRuChangPage.IsActive = false;
        likePage.IsActive = false;
        winningPointPage.IsActive = false;
    }

    private void RefreshSideTip(PlayerInfo info)
    {
        sideCenterPage.RefreshSideTip(info);
        daGeRuChangPage.RefreshDaGeTip(info);
    }

    private void RefreshGiftTip(string openId, string giftId, long giftCount)
    {
        TOperateData data = TOperateDataManager.Instance.GetItem(int.Parse(giftId));
        switch (data.tanChuangType)
        {
            case 1:
                giftCenterPage.RefreshSideTip(openId, giftId, giftCount);
                break;
            case 2:
                bigGiftCenterPage.RefreshSideTip(openId, giftId, giftCount);
                break;
            case 3:
                giftCenterPage.RefreshSideTip(openId, giftId, giftCount);
                bigGiftCenterPage.RefreshSideTip(openId, giftId, giftCount);
                break;
            default:
                break;
        }
    }

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener<string, string, long>(UIEvent.GiftSideView_gift_tip, RefreshGiftTip);
        EventMgr.AddEventListener<PlayerInfo>(UIEvent.GiftSideView_side_tip, RefreshSideTip);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener<string, string, long>(UIEvent.GiftSideView_gift_tip, RefreshGiftTip);
        EventMgr.RemoveEventListener<PlayerInfo>(UIEvent.GiftSideView_side_tip, RefreshSideTip);
    }
}