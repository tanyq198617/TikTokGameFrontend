using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Collections;
using UnityEngine;

/// <summary>
/// 地图划线
/// </summary>
public class MapLine : AItemPageBase
{
    private LineRenderer _renderer;
    private LineRenderComponent _component;
    
    private List<Vector3> positions = new List<Vector3>();
    private float totalLength = 0;
    
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        _renderer = Trans.GetComponent<LineRenderer>();
        _component = Trans.GetComponent<LineRenderComponent>();
        _component.onMouseDown += OnMouseDown;
        _component.onMouse += OnMouse;
        _component.onMouseUp += OnMouseUp;
    }

    private void OnMouseUp()
    {
        // Debuger.LogError($"共{positions.Count}个点, 长度：{totalLength}");
        _renderer.positionCount = 0;
        CameraMgr.Instance.TweenTo(positions, () =>
        {
            totalLength = 0;
            positions.Clear();
        }).Forget();
    }

    private void OnMouse()
    {
        Vector3 currentPoint = CameraMgr.Instance.ScreenToWorldPoint(Input.mousePosition);
        Vector3 previousPoint = _renderer.GetPosition(_renderer.positionCount - 1);
        if(currentPoint == previousPoint) return;
        _renderer.positionCount++;
        totalLength += Vector3.Distance(previousPoint, currentPoint);
        currentPoint.z = 0;
        _renderer.SetPosition(_renderer.positionCount - 1, currentPoint);
        positions.Add(currentPoint);
    }

    private void OnMouseDown()
    {
        //添加自身位置
        positions.Clear();
        _renderer.positionCount = 1;
        Vector3 startPosition = CameraMgr.Instance.Trans.position;
        _renderer.SetPosition(0, startPosition);
        positions.Add(startPosition);
        totalLength = 0;
    }

    public void initialize()
    {
    }

}
