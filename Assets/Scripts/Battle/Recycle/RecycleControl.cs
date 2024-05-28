using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 越界球球回收器
/// </summary>
public class RecycleControl : AItemPageBase
{
    /// <summary> 阵营 </summary>
    protected CampType Camp;

    protected TriggerHelper _triggleHelper;

    protected OverflowControl _control;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        _triggleHelper = Trans.GetOrAddComponent<TriggerHelper>();
        _triggleHelper.AddTriggerEvent(OnTrigger);
    }

    public void Set(bool isRed, OverflowControl control)
    {
        _control = control;
        Camp = isRed ? CampType.红 : CampType.蓝;
    }

    /// <summary>
    /// 触发溢出检测 
    /// </summary>
    private void OnTrigger(Collider other)
    {
        if(other.gameObject == null) return;
        var ball = BallFactory.Find(other.gameObject);
        if (ball == null) return;
        var oBall = OverflowBall.Create(ball);
        _control.EnqueueOverflowBall(oBall);
        ball.Recycle();
    }
}
