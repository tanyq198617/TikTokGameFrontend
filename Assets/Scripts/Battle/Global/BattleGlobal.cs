using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 全局战斗入口
/// </summary>
public class BattleGlobal : Singleton<BattleGlobal>
{
    private readonly Dictionary<Type, ABattleControl> controls = new Dictionary<Type, ABattleControl>();
    
    /// <summary> 当前执行战斗种类 </summary>
    public ABattleControl Control { get; private set; }
    
    
    /// <summary> 获取战斗种类 </summary>
    public ABattleControl GetControl(Type t)
    {
        if (!controls.TryGetValue(t, out var control) || control.IsNull())
        {
            control = ActivatorFactory.CreateInstance(t) as ABattleControl;
            controls.Add(t, control);
        }
        return control;
    }
    
    public void OnEnter<T>(int regionId, bool isForced = false, Action callback = null, object args = null) where T : ABattleControl, new ()
    {
        Control?.Clear();
        Control = GetControl(typeof(T));
        EventMgr.Dispatch(BattleEvent.Battle_TryEnter_Scene);
        Control.OnEnter(regionId, isForced, callback, args);
    }
    
    public void OnExit(int args, bool v)
    {
        Control?.OnExit(args, v);
    }
}
