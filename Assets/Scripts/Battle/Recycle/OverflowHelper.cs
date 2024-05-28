using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverflowHelper : MonoBehaviour
{
    protected event Action onFire;

    protected Ray _ray;

    private void Awake()
    {
        var position = transform.position;
        position.x = 10;
        _ray = new Ray(position, Vector3.left);
    }

    public void AddFireEvent(Action onFire)
    {
        this.onFire += onFire;
    }

    private void Update()
    {
#if UNITY_EDITOR
        Debug.DrawRay(_ray.origin, _ray.direction * 20, Color.red);
#endif
        if (!Physics.Raycast(_ray, 20, Layer.Ball))
        {
            if (onFire != null)
            {
                onFire();
            }
        }
    }
}
