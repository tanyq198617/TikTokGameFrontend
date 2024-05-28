using System;
using System.Collections;
using System.Collections.Generic;
using HotUpdateScripts;
using UnityEngine;

/// <summary>
/// 阵营基类
/// </summary>
public abstract class ACampControllerBase : AItemPageBase
{
    private Transform _root;
    
    /// <summary> 发射器(炮口), key=发射器ID, value=发射器 </summary>
    protected readonly Dictionary<int, MuzzleControl> muzzleDict = new Dictionary<int, MuzzleControl>();
    
    /// <summary> 结算点 </summary>
    protected AResultPointBase resultPoint;
    
    /// <summary> 角色点管理器 </summary>
    protected RichManPointControl richManControl;

    /// <summary> 越界回收管理器 </summary>
    protected RecycleControl recycleControl;

    /// <summary> 越界溢出发射器 </summary>
    protected OverflowControl overflowControl;

    /// <summary> 发射点特效管理脚本 /// </summary>
    protected AShotPointEffectControllerBase shotPointEffectController;

    /// <summary> 30个小弟的坐标 /// </summary>
    protected YoungerBrotherControl _youngerBrotherControl;
    
    protected abstract CampType CampType { get; }
    protected bool mirror => CampType == CampType.红;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        _root = UIUtility.Control("fire", Trans);
        richManControl = UIUtility.CreatePageNoClone<RichManPointControl>(Trans, "role");
        recycleControl = UIUtility.CreatePageNoClone<RecycleControl>(Trans, "recycle");
        overflowControl = UIUtility.CreatePageNoClone<OverflowControl>(Trans, "line");
        _youngerBrotherControl = UIUtility.CreatePageNoClone<YoungerBrotherControl>(Trans, "youngerBrother");
    }

    public override void Refresh()
    {
        base.Refresh();

        resultPoint.IsActive = true;
        overflowControl.IsActive = true;
        
        richManControl.IsActive = true;
        richManControl.Set(mirror);
        
        recycleControl.IsActive = true;
        recycleControl.Set(mirror, overflowControl);

        shotPointEffectController.IsActive = true;
        
        _youngerBrotherControl.IsActive = true;
        _youngerBrotherControl.Set(mirror);
        //获得所有发射器(炮口)
        var arr = TFireDataManager.Instance.GetAllItem();
        for (int i = 0; i < arr.Length; i++)
        {
            var muzzle = GameObjectFactory.GetOrCreate<MuzzleControl>();
            muzzle.Attach(_root);
            muzzle.OnInit(arr[i], mirror);
            muzzleDict.Add(arr[i].Id, muzzle);
        }
    }

    public override void Close()
    {
        base.Close();
        
        //回收所有炮口
        foreach (var kv in muzzleDict)
            GameObjectFactory.Recycle(kv.Value);
        muzzleDict.Clear();
        
        richManControl.IsActive = false;
        resultPoint.IsActive = false;
        recycleControl.IsActive = false;
        overflowControl.IsActive = false;
        shotPointEffectController.IsActive = false;
        _youngerBrotherControl.IsActive = false; 
    }

    public Transform GetRichManTrans(int i) => richManControl?.GetRichManTrans(i);  
    public Transform GetRichManHeadTrans(int i) => richManControl?.GetRichManHeadTrans(i);
    public Transform GetReslutTrans() => resultPoint?.Trans;
    
    public Transform GetYoungerBrotherPoint(int i) => _youngerBrotherControl?.GetYongBrotherPoint(i);
    public void OnBossHpValueChanged(Action<int, int> callback) => resultPoint?.OnValueChanged(callback);
    public T GetReslut<T>() where T : AResultPointBase => resultPoint as T;
    public RichManPointControl GetRichMan() => richManControl;
    public YoungerBrotherControl GetYoungerBrother() => _youngerBrotherControl;

    
    /// <summary> 获取头像坐标,玩家的坐下索引是从0开始的/// </summary>
    public Transform GetHeadTransform(int sitIndex)
    {
        if (sitIndex < 3)
            return GetRichManHeadTrans(sitIndex + 1);
        else
            return GetYoungerBrotherPoint(sitIndex - 3);
    }

    /// <summary> 游戏结束,清除一些数据 /// </summary>
    private void GameIsOver(CampType camp)
    {
        overflowControl.ClearOverflowBall();
    }
    
    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener<CampType, int>(BattleEvent.Battle_AddShield, OnAddShield);
        EventMgr.AddEventListener<CampType>(BattleEvent.Battle_GameIsOver, GameIsOver);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener<CampType, int>(BattleEvent.Battle_AddShield, OnAddShield);
        EventMgr.RemoveEventListener<CampType>(BattleEvent.Battle_GameIsOver, GameIsOver);
    }

    public void OnFireAcceleSpeed(int index, float acceleTime)
    {
        if (muzzleDict.TryGetValue(index, out var fire))
        {
            fire.SetAccele(acceleTime);
        }
    }

    private void OnAddShield(CampType camp, int count)
    {
        if (camp == this.CampType)
        {
            resultPoint.AddShield(count);
        }
    }
}
