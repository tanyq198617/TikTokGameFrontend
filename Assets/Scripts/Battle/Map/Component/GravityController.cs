using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HotUpdateScripts;
using UnityEngine;
using UnityEngine.Rendering;

public class GravityController : MonoBehaviour
{
    // 在Inspector窗口中可以设置的重力方向
    public Vector3 customGravityDirection = Vector3.forward;

    /// <summary> 皮肤层 </summary>
    public Transform _skin;

    public Transform skin
    {
        get
        {
            if (_skin == null)
            {
                _skin = UIUtility.Control("Skin", transform);
            }

            return _skin;
        }
    }

    private Rigidbody _rb;

    public Rigidbody rb
    {
        get
        {
            if (_rb == null)
            {
                _rb = GetComponent<Rigidbody>();
                _rb.useGravity = false;
            }

            return _rb;
        }
    }

    private CapsuleCollider _collider;

    public CapsuleCollider collider
    {
        get
        {
            if (_collider == null)
            {
                _collider = GetComponent<CapsuleCollider>();
                originalSize = _collider.radius;
            }

            return _collider;
        }
    }

    protected event Action<Collision> onCollisionEnter;
    protected event Action<Collision> onCollisionExit;

    private readonly List<Collision> stayHashSet = new List<Collision>();

    private float originalSize;
    private float bornTimer;
    private int attackLayer;
    private int selfLayer;
    private Vector3 bornPosition;
    private float noColliderTime;
    private float noColliderRange;
    private bool isPause = false;
    private bool isEffecting = false;
    private int targetLayer;
    private float maxSpeed;
    private float mass;
    private bool isRed;
    private const float bound = 15;

    void FixedUpdate()
    {
        if (isPause) return;
        if (isEffecting)
        {
            if (collider.isTrigger)
            {
                var pos = transform.position;
                bool isCollider = (isRed && pos.z > 7) || (!isRed && pos.z < -7);
                if (isCollider)
                {
                    rb.isKinematic = true;
                    collider.isTrigger = false;
                } 
            }
            return;
        }

        if (TimeMgr.RealTime - bornTimer >= noColliderTime)
        {
            // 计算当前速度的大小
            var velocity = rb.velocity;
            float currentSpeed = velocity.magnitude;
            var normalized = customGravityDirection.normalized;

            // 此处可以添加其他物理逻辑或限制速度的代码
            var pos = transform.position;
            if (isRed)
            {
                if (pos.z < -bound)
                {
                    // 计算速度向量和自定义重力方向在Z方向上的夹角
                    float angle = Vector3.Angle(Vector3.ProjectOnPlane(normalized, Vector3.up), Vector3.ProjectOnPlane(velocity.normalized, Vector3.up));
                    
                    if (angle > 30)
                    {
                        var force = Mathf.Min(-(pos.z + bound), TGlobalDataManager.OutsideForce);
                        if (force > maxSpeed) force = maxSpeed;
                        AddForce(normalized * force, ForceMode.VelocityChange);
                    }
                    else
                    {
                        if (currentSpeed < TGlobalDataManager.OutsideForce)
                        {
                            var force = Mathf.Min(-(pos.z + bound), TGlobalDataManager.OutsideForce);
                            AddForce(normalized * force, ForceMode.VelocityChange);
                        }
                        else
                        {
                            var newVelocity = velocity.normalized * maxSpeed;
                            rb.velocity = newVelocity;
                        }
                    }
                    return;
                }
            }
            else
            {
                if (pos.z > bound)
                {
                    // 计算速度向量和自定义重力方向在Z方向上的夹角
                    float angle = Vector3.Angle(Vector3.ProjectOnPlane(normalized, Vector3.up), Vector3.ProjectOnPlane(velocity.normalized, Vector3.up));
                    
                    if (angle > 30)
                    {
                        var force = Mathf.Min((pos.z - bound), TGlobalDataManager.OutsideForce);
                        if (force > maxSpeed) force = maxSpeed;
                        AddForce(normalized * force, ForceMode.VelocityChange);
                    }
                    else
                    {
                        if (currentSpeed < TGlobalDataManager.OutsideForce)
                        {
                            var force = Mathf.Min((pos.z - bound), TGlobalDataManager.OutsideForce);
                            AddForce(normalized * force, ForceMode.VelocityChange);
                        }
                        else
                        {
                            var newVelocity = velocity.normalized * maxSpeed;
                            rb.velocity = newVelocity;
                        }
                    }
                    return;
                }
            }

            if (currentSpeed < maxSpeed)
            {
                var gravity = customGravityDirection * Physics.gravity.magnitude;
                AddVelocity(gravity);
                return;
            }

            // 如果速度超过最大速度，则限制为最大速度
            if (currentSpeed > maxSpeed)
            {
                // 根据比例缩放速度
                var newVelocity = velocity.normalized * maxSpeed;
                // 更新刚体速度
                rb.velocity = newVelocity;
            }
        }
    }

    private void AddVelocity(Vector3 gravity)
    {
        // 设置物体的自定义重力方向
        rb.velocity += gravity * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        targetLayer = 1 << other.gameObject.layer;
        if ((attackLayer & targetLayer) != 0 && onCollisionEnter != null)
        {
            onCollisionEnter(other);
            stayHashSet.Add(other);
        }
    }

    // private void OnCollisionStay(Collision other)
    // {
    //     targetLayer = 1 << other.gameObject.layer;
    //     if ((attackLayer & targetLayer) != 0 && onCollisionEnter != null)
    //     {
    //         onCollisionEnter(other);
    //     }
    // }

    private void OnCollisionExit(Collision other)
    {
        if (onCollisionExit != null)
            onCollisionExit(other);
        stayHashSet.Remove(other);
    }

    private void OnTriggerEnter(Collider other)
    {
        //!=0为包含
        if (collider.isTrigger)
        {
            targetLayer = 1 << other.gameObject.layer;
            if ((attackLayer & targetLayer) != 0 ||
                Layer.IsNotBall(isRed, other.gameObject.layer))
            {
                SetTriggle(false);
            }
        }
    }

    private void Update()
    {
        if (isPause || isEffecting)
            return;

        if (collider.isTrigger && (TimeMgr.RealTime - bornTimer >= noColliderTime ||
                                   (transform.position - bornPosition).sqrMagnitude >=
                                   noColliderRange * noColliderRange))
        {
            AddForce(customGravityDirection.normalized * maxSpeed, ForceMode.VelocityChange);
            SetMass(this.mass);
            SetTriggle(false);
        }

        // 检查当前帧是否会被渲染
        // if (OnDemandRendering.willCurrentFrameRender)
        // {
        //结算
        // if (stayHashSet.Count > 0)
        // {
        //     for (int i = stayHashSet.Count - 1; i >= 0; i--)
        //     {
        //         OnCollisionEnter(stayHashSet[i]);
        //     }
        // }
        // }
    }

    public void Schedule(bool isRed, float noColliderTime, float noColliderRange, float size, float mass)
    {
        this.isRed = isRed;
        this.mass = mass;
        this.isEffecting = false;
        this.noColliderTime = noColliderTime;
        this.noColliderRange = noColliderRange;
        transform.position = Vector3.one * 1000;
        SetTriggle(true);
        rb.velocity = Vector3.zero;
        SetColliderActive(false);
        rb.Sleep();
        SetSize(size);
        SetMass(mass);
        enabled = false;
    }

    public void OnBorn(Vector3 position, Vector3 gravity, Vector3 forward, float speed)
    {
        position.y = 0f;
        transform.position = bornPosition = position;
        customGravityDirection = gravity;
        rb.velocity = forward * speed;
        rb.WakeUp();
        rb.isKinematic = false;
        this.isEffecting = false;
        SetTriggle(true);
        bornTimer = TimeMgr.RealTime;
        SetColliderActive(true);
        SetMass(10000);
        enabled = true;
        isPause = false;
    }

    public void OnOverflowBorn(Vector3 position, Vector3 gravity, Vector3 forward)
    {
        position.y = 0f;
        transform.position = bornPosition = position;
        customGravityDirection = gravity;
        rb.velocity = forward;
        rb.WakeUp();
        rb.isKinematic = false;
        this.isEffecting = false;
        SetTriggle(false);
        bornTimer = TimeMgr.RealTime - noColliderTime;
        SetColliderActive(true);
        enabled = true;
        isPause = false;
    }

    public void SetLayer(int layer, int attacklayer)
    {
        if (gameObject == null) return;
        gameObject.layer = selfLayer = layer;
        this.attackLayer = attacklayer;
    }

    public void SetColliderActive(bool active)
    {
        collider.SetEnabledEX(active);
    }

    public void SetTriggle(bool isTrigger)
    {
        collider.isTrigger = isTrigger;
    }

    public void SetSize(float size)
    {
        //skin.localScale = Vector3.one * size;
        // skin.transform.localScale = Vector3.one * size;
        // collider.radius = originalSize * size;
        transform.localScale = Vector3.one * size;
    }

    public void SetMass(float mass)
    {
        rb.mass = mass;
    }

    public void SetMaxSpeed(float maxSpeed)
    {
        this.maxSpeed = maxSpeed;
    }

    public void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Acceleration)
    {
        rb.AddForce(force, forceMode);
    }

    public void Stop()
    {
        // rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        isPause = true;
    }

    /// <summary>
    /// 改变移动方向
    /// </summary>
    public void ChangeVelocity(float moveSpeed)
    {
        var speed = customGravityDirection.normalized * moveSpeed;
        maxSpeed = moveSpeed;
        rb.isKinematic = true;
        rb.velocity = speed;
        // rb.mass = 100000;
        isEffecting = true;
        transform.forward = customGravityDirection.normalized;
        AddForce(speed, ForceMode.VelocityChange);
        rb.isKinematic = false;
    }

    public void Clear()
    {
        stayHashSet.Clear();
        onCollisionEnter = null;
        onCollisionExit = null;
        selfLayer = 0;
        attackLayer = 0;
        maxSpeed = 1;
        rb.Sleep();
        SetColliderActive(false);
        enabled = false;
        isEffecting = false;
    }

    public void AddCollisionEnter(Action<Collision> callback) => onCollisionEnter += callback;
    public void RemoveCollisionEnter(Action<Collision> callback) => onCollisionEnter -= callback;
}