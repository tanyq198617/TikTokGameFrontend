using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 结算点触发
/// </summary>
public class ResultComponent : MonoBehaviour
{
    protected int resultLayer;
    protected float interval;
    protected float timer;
    
    protected event Action<Collision> onCollision;
    
    public void SetResultLayer(int layer, float interval = 0.1f)
    {
        this.resultLayer = layer;
        this.interval = interval;
        this.timer = 0;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        CheckCollision(other);
    }

    private void OnCollisionStay(Collision other)
    {
        CheckCollision(other);
    }

    private void CheckCollision(Collision other)
    {
        if (MathUtility.IsOutTime(ref timer, interval))
        {
            if ((resultLayer & 1 << other.gameObject.layer) != 0)
            {
                onCollision?.Invoke(other);
            }
            timer = 0;
        }
    }

    public void AddCollision(Action<Collision> callback) => onCollision += callback;

    public void Clear()
    {
        resultLayer = 0;
    }

    private void OnDestroy()
    {
        onCollision = null;
    }
}
