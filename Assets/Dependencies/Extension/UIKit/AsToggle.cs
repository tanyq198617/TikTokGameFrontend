using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AsToggle : Toggle
{
    public EventTriggerListener.EventHandle<PointerEventData> onClick = new EventTriggerListener.EventHandle<PointerEventData>();

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        onClick?.Invoke(gameObject, eventData);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        onClick.RemoveAllListener();
    }
}
