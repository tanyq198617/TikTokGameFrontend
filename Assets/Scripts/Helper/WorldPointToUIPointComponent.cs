using System;
using UnityEngine;

/// <summary>
/// 3d模型坐标转ui下的坐标,gameobject的坐标,随相机改变改变
/// 脚本改到ui预制体上
/// </summary>
public class WorldPointToUIPointComponent : MonoBehaviour
{
    private Vector3 targetPoint = Vector3.zero;

    public Transform targetTransform;

    private RectTransform _rectTransform;
    public Camera worldCamera = null;
    public Camera uiCamera = null;
    private bool IsStart;

    public void Awake()
    {
        _rectTransform = transform.parent.GetComponent<RectTransform>();
    }

    public void SetTargetTransform(Transform _targetTran)
    {
        if (worldCamera == null)
            worldCamera = CameraMgr.Instance.MainCamera;
        if (uiCamera == null)
            uiCamera = UIRoot.Instance.UICamera;
        targetTransform = _targetTran;
        if (_targetTran != null)
            targetPoint = _targetTran.position;
    }

    public void LateUpdate()
    {
        if (targetTransform != null)
            SetPoint();
    }

    /// <summary> 刷新3d目标坐标值 /// </summary>
    public void RefreshTargetPoint()
    {
        if (targetTransform != null)
            targetPoint = targetTransform.position;
    }
    
    private void SetPoint()
    {
        if(worldCamera == null) return;
        Vector2 screenPoint = worldCamera.WorldToScreenPoint(targetPoint);
       // Vector2 screenPoint = worldCamera.WorldToScreenPoint(targetTransform.position);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTransform, screenPoint, uiCamera,
            out var localPos);
        transform.position = localPos;
    }

    private void OnDisable()
    {
        worldCamera = null;
        uiCamera = null;
        targetTransform = null;
        targetPoint = Vector3.zero * 1000;
    }
}