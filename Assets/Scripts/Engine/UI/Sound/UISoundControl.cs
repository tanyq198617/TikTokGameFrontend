using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundControl
{
    private APanelBase m_panel;

    public UISoundControl(APanelBase panel)
    {
        this.m_panel = panel;
    }
    public virtual void PlayOpenSound()
    {
    }
    public void PlayCloseSound()
    {
    }

    public void PlayButtonSound(GameObject obj)
    {
    }

    public void PlayMusic()
    {
    }

    public void ResetMusic()
    {
    }
}
