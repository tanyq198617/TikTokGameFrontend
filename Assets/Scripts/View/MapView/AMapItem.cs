using HotUpdateScripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AMapItem : ALayoutItem
{
    private TMapData _mapData;
    private Image mapImage;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
       // tx_mapName = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_mapName");
       mapImage = UIUtility.GetComponent<Image>(RectTrans, "mapImage");
        UIUtility.BindClickEvent(m_gameobj, OnClick);
    }

    public override void Refresh<T>(T data)
    {
        TMapData info = data as TMapData;
        _mapData = info;
        if (info == null)
            return;
        string imageName = $"dt_btn_0{info.Id}";
        UIUtility.Safe_UGUI(ref mapImage, SpriteMgr.Instance.LoadSpriteFromMapSelection(imageName));
    }

    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);
        
        if (obj.Equals(m_gameobj))
        {
            if (_mapData.IsNull())
                Debuger.LogError("地图数据为空");
            else
            {
                EventMgr.Dispatch(BattleEvent.Battle_Map_Selected, _mapData.Id);
                UIMgr.Instance.CloseUI(UIPanelName.MapView);
            }
        }
    }

    public override void Clear()
    {
        _mapData = null;
    }
}