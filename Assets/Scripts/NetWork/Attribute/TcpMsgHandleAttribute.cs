using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameNetwork
{
    /// <summary>
    /// TCP消息处理标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TcpMsgHandleAttribute : Attribute
    {
    }
}
