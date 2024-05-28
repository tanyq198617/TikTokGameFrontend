using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 发射组件
/// </summary>
public class FireComponent : MonoBehaviour
{
    private Ray _ray;

    private Vector3 form;
    private Vector3 to;

    public Transform trans;
    public Vector3 forward => trans.forward;

    private Tween _tween;
    private float curSpeed;
    
    private void Awake()
    {
        this.trans = transform;
        _ray = new Ray(trans.position, trans.forward);
    }

    public void Init(Vector3 form, Vector3 to, float speed = 0.5f)
    {
        this.form = form;
        this.to = to;
        this.curSpeed = speed;

        // 创建PingPong旋转动画
        Clear();
        // transform.rotation = Quaternion.Euler(form); 
        transform.eulerAngles = form;
        Play(speed);
    }

    public void SetTweenSpeed(float speed)
    {
        var duration = _tween?.fullPosition ?? 0; 
        var t = duration / curSpeed;
        duration = speed * t;
        curSpeed = speed;
        
        Clear();
        transform.eulerAngles = form; 
        
        Play(speed);
        if (duration > 0)
            _tween.fullPosition = duration;
    }

    private void Play(float speed)
    {
        _tween = transform.DORotate(this.to, speed, RotateMode.WorldAxisAdd)
            .SetEase(Ease.Linear) // 设置缓动函数，可根据需要选择
            .SetLoops(-1, LoopType.Yoyo); // 设置循环次数为无限，LoopType.Yoyo表示PingPong效果 
    }

#if UNITY_EDITOR
    private void Update()
    {
        _ray.origin = trans.position;
        _ray.direction = trans.forward;
        Debug.DrawRay(_ray.origin, _ray.direction * 10, Color.red);
    }

    public void SetName(string name)
    {
        gameObject.name = name;
    }
#endif

    public void Clear()
    {
        _tween?.Kill(false);
        _tween = null;
    }
}
