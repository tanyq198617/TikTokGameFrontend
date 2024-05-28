using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HudAdjust : MonoBehaviour
{

    [SerializeField]
    private Transform _adjustNodeTrans = null;

    [SerializeField, SetProperty("baseScale")]
    private Vector3 _baseScale = new Vector3(0.01f, 0.01f, 0.01f);

    public Vector3 baseScale
    {
        get { return _baseScale; }
        set { _baseScale = value; }
    }

    void LateUpdate()
    {
        UpdateAdjustNode();
    }

    void UpdateAdjustNode()
    {
        if (Camera.main == null)
            return;

        var cameraTrans = Camera.main.transform;
        var dis = Vector3.Dot(transform.position - cameraTrans.position, cameraTrans.forward);

        // _adjustNodeTrans.forward = cameraTrans.forward;

        //因为如果只是forward,LookAt等对齐（保持worldUp），只能保证一个轴向一致，美术的相机或者设定有可能z轴向有轻微角度
        //方式1
        _adjustNodeTrans.rotation = cameraTrans.rotation;
        //方式2
        //_adjustNodeTrans.forward = cameraTrans.forward;
        // _adjustNodeTrans.up = cameraTrans.up;

        _adjustNodeTrans.localScale = baseScale * dis;

    }
}