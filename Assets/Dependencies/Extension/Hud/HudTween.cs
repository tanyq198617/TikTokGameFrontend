using DG.Tweening;
using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 血条动画
/// </summary>
public class HudTween : MonoBehaviour
{
    private Vector3 original;
    private float ignoreValue = 0.01f;
    private Ease easeType;

    private float startTime;
    private float to;
    private float duration;
    private bool running = false;
    private Action callback;

    private float _timer = 0;
    private float _delay = 0.25f;
    private Vector3 tempPos;
    private float passedTime;

    private void Awake()
    {
        original = transform.localPosition;
    }

    public void DOLocalMoveY(Ease easeType, float endValue, float duration, Action onCompleted, float ignoreValue = 0.01f)
    {
        this.transform.localPosition = original;
        this.easeType = easeType;
        this.to = endValue;
        this.duration = duration;
        this.ignoreValue = ignoreValue;
        this.startTime = Time.time;
        this.callback = onCompleted;
        this.running = true; 
    }

    public void Execute()
    {
        if (transform == null)
        {
            return;
        }
        passedTime = Time.time - startTime;
        float v = EaseManager.Evaluate(easeType, null, passedTime, duration, 0, 0);
        tempPos = transform.localPosition;
        tempPos.y = Mathf.Lerp(transform.localPosition.y, to, v);
        transform.localPosition = tempPos;
        CheckComplete(tempPos.y);
    }

    public void OnTopTo(float value)
    {
        to += value;
        //tempPos = transform.localPosition;
        //transform.localPosition = new Vector3(tempPos.x, tempPos.y + value, tempPos.z);
    }

    public void OnTweenTopTo(float value, float duration, float ignoreValue = 0.01f)
    {
        this.to = this.transform.localPosition.y + value;
        this.duration = duration;
        this.ignoreValue = ignoreValue;
        this.startTime = Time.time;
        this.running = true;
    }

    private void CheckComplete(float y)
    {
        _timer -= Time.time;
        if (_timer <= 0)
        {
            _timer = Time.time + _delay;
            if (Mathf.Abs(y - to) <= ignoreValue)
            {
                OnComplete();
            }
        }
    }

    public void Update()
    {
        if (running)
        {
            Execute();
        }
    }

    public void OnComplete()
    {
        callback?.Invoke();
        Clear();
    }

    public void Clear()
    {
        this.to = 0;
        this.duration = 0;
        this.ignoreValue = 0.01f;
        this.startTime = Time.time;
        this.callback = null;
        this.running = false;
    }
}
