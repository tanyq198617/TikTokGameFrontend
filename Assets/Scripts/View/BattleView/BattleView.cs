using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[UIBind(UIPanelName.BattleView)]
public class BattleView : APanelBase
{
    public static BattleView Instance { get { return Singleton<BattleView>.Instance; } }

    public BattleView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.One;
    }    

    private BattleRightPage rightPage;
    private BattleBottomRightPage bottomRightPage;
    private BattleCenterPage centerPage;
    private BattleTopPage topPage;
    private BattleBottomPage bottomPage;

    public override void Init()
    {
        base.Init();

        rightPage = UIUtility.CreatePageNoClone<BattleRightPage>(Trans, "Right");
        bottomRightPage = UIUtility.CreatePageNoClone<BattleBottomRightPage>(Trans, "BottomRight");
        topPage = UIUtility.CreatePageNoClone<BattleTopPage>(Trans, "Top");
        bottomPage = UIUtility.CreatePageNoClone<BattleBottomPage>(Trans, "Bottom");
        centerPage = UIUtility.CreatePageNoClone<BattleCenterPage>(Trans, "Center");
    }
    
    public override void Refresh()
    {
        base.Refresh();
        rightPage.IsActive = true;
        bottomRightPage.IsActive = true;
        topPage.IsActive = true;
        bottomPage.IsActive = true;
        centerPage.IsActive = true;
    }

    public override void CloseUI()
    {
        base.CloseUI();
        rightPage.IsActive = false;
        bottomRightPage.IsActive = false;
        topPage.IsActive = false;
        bottomPage.IsActive = false;
        centerPage.IsActive = false;
    }

    /// <summary>
    /// 场上弹夹数量变化
    /// </summary>
    /// <param name="camp"></param> 阵营
    /// <param name="ballType"></param>子弹类型,1小球,2大球(读ball表里面的ballBigOrSmalltype字段)
    /// <param name="ballCount"></param>剩余为生成弹夹数量
    public void BallValueChanged(int camp,int ballType,int ballCount)
    {
        
        switch ((CampType)camp)
        {
            case CampType.红:
                bottomPage.RefreshQiuCount(ballType,ballCount);
                break;
            case CampType.蓝:
                topPage.RefreshQiuCount(ballType,ballCount);
                break;
        }
    }
    
    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener<int, int, int>(BattleEvent.Battle_Ball_ValueChanged, BallValueChanged);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener<int, int, int>(BattleEvent.Battle_Ball_ValueChanged, BallValueChanged);
    }
}
