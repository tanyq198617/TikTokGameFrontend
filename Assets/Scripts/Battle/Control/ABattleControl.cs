using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABattleControl
{
    /// <summary> 手动进入关卡 </summary>
    public abstract void OnEnter(int regionId, bool isForced = false, Action callback = null, object args = null);

    /// <summary> 退出关卡 </summary>
    public abstract void OnExit(int args, bool isSysJump);

    /// <summary> 清理关卡数据 </summary>
    public abstract void Clear();
}
