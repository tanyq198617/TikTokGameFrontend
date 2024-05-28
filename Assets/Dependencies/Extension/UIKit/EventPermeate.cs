using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class EventPermeate : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    // 事件穿透对象
    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public Action clickBack;
    private List<RaycastResult> results = new List<RaycastResult>();

    // 监听按下
    public void OnPointerDown(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.pointerDownHandler);
    }

    // 监听抬起
    public void OnPointerUp(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.pointerUpHandler);
    }

    // 监听点击
    public void OnPointerClick(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.submitHandler);
        PassEvent(eventData, ExecuteEvents.pointerClickHandler, true);
    }

    // 把事件透下去
    public void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function, bool isClick = false)
        where T : IEventSystemHandler
    {
        if (target == null)
            return;
        EventSystem.current.RaycastAll(data, results);
        GameObject current = data.pointerCurrentRaycast.gameObject;
        for (int i = 0; i < results.Count; i++)
        {
            if (target != results[i].gameObject)
                continue;

            // 如果是目标物体，则把事件透传下去，然后break
            ExecuteEvents.Execute(results[i].gameObject, data, function);

            if (isClick && clickBack != null)
                clickBack();
            break;
        }
    }
}