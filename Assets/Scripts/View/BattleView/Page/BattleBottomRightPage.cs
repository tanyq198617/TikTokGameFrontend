using System;
using System.Collections;
using System.Collections.Generic;
using HotUpdateScripts;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 目前只显示礼物对应的刷兵数据,不做逻辑处理
/// </summary>
public class BattleBottomRightPage : AItemPageBase
{
    private Image image_tip;
    private List<string> sptiteNames;
    /// <summary>刷新图集的名字下标/// </summary>
    private int spriteNameIndex;
    
    private TickedBase _tick;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        image_tip = UIUtility.GetComponent<Image>(RectTrans,"image_tip");
    }
    public override void Refresh()
    {
        base.Refresh();
       
        spriteNameIndex = 0;
        sptiteNames = TGlobalDataManager.GiftExplainSpriteName;
        _tick ??=TickedBase.Create(TGlobalDataManager.GiftExplainTime, SetTipImage, false, true);
        _tick.Reset();
    }
    
    /// <summary> 设置提示图片的显示 /// </summary>
    private void SetTipImage()
    {
        spriteNameIndex++;
        if (spriteNameIndex >= sptiteNames.Count)
            spriteNameIndex = 0;
        UIUtility.Safe_UGUI(ref image_tip,SpriteMgr.Instance.LoadSpriteFromMainView(sptiteNames[spriteNameIndex]));
        UIUtility.SetNativeSize(ref image_tip);
    }
    
    public override void Close()
    {
        base.Close();
        if (_tick != null)
        {
            TickedMgr.Instance.Remove(_tick);
            _tick = null;
        }
    }
}
