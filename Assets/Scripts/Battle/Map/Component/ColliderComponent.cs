using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 碰撞器
/// </summary>
public class ColliderComponent : MonoBehaviour
{
    [SerializeField] public Collider[] Colliders;
    [SerializeField] public Collider Resulter;
}
