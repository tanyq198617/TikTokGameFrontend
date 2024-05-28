﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[Serializable]
public class TweenRotationX : BaseTween
{
    public Transform m_Transfrom;
    public RotateMode m_RotateMode = RotateMode.Fast;
    public float m_From = 0;
    public float m_To = 0;

    protected override void CreateTween()
    {
        base.CreateTween();
        if (m_Transfrom == null) return;
        var _To = m_Transfrom.localEulerAngles;
        _To.x = m_To;
        m_Tweener = m_Transfrom.DOLocalRotate(_To, m_Duration, m_RotateMode);
    }

    public override void ResetTween()
    {
        base.ResetTween();
        if (m_Transfrom == null) return;

        var _From = m_Transfrom.localEulerAngles;
        _From.x = m_From;
        m_Transfrom.localEulerAngles = _From;
    }
}
