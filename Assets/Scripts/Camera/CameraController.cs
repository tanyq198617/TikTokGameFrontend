using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 defaultPos = new Vector3(0, 35f, -1f);
    Vector3 defaultEulerAngles = new Vector3(90, 0, 0);
    Vector3 centerPoint = new Vector3(0.04f, 0, 1.05f);


    private Vector3 redDefaultPos = new Vector3(-0.1f, 35f, -15f);
    private Vector3 redDefaultAngles = new Vector3(70f, 0, 0);
    private Vector3 redVerticalPos = new Vector3(-0.1f, 38.38f, -2f);
    private Vector3 redVerticalAngles = new Vector3(89.9f, 0f, 0);
    
    
    private Vector3 blueDefaultPos = new Vector3(-0.1f, 35f, 15f);
    private Vector3 blueDefaultAngles = new Vector3(73f, 180f, 0);
    private Vector3 blueVerticalPos = new Vector3(-0.1f, 37.55f, 4.09f);
    private Vector3 blueVerticalAngles = new Vector3(89.89f, 0f, 180);
    
    
    float angleXMin = 70, angleXMax = 90;

    private float speed = 25;

    /// <summary> 当前旋转角度 /// </summary>
    private float currentAngle = 0;

    /// <summary> z轴是否大于1 ,相机z轴大于1会使场景上台子的字反过来/// </summary>
    public bool greaterZhanOne;

    private bool isStartRotation;

    private void Awake()
    {
        //相机坐标不再地图的中心点,代码设置相机坐标
        transform.position = defaultPos;
        transform.localEulerAngles = defaultEulerAngles;
    }

    private void Start()
    {
        currentAngle = transform.localEulerAngles.x;
        isStartRotation = false;
    }

    public void SysUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            GetKeyA();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            GetKeyD();
        }
        else if (Input.GetKey(KeyCode.W))
        {
            if (currentAngle < angleXMax) RotateUp();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (currentAngle > angleXMin) RotateUp(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetRedVisualAngle();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetBlueVisualAngle();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetRedVerticaAngle();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetBlueVerticalAngle();
        }
        else
        {
            SetRotationState(false);
        }
    }

    /// <summary> 重置相机的坐标和旋转角度 /// </summary>
    public void ResetPointAndEulerAngles()
    {
        greaterZhanOne = false;
        isStartRotation = false;
        SetRedVisualAngle();
    }
    
    private void GetKeyA()
    {
        transform.RotateAround(centerPoint, Vector3.up, speed * Time.deltaTime);
        CheckGreaterZhanOne();
        SetRotationState(true);
    }

    private void GetKeyD()
    {
        transform.RotateAround(centerPoint, Vector3.up, -speed * Time.deltaTime);
        CheckGreaterZhanOne();
        SetRotationState(true);
    }

    void RotateUp(bool isUp = true)
    {
        int dir = isUp ? 1 : -1;
        float angle = dir * speed * Time.deltaTime;

        currentAngle += angle;
        transform.RotateAround(centerPoint, transform.right, angle);
        CheckGreaterZhanOne();
        SetRotationState(true);
    }

    /// <summary> 设置红方视角 /// </summary>
    void SetRedVisualAngle()
    {
        CameraMgr.Instance.StopCameraShake();
        transform.position = redDefaultPos;
        transform.localEulerAngles = redDefaultAngles;
        currentAngle = redDefaultAngles.x;
        CheckGreaterZhanOne();
    }
    
    /// <summary> 设置红方垂直视角 /// </summary>
    void SetRedVerticaAngle()
    {
        CameraMgr.Instance.StopCameraShake();
        transform.localPosition = redVerticalPos;
        transform.localEulerAngles = redVerticalAngles;
        currentAngle = redVerticalAngles.x;
        CheckGreaterZhanOne();
    }
    
    /// <summary> 设置蓝方视角 /// </summary>
    void SetBlueVisualAngle()
    {
        CameraMgr.Instance.StopCameraShake();
        transform.position = blueDefaultPos;
        transform.localEulerAngles = blueDefaultAngles;
        currentAngle = blueDefaultAngles.x;
        CheckGreaterZhanOne();
    }
    
    /// <summary> 设置蓝方垂直视角 /// </summary>
    void SetBlueVerticalAngle()
    {
        CameraMgr.Instance.StopCameraShake();
        transform.localPosition = blueVerticalPos;
        transform.localEulerAngles = blueVerticalAngles;
        currentAngle = blueVerticalAngles.x;
        CheckGreaterZhanOne();
    }
    
    /// <summary> 检测轴是否大于1,场景旋转掉头 /// </summary>
    private void CheckGreaterZhanOne()
    {
        bool _greaterZhanOne = transform.localPosition.z > 1;
        if (_greaterZhanOne != greaterZhanOne)
        {
            greaterZhanOne = _greaterZhanOne;
            
            RedCampController.Instance.GetYoungerBrother().SetPointOffset(greaterZhanOne);
            BlueCampController.Instance.GetYoungerBrother().SetPointOffset(greaterZhanOne);
            EventMgr.Dispatch(BattleEvent.Battle_Camera_Rotation, greaterZhanOne);
        }
    }

    /// <summary> 设置旋转状态 /// </summary>
    private void SetRotationState(bool _isRatation)
    {
        if (isStartRotation == _isRatation)
            return;
        isStartRotation = _isRatation;
        CameraMgr.Instance.StartOrStopRotation(_isRatation);
    }
}