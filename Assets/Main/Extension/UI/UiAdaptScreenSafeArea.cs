using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UiAdaptScreenSafeArea : MonoBehaviour
{
    private RectTransform _rectTrans;
    /// <summary>
    /// 原始屏幕方向
    /// </summary>
    private ScreenOrientation curOrientation;
    private void Start()
    {
        Init();
        AdaptAnchorsValue();
        curOrientation = Screen.orientation;
    }

    private void Update()
    {
        // 屏幕方向改变
        if (curOrientation != Screen.orientation)
        {
            // 执行适配
            AdaptAnchorsValue();
            curOrientation = Screen.orientation;
        }
    }

    private void Init()
    {
        _rectTrans = GetComponent<RectTransform>();

        _rectTrans.anchorMin = Vector2.zero;
        _rectTrans.anchorMax = Vector2.one;
        _rectTrans.anchoredPosition = Vector2.zero;
        _rectTrans.sizeDelta = Vector2.zero;

    }

    private void AdaptAnchorsValue()
    {
        var maxWidth = Screen.width;
        var maxHeight = Screen.height;
        var safeArea = Screen.safeArea;
        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= maxWidth;
        anchorMin.y /= maxHeight;
        anchorMax.x /= maxWidth;
        anchorMax.y /= maxHeight;

        _rectTrans.anchorMin = anchorMin;
        _rectTrans.anchorMax = anchorMax;
    }
}
