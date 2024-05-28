using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[UIBind(UIPanelName.TestView)]
public class TestView : APanelBase
{
    public static TestView Instance { get { return Singleton<TestView>.Instance; } }

    public TestView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.One;
    }    

    private TestCenterPage centerPage;
    private TestTopPage topPage;
    private TestBottomPage bottomPage;
    private TestLeftPage leftPage;

    public override void Init()
    {
        base.Init();

        centerPage = UIUtility.CreatePageNoClone<TestCenterPage>(Trans, "Center");
        topPage = UIUtility.CreatePageNoClone<TestTopPage>(Trans, "Top");
        bottomPage = UIUtility.CreatePageNoClone<TestBottomPage>(Trans, "Bottom");
        leftPage = UIUtility.CreatePageNoClone<TestLeftPage>(Trans, "Left");

    }

    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);


    }
}
