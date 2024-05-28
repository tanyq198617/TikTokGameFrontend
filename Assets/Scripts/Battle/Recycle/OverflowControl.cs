using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 溢出球球控制管理
/// </summary>
public class OverflowControl : AItemPageBase
{
    protected readonly Queue<OverflowBall> _queue = new Queue<OverflowBall>();

    protected OverflowHelper _overflowHelper;

    protected float z;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        _overflowHelper = Trans.GetOrAddComponent<OverflowHelper>();
        _overflowHelper.AddFireEvent(OnFire);
        _overflowHelper.enabled = false;
        this.z = Trans.position.z;
    }

    public override void Refresh()
    {
        base.Refresh();
        _overflowHelper.enabled = true;
    }

    /// <summary> 压入数据 </summary>
    public void EnqueueOverflowBall(OverflowBall ball)
    {
        if (CityGlobal.Instance.IsOver)
            return;
        _queue.Enqueue(ball);
    }

    private void OnFire()
    {
        if (CityGlobal.Instance.IsOver)
            return;
        if (_queue.Count > 0)
        {
            var data = _queue.Dequeue();
            var position = data.Position;
            position.z = z;
            var ball = BallFactory.GetOrCreate(data.Camp == CampType.红, data.Config, data.Player, data.IsShowHead);
            ball.OnOverflowBorn(position, Trans.forward);
        }
    }

    /// <summary> 清除溢出待发射的球球 /// </summary>
    public void ClearOverflowBall()
    {
        _queue.Clear();
    }
    
    public override void Close()
    {
        base.Close();
        _queue.Clear();
        _overflowHelper.enabled = false;
    }
}
