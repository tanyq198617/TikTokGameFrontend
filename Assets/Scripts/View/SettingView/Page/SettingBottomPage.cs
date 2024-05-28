using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingBottomPage : AItemPageBase
{

    private GameObject btn_beishu;
    private GameObject object_yiBeiShu;
    private GameObject object_erBeiShu;
    private Slider soundVolume;
    private bool isYiBeiShu = false;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        btn_beishu = UIUtility.BindClickEvent(RectTrans, "btn_beishu", OnClick);
        object_yiBeiShu = UIUtility.Control("object_yiBeiShu", m_gameobj);
        object_erBeiShu = UIUtility.Control("object_erBeiShu", m_gameobj);
        soundVolume = UIUtility.GetComponent<Slider>(m_gameobj, "slider_volume");
        UIUtility.BindValueChanged(ref soundVolume, SoundVolumeChanged);
    }

    public override void Refresh()
    {
        base.Refresh();
        soundVolume.value = AudioMgr.Instance.GetSoundVolume();
        object_yiBeiShu.SetActiveEX(true);
        object_erBeiShu.SetActiveEX(false);
        isYiBeiShu = true;
    }

    public override void Close()
    {
        base.Close();
        object_yiBeiShu.SetActiveEX(true);
        object_erBeiShu.SetActiveEX(false);
        isYiBeiShu = true;
        TimeMgr.Instance.TimeScale = 1;
    }

    private void SoundVolumeChanged(object _value)
    {
        float value = (float)_value;
        AudioMgr.Instance.SetSoundVolume((float)_value);
    }
    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);

        if (obj.Equals(btn_beishu)) 
        {
            if (isYiBeiShu)
            {
                isYiBeiShu = false;
                object_yiBeiShu.SetActiveEX(false);
                object_erBeiShu.SetActiveEX(true);
                TimeMgr.Instance.TimeScale = 1.5f;
            }
            else
            {
                isYiBeiShu = true;
                object_yiBeiShu.SetActiveEX(true);
                object_erBeiShu.SetActiveEX(false);
                TimeMgr.Instance.TimeScale = 1;
            }
        }
    }
}
