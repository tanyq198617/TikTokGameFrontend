using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 触发器
/// </summary>
public class TriggerHelper : MonoBehaviour
{
    protected event Action<Collider> onTrigger;

    private void OnTriggerEnter(Collider other)
    {
        CheckTriggle(other);
    }

    private void OnTriggerStay(Collider other)
    {
        CheckTriggle(other);
    }

    private void CheckTriggle(Collider other)
    {
        if (Layer.IsBall(other.gameObject.layer))
        {
            if (onTrigger != null)
            {
                onTrigger(other);
            }
        }
    }

    public void AddTriggerEvent(Action<Collider> action)
    {
        onTrigger += action;
    }

    public void Clear()
    {
        onTrigger = null;
    }

    private void OnDestroy()
    {
        Clear();
    }
}
