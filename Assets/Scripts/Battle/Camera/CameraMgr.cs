using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using HotUpdateScripts;
using UnityEngine.Rendering.Universal;

public class CameraMgr : Singleton<CameraMgr>, IEvent
{
    public Camera MainCamera { get; private set; }

    public Transform Trans { get; private set; }

    private List<Camera> cameraStack;

    // private SimpleCameraController controller;
    private bool isEnable;
    private bool isShaking;
    private Tween _tween;
    private Camera textCamera;
    private List<Camera> giftTimeLineCameraList = new List<Camera>();
    private List<Camera> ballRuChangTimeLineCameraList = new List<Camera>();

    /// <summary> 相机旋转,移动脚本 /// </summary>
    public CameraController cameraController { get; private set; }

    /// <summary> 相机振动前的坐标 /// </summary>
    private Vector3 cameraShakeInitPoint;

    /// <summary> 相机是否在旋转 /// </summary>
    private bool isRotation;
    
    public void initialize()
    {
        if (MainCamera == null)
        {
            MainCamera = Camera.main;
            Trans = MainCamera.transform;
            cameraStack = MainCamera.GetUniversalAdditionalCameraData().cameraStack;
            // controller = MainCamera.GetComponent<SimpleCameraController>();
            UIRoot.Instance.SetUrpRenderType(CameraRenderType.Overlay);
            textCamera = UIUtility.GetComponent<Camera>(Trans, "textProCamera");
            cameraStack.Add(textCamera);
            cameraStack.Add(UIRoot.Instance.UICamera);
            cameraController = MainCamera.GetOrAddComponent<CameraController>();
            // UIRoot.Instance.SetDepth(-2);
        }
        cameraController?.ResetPointAndEulerAngles();
        giftTimeLineCameraList.Clear();
        AddEventListener();
        isRotation = false;
    }

    public void SetPostProcessingActive(Camera camera, bool active)
    {
        if (camera != null)
            camera.GetUniversalAdditionalCameraData().renderPostProcessing = active;
    }

    #region 添加移除Urp CameraStack

    /// <summary> 添加相机到urp相机队列,目前动态加的相机层级都在textcamera相机后面,ui相机前面 /// </summary>
    public void AddCameraStack(Camera camera,TimeLineEnum _enum)
    {
        if (camera.IsNull())
            return;
        switch (_enum)
        {
            case TimeLineEnum.Gift:
                giftTimeLineCameraList.Add(camera);
                break;
            case TimeLineEnum.Ball:
                ballRuChangTimeLineCameraList.Add(camera);
                break;
            default:
                break;
        }
        SetCameraStack();
    }

    /// <summary> 从urp相机队列中删除相机 /// </summary>
    public void RemoveCameraStack(Camera camera,TimeLineEnum _enum)
    {
        if (camera.IsNull())
            return;
        switch (_enum)
        {
            case TimeLineEnum.Gift:
                giftTimeLineCameraList.Remove(camera);
                break;
            case TimeLineEnum.Ball:
                ballRuChangTimeLineCameraList.Remove(camera);
                break;
            default:
                break;
        }
        SetCameraStack();
    }

    /// <summary> 设置相机队列 /// </summary>
    private void SetCameraStack()
    {
        cameraStack.Clear();
        cameraStack.Add(textCamera);
        cameraStack.AddRange(ballRuChangTimeLineCameraList);
        cameraStack.AddRange(giftTimeLineCameraList);
        cameraStack.Add(UIRoot.Instance.UICamera);
    }

    #endregion

    public async UniTask TweenTo(List<Vector3> positions, Action onfinish)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            Vector3 pos = Trans.position;
            float distance = Vector3.Distance(pos, positions[i]);
            if (distance > 1)
            {
                await Trans.DOMove(positions[i], 0.1f).ToUniTask();
            }
            else
            {
                Trans.position = positions[i];
                await UniTask.Yield();
            }
        }

        onfinish?.Invoke();
    }

    public Vector3 ScreenToWorldPoint(Vector3 pos)
    {
        return MainCamera.ScreenToWorldPoint(pos);
    }

    /// <summary>
    /// 震屏,1普通震屏,2神龙震屏,3黑洞震屏
    /// </summary>
    /// <param name="type"></param>
    public void Shake(int type)
    {
        if (Trans == null)
            return;
        
        if (isShaking)
            return;
        else if (isRotation)
            return;
        
        isShaking = true;
        var data = TCameraShakeManager.Instance.GetItem(type);
        if (data == null)
        {
            Debuger.LogError($"不存在的震动配置,ID={type}");
            return;
        }

        cameraShakeInitPoint = Trans.position;
        // 调用DOShakePosition方法进行屏幕震动
        isEnable = cameraController.enabled;
        cameraController.enabled = false;
        _tween = Trans.DOShakePosition((float)data.time, (float)data.shakeDynamics, data.shakeCount);
        _tween.OnComplete(() =>
        {
            cameraController.enabled = isEnable;
            isShaking = false;
        });
    }

    /// <summary> 相机开始,停止旋转 /// </summary>
    public void StartOrStopRotation(bool _isRatation)
    {
        isRotation = _isRatation;
        if (isRotation)
        {
            if (isShaking)
            {
                _tween?.Kill(false);
                Trans.position = cameraShakeInitPoint;
                isShaking = false;
            }
        }
    }
    
    /// <summary> 停止相机抖动 /// </summary>
    public void StopCameraShake()
    {
        if (isShaking)
        {
            _tween?.Kill(false);
            Trans.position = cameraShakeInitPoint;
            isShaking = false;
        }
    }
    
    public void Clear()
    {
        _tween?.Kill(false);
        _tween = null;
        isShaking = false;
        cameraStack?.Clear();
        UIRoot.Instance.SetUrpRenderType(CameraRenderType.Base);
        RemoveEventListener();
        MainCamera = null;
        Trans = null;
        if (cameraController != null)
        {
            cameraController?.ResetPointAndEulerAngles();
            cameraController = null;
        }
    }

    public void AddEventListener()
    {
        EventMgr.AddEventListener<int>(BattleEvent.Battle_Camera_Shake, Shake);
    }

    public void RemoveEventListener()
    {
        EventMgr.RemoveEventListener<int>(BattleEvent.Battle_Camera_Shake, Shake);
    }
}

public enum TimeLineEnum
{
    Gift,
    Ball
}