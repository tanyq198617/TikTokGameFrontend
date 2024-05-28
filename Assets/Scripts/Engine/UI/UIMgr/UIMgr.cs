using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : Singleton<UIMgr>
{
    public UIMgr() { }

    public Dictionary<string, APanelBase> m_uIbaseDic = new Dictionary<string, APanelBase>();

    public List<APanelBase> m_openUIbaseList = new List<APanelBase>();

    public void Register<T>(string name, T panel) where T : APanelBase
    {
        if (string.IsNullOrEmpty(name) || panel == null) return;

        if (m_uIbaseDic.ContainsKey(name))
        {
            Debuger.LogError("panel为[{0}]注册为{1}失败，{2}已定义为{3}", panel.GetType(), name, name, m_uIbaseDic[name].GetType());
        }
        else
        {
            panel.m_strPanelViewName = name;
            m_uIbaseDic[name] = panel;
        }
    }

    public void Register(string name, APanelBase panel)
    {
        if (string.IsNullOrEmpty(name) || panel == null) return;

        if (m_uIbaseDic.ContainsKey(name))
        {
            Debuger.LogError("panel为[{0}]注册为{1}失败，{2}已定义为{3}", panel.GetType(), name, name, m_uIbaseDic[name].GetType());
        }
        else
        {
            panel.m_strPanelViewName = name;
            m_uIbaseDic[name] = panel;
        }
    }

    /// <summary>
    /// 打开UI
    /// </summary>
    /// <param name="uibaseName"></param>
    public void ShowUI(string uibaseName, Action<APanelBase> openCall = null, Action<APanelBase> closeCall = null, bool isClearAll = true)
    {
        if (m_uIbaseDic.ContainsKey(uibaseName))
        {
            if (!m_uIbaseDic[uibaseName].isFilm && isClearAll)
                CloseAllUI(false);
            closeCall += (panel) => { m_openUIbaseList.Remove(panel); };
            m_uIbaseDic[uibaseName].ShowUI(openCall, closeCall);
            m_openUIbaseList.Add(m_uIbaseDic[uibaseName]);
        }
        else
            Debug.LogError($"[严重错误] {uibaseName} 未注册,请检查!");
    }

    public void ShowUICall(string uibaseName, Action openCall = null, Action closeCall = null, bool isClearAll = true)
    {
        if (m_uIbaseDic.ContainsKey(uibaseName))
        {
            if (!m_uIbaseDic[uibaseName].isFilm && isClearAll)
                CloseAllUI(false);
            m_uIbaseDic[uibaseName].ShowUI(openCall, closeCall, (panel) => { m_openUIbaseList.Remove(panel); });
            m_openUIbaseList.Add(m_uIbaseDic[uibaseName]);
        }
        else
            Debug.LogError($"[严重错误] {uibaseName} 未注册,请检查!");
    }


    private List<APanelBase> _tempHideList = new List<APanelBase>();


    public APanelBase GetShowAndNoFilmUI()
    {
        if (m_openUIbaseList == null || m_openUIbaseList.Count == 0) return null;

        for (int i = 0; i < m_openUIbaseList.Count; i++)
        {
            if (!m_openUIbaseList[i].isFilm && !m_openUIbaseList[i].m_IsKeepOpen && (m_openUIbaseList[i].IsOpen))
                return m_openUIbaseList[i];
        }
        return null;
    }

    public void PreLoadUI(string uibaseName)
    {
        if (m_uIbaseDic.ContainsKey(uibaseName))
            m_uIbaseDic[uibaseName].PreLoadUI().Forget();
    }

    public APanelBase GetUI(string uibaseName)
    {
        APanelBase ui = null;
        if (m_uIbaseDic.ContainsKey(uibaseName))
        {
            ui = m_uIbaseDic[uibaseName];
            if (ui.m_gameobj == null)
                ui = null;
        }
        return ui;
    }

    public void DestoryUI(string uibaseName)
    {
        APanelBase uibase = GetUI(uibaseName);

        if (uibase != null)
        {
            m_uIbaseDic.Remove(uibaseName);
            uibase.Destroy();
        }
    }

    public void CloseAndDestoryUI(string uibaseName)
    {
        APanelBase uibase = GetUI(uibaseName);

        if (uibase != null)
        {
            uibase.CloseUI();
            uibase.Destroy();
        }
    }

    public bool IsOpen(string planeName)
    {
        if (string.IsNullOrEmpty(planeName) || !m_uIbaseDic.ContainsKey(planeName))
            return false;
        return m_uIbaseDic[planeName].IsOpen;
    }

    public bool InHavePanel(string planeName) { return m_uIbaseDic.ContainsKey(planeName); }


    public void CloseUI(string uibaseName)
    {
        if (m_uIbaseDic.ContainsKey(uibaseName))
        {
            m_uIbaseDic[uibaseName].CloseUI();
            //Debuger.Log("---->>>CloseUI  uibaseName ***************   " + uibaseName);
            m_openUIbaseList.Remove(m_uIbaseDic[uibaseName]);
        }
    }

    public void Clear(string uibaseName)
    {
        if (m_uIbaseDic.ContainsKey(uibaseName))
            m_uIbaseDic[uibaseName].Clear();
    }

    /// <summary>
    /// 获取UI
    /// </summary>
    /// <param name="uibaseName"></param>
    /// <returns></returns>
    public APanelBase Get(string uibaseName)
    {
        if (m_openUIbaseList == null || m_openUIbaseList.Count == 0) return null;

        for (int i = 0; i < m_openUIbaseList.Count; i++)
        {
            if (m_openUIbaseList[i].m_gameobj != null && m_openUIbaseList[i].m_gameobj.name == uibaseName)
            {
                return m_openUIbaseList[i];
            }
        }
        return null;
    }

    public int GetUIIndex(string uibaseName)
    {
        if (m_openUIbaseList == null || m_openUIbaseList.Count == 0)
            return 0;

        for (int i = 0; i < m_openUIbaseList.Count; i++)
        {
            if (m_openUIbaseList[i].m_gameobj != null && m_openUIbaseList[i].m_gameobj.name == uibaseName)
            {
                return m_openUIbaseList[i].GetCurrentIndex();
            }
        }
        return 0;
    }

    /// <summary>
    /// 关掉打开ui
    /// </summary>
    public void CloseAllUI(bool isFilm)
    {
        if (m_openUIbaseList == null || m_openUIbaseList.Count <= 0)
            return;

        for (int i = m_openUIbaseList.Count - 1; i >= 0; i--)
        {
            APanelBase ui = m_openUIbaseList[i];
            if (ui.isFilm.Equals(isFilm) && !ui.m_IsKeepOpen)
            {
                ui.SetEventNull();
                ui.CloseUI();
                m_openUIbaseList.Remove(ui);
            }
        }
    }

    public void CloseAllUI()
    {
        if (m_openUIbaseList == null || m_openUIbaseList.Count <= 0)
            return;

        for (int i = m_openUIbaseList.Count - 1; i >= 0; i--)
        {
            APanelBase ui = m_openUIbaseList[i];
            if (!ui.m_IsKeepOpen)
            {
                ui.SetEventNull();
                ui.CloseUI();
                m_openUIbaseList.Remove(ui);
            }
        }
    }

    public void CloseAllUI(string excludeUI)
    {
        if (m_openUIbaseList == null || m_openUIbaseList.Count <= 0)
            return;

        for (int i = m_openUIbaseList.Count - 1; i >= 0; i--)
        {
            APanelBase ui = m_openUIbaseList[i];
            if (ui.m_IsKeepOpen || ui.Name.Equals(excludeUI))
                continue;

            ui.SetEventNull();
            ui.CloseUI();
            m_openUIbaseList.Remove(ui);
        }
    }

    public void CloseAllUI(UIPanelType type)
    {
        if (m_openUIbaseList == null || m_openUIbaseList.Count <= 0)
            return;

        for (int i = m_openUIbaseList.Count - 1; i >= 0; i--)
        {
            APanelBase ui = m_openUIbaseList[i];
            if (ui.m_Type == type)
            {
                ui.SetEventNull();
                ui.CloseUI();
                m_openUIbaseList.Remove(ui);
            }
        }
    }
}
