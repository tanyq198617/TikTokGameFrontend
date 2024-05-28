using System;
using System.Collections;
using System.Collections.Generic;
using GameNetwork;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[UIBind(UIPanelName.GmView)]
public class GmView : APanelBase
{
    public static GmView Instance { get { return Singleton<GmView>.Instance; } }

    public GmView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.One;
    }
    private GameObject itemBtn;
    private GameObject closeBtn;
    private Transform itemParent;
    
    private GmGiftPage _gmGiftPage;
    private GmCommentPage commentPage;
    public override void Init()
    {
        base.Init();
        
        _gmGiftPage = UIUtility.CreatePageNoClone<GmGiftPage>(Trans, "giftNumPage");
        commentPage =  UIUtility.CreatePageNoClone<GmCommentPage>(Trans, "page_comment");
        itemBtn = UIUtility.Control("itemButton",m_gameobj);
        itemParent = UIUtility.Control("Content",m_gameobj.transform);
        
        closeBtn = UIUtility.BindClickEvent(Trans, "closeButton", OnClick);
        
        int gmCount = gmDic.Count;
        for (int i = 0; i < gmCount; i++)
        {
            GameObject item = GameObject.Instantiate(itemBtn, itemParent);
            item.SetActiveEX(true);
            
            int index = i;
            UIUtility.GetComponent<TextMeshProUGUI>(item, "Text (TMP)").text = gmDic[i];
            EventTriggerListener.Get(item).onClick.AddListener(delegate(GameObject o, PointerEventData ev)
            {
                OnGmClick(index);
            });
        }
    }

    public override void Refresh()
    {
        base.Refresh();
        _gmGiftPage.IsActive = false;
        commentPage.IsActive = false;
        // Debuger.LogError("芜湖");
    }

    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);

        if (obj.Equals(closeBtn))
        {
            UIMgr.Instance.CloseUI(UIPanelName.GmView);
           
        }
    }
    
    private Dictionary<int, string> gmDic = new Dictionary<int, string>()
    {
        [0] = "蓝方获胜",
        [1] = "红方获胜",
        [2] = "打开礼物命令",
        [3] = "测试评论",
        [4] = "大哥入场",
        [5] = "二哥入场",
        [6] = "三哥入场",
        [7] = "测试蓝方神龙炸弹",
        [8] = "c2s_设置房间号",
        [9] = "c2s_生成新玩家",
        [10] = "c2s_榜一大哥刷礼物",
    };
    
    public void OnGmClick(int index)
    {
        switch (index)
        {
            case 0:
                EventMgr.Dispatch(BattleEvent.Battle_GameIsOver, CampType.红);
                break;
            case 1:
                EventMgr.Dispatch(BattleEvent.Battle_GameIsOver, CampType.蓝);
                break;
            case 2:
                _gmGiftPage.IsActive = true;
                break;
            case 3:
                commentPage.IsActive = true;
                /*int _camp3 = SimulateModel.Instance.CampType.ToInt();
                var _player3 = PlayerModel.Instance.RandomPlayer(_camp3);
                //FairyDragonBallFactory.GetOrCreate(_player3);
                SimulateModel.Instance.Simulate_Send_douyin_comment(_player3,"召唤甜甜圈");*/
                break;
            case 4:
                int _camp4 = SimulateModel.Instance.CampType.ToInt();
                var _player4 = PlayerModel.Instance.RandomPlayer(_camp4);
                _player4.world_rank = 2;
                EventMgr.Dispatch(UIEvent.GiftSideView_side_tip, _player4);
                break;
            case 5:
                int _camp = SimulateModel.Instance.CampType.ToInt();
                var _player = PlayerModel.Instance.RandomPlayer(_camp);
                _player.world_rank = 20;
                EventMgr.Dispatch(UIEvent.GiftSideView_side_tip, _player);
                break;
            case 6:
                int _camp6 = SimulateModel.Instance.CampType.ToInt();
                var _player6 = PlayerModel.Instance.RandomPlayer(_camp6);
                _player6.world_rank = 40;
                EventMgr.Dispatch(UIEvent.GiftSideView_side_tip, _player6); 
                break;
            case 7:
                int _camp7 =  CampType.蓝.ToInt();
                var _player7 = PlayerModel.Instance.RandomPlayer(_camp7);
                FairyDragonBallFactory.GetOrCreate(_player7);
               // FairyDragonBallFactory.GetOrCreate(false,null);
                break;
            case 8:
                NetMgr.GetHandler<GmHandler>().ReqSetRoomId();
                break;
            case 9:
                NetMgr.GetHandler<GmHandler>().Req_douyin_new_user();
                break;
            case 10:
                NetMgr.GetHandler<GmHandler>().Req_douyin_top_acc();
                break;
            default:
                Debuger.Log("该gm按钮没有实现方法");
                break;
        }
    }
}
