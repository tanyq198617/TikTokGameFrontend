using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HotUpdateScripts;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[UIBind(UIPanelName.MapView)]
public class MapView : APanelBase
{
    public static MapView Instance
    {
        get { return Singleton<MapView>.Instance; }
    }

    private UILayoutGroup<AMapItem, TMapData> mapItemGroup;

    public MapView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.One;
    }

    private List<TMapData> list;

    public override void Init()
    {
        base.Init();
        mapItemGroup = new UILayoutGroup<AMapItem, TMapData>();
        mapItemGroup.OnInit(UIUtility.Control("Content",m_gameobj));
    }

    public override void Refresh()
    {
        list ??= TMapDataManager.Instance.GetAllItem().ToList();
        mapItemGroup.DefaultRefresh(list);
    }
}