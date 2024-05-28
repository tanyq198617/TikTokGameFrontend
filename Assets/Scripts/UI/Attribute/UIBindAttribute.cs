using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TCP消息处理标记
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class UIBindAttribute : Attribute
{
    private string uiname;
    public string UIName => uiname;

    public UIBindAttribute(string uiname) 
    {
        this.uiname = uiname;
    }
}
