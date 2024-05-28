using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[UIBind(UIPanelName.MainBottomView)]
public class MainBottomView : APanelBase
{
    public static MainBottomView Instance { get { return Singleton<MainBottomView>.Instance; } }

    public MainBottomView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.One;
    }

    private MainShangchengPage shangchengPage;
    private MainJueSePage juesePage;
    private MainZhanDouPage zhandouPage;
    private MainWuXuePage wuxuePage;
    private MainMenPaiPage menpaiPage;

    private AMainBottomBase select;

    private Dictionary<int, AMainBottomBase> panelDict;
    private Dictionary<string, int> indexDict;
    private Dictionary<int, string> panelNameDict;

    public override void Init()
    {
        base.Init();

        shangchengPage = UIUtility.CreateItemNoClone<MainShangchengPage>(Trans, "btn_shop");
        juesePage = UIUtility.CreateItemNoClone<MainJueSePage>(Trans, "btn_role");
        zhandouPage = UIUtility.CreateItemNoClone<MainZhanDouPage>(Trans, "btn_fight");
        wuxuePage = UIUtility.CreateItemNoClone<MainWuXuePage>(Trans, "btn_skill");
        menpaiPage = UIUtility.CreateItemNoClone<MainMenPaiPage>(Trans, "btn_union");

        shangchengPage.OnItemClick = OnItemClick;
        juesePage.OnItemClick = OnItemClick;
        zhandouPage.OnItemClick = OnItemClick;
        wuxuePage.OnItemClick = OnItemClick;
        menpaiPage.OnItemClick = OnItemClick;

        indexDict = new Dictionary<string, int>();
        panelNameDict = new Dictionary<int, string>
            {
                {shangchengPage.GetIndex(), UIPanelName.ShopView },
                {juesePage.GetIndex(), UIPanelName.RoleView },
                {zhandouPage.GetIndex(), UIPanelName.FightView },
                {wuxuePage.GetIndex(), UIPanelName.SkillView },
                {menpaiPage.GetIndex(), UIPanelName.UnionView },
            };

        panelDict = new Dictionary<int, AMainBottomBase>()
            {
                //{shangchengPage.GetIndex(), shangchengPage },
                {juesePage.GetIndex(), juesePage },
                {zhandouPage.GetIndex(), zhandouPage },
                //{wuxuePage.GetIndex(), wuxuePage },
            };

        foreach (var item in panelNameDict)
            indexDict[item.Value] = item.Key;
    }

    public void OpenPage(int index, Action callback = null)
    {
        if (!IsOpen)
        {
            UIMgr.Instance.ShowUI(m_strPanelViewName, openCall: (p) =>
            {
                OpenIndex(index);
                callback?.Invoke();
            });
        }
        else
        {
            OpenIndex(index);
            callback?.Invoke();
        }
    }

    private void OpenIndex(int index)
    {
        if (!panelDict.ContainsKey(index))
            return;
        shangchengPage.IsActive = index == 0;
        juesePage.IsActive = index == 1;
        zhandouPage.IsActive = index == 2;
        wuxuePage.IsActive = index == 3;
        menpaiPage.IsActive = index == 4;
        select = panelDict[index];

        //shangchengPage.RefreshRedPt();
    }

    private void OnItemClick(int index)
    {
        OpenIndex(index);
    }

    public override void CloseUI()
    {
        base.CloseUI();
        select?.NoSelect();
    }

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener<string>(UIEvent.UI_Panel_Open, OnPanelOpen);
        //EventMgr.AddEventListener(UIEvent.UI_RedPoint_ValueChanged, this, "OnRedPoint");
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener<string>(UIEvent.UI_Panel_Open, OnPanelOpen);
        //EventMgr.RemoveEventListener(UIEvent.UI_RedPoint_ValueChanged, this, "OnRedPoint");
    }

    private void OnPanelOpen(string panelName)
    {
    }

    private void OnRedPoint()
    {
        //shangchengPage?.RefreshRedPt();
    }
}
