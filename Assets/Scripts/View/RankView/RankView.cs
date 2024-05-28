using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 排行榜目前只留当前排行榜,世界排行榜(界面叫每周排行榜)
/// </summary>
[UIBind(UIPanelName.RankView)]
public class RankView : APanelBase
{
    public static RankView Instance { get { return Singleton<RankView>.Instance; } }

    public RankView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.One;
    }    

    private RankGameRankPage gameRankPage;
    private RankWorldRankRootPage worldRankRootPage;

    private RankGameBtnPage gameBtnPage;
    private RankWorldBtnPage worldBtnPage;
   
    
    private GameObject reStart;
    private int selectId;

    private GameObject object_gameRankTitle;
    private GameObject object_worldRankTitle;
    private AudioTask audioTask;
    public override void Init()
    {
        base.Init();

        gameRankPage = UIUtility.CreatePageNoClone<RankGameRankPage>(Trans, "page_gameRank");
        worldRankRootPage = UIUtility.CreatePageNoClone<RankWorldRankRootPage>(Trans, "page_worldRankRoot");
        
        gameBtnPage = UIUtility.CreateItemNoClone<RankGameBtnPage>(Trans, "game");
        worldBtnPage = UIUtility.CreateItemNoClone<RankWorldBtnPage>(Trans, "world");

        object_gameRankTitle = UIUtility.Control("object_gameRankTitle", m_gameobj);
        object_worldRankTitle = UIUtility.Control("object_worldRankTitle", m_gameobj);
        
        gameBtnPage.OnItemClick = OnItemClick;
        worldBtnPage.OnItemClick = OnItemClick;
        
        reStart = UIUtility.BindClickEvent(Trans, "reStart", Restart);
        selectId = 0;
    }

    public override void Refresh()
    {
        base.Refresh();
        selectId = 0;
        OpenIndex(gameBtnPage.GetIndex());
        audioTask = AudioMgr.Instance.PlayBackSoundName(18,true);
    }


    private void Restart(GameObject obj, PointerEventData eventData)
    {
        // Debuger.LogError("重新开始按钮");
        EventMgr.Dispatch(BattleEvent.Battle_RestartGame);
    }
    
    private void OnItemClick(int index)
    {
        OpenIndex(index);
    }
    
    private void OpenIndex(int index)
    {
        if (selectId == index)
            return;
        bool isGameBtn = index == 1;
        gameRankPage.IsActive = isGameBtn;
        gameBtnPage.IsActive = isGameBtn;
        
        bool isWorldBtn = index == 2;
        worldBtnPage.IsActive = isWorldBtn;
        worldRankRootPage.IsActive = isWorldBtn;

        object_gameRankTitle.SetActiveEX(isGameBtn);
        object_worldRankTitle.SetActiveEX(isWorldBtn);
        
        
        selectId = index;

        if (index < 4)
            NetMgr.GetHandler<RankHandler>().ReqRank(GetRankType(index));
    }

    public override void CloseUI()
    {
        base.CloseUI();
        selectId = -1;
        if (audioTask != null)
        {
            audioTask.IsEnd = true;
            audioTask = null;
        }
    }

    private ENUM_RANK_TYPE GetRankType(int index)
    {
        return index switch
        {
            1 => ENUM_RANK_TYPE.Game,
            2 => ENUM_RANK_TYPE.World,
            3 => ENUM_RANK_TYPE.winningStreak,
            _ => ENUM_RANK_TYPE.Game
        };
    }
}
