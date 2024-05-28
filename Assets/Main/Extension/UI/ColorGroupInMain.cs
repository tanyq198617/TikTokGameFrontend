using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGroupInMain : MaskableGraphic
{
    private Graphic[] childs;

    protected override void Start()
    {
        base.Start();
        raycastTarget = false;
        childs = GetComponentsInChildren<Graphic>();
        m_OnDirtyVertsCallback += OnDirtyVertsCallback;
    }

    private void OnDirtyVertsCallback()
    {
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i].color = color;
        }
    }
}
