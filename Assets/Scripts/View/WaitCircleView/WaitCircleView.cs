using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[UIBind(UIPanelName.WaitcircleView)]
public class WaitCircleView : APanelBase
{
    public static WaitCircleView Instance { get { return Singleton<WaitCircleView>.Instance; } }

    public WaitCircleView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.Mask;
    }

    private TickedBase tickBase;
    private DelayTimeAction delayTime;
    private GameObject m_circle;
    private GameObject m_lock;
    private bool isRunning = false;
    private bool forever = false;

    public override void Init()
    {
        base.Init();
        m_circle = UIUtility.Control("circle", m_gameobj);
        m_lock = UIUtility.Control("empty", m_gameobj);
    }

    public void OnLockForever() => OnLock(true);
    public void OnLock() => OnLock(false);

    private void OnLock(bool forever = false)
    {
        InputControl.Locked(true);
        this.isRunning = true;
        this.forever = forever;
        if (IsOpen)
        {
            RefreshCircle();
            return;
        }
        UIMgr.Instance.ShowUICall(m_strPanelViewName, RefreshCircle);
    }

    public void UnLock()
    {
        isRunning = false;
        if (m_circle == null)
            return;
        ClearTick();
        ClearDelay();
        m_lock?.SetActive(isRunning);
        m_circle?.SetActiveEX(false);
        InputControl.Locked(false);
    }

    public override void Refresh()
    {
        base.Refresh();
        m_circle.SetActiveEX(false);
    }

    private void RefreshCircle()
    {
        m_lock.SetActive(isRunning);

        if (!isRunning)
            return;

        if (!m_circle.activeSelf)
        {
            //m_circle.SetActiveEX(false);
            delayTime = TimeMgr.Instance.Delay(0.75f, () =>
            {
                m_circle.SetActiveEX(isRunning);
                delayTime = null;
            });
        }

        if (tickBase == null)
            tickBase = TickedBase.Create(10.0f, TryUnLock, false, true);
        else
            tickBase.Reset();
    }

    private void TryUnLock()
    {
        if (forever)
            return;
        //if (NetMgr.Instance.IsConnected)
        //    UnLock();
    }

    private void ClearTick()
    {
        if (tickBase != null)
        {
            TickedMgr.Instance.Remove(tickBase);
            tickBase = null;
        }
    }

    private void ClearDelay()
    {
        if (delayTime != null)
        {
            TimeMgr.Instance.ClearDelay(delayTime);
            delayTime = null;
        }
    }

    public override void CloseUI()
    {
        base.CloseUI();
        UnLock();
        this.forever = false;
    }
}
