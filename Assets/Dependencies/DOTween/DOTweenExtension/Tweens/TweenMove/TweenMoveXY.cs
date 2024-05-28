﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[Serializable]
public class TweenMoveXY : BaseTween
{
    public Transform m_Transfrom;

    public Vector2 m_From = Vector2.zero;
    public Vector2 m_To = Vector2.zero;

    protected override void CreateTween()
    {
        base.CreateTween();
        if (m_Transfrom == null) return;
        Vector3 _To = m_To;
        _To.z = m_Transfrom.position.z;
        m_Tweener = m_Transfrom.DOMove(_To, m_Duration);
    }

    public override void ResetTween()
    {
        base.ResetTween();
        if (m_Transfrom == null) return;

        Vector3 _From = m_From;
        _From.z = m_Transfrom.position.z;
        m_Transfrom.position = _From;
    }
}
