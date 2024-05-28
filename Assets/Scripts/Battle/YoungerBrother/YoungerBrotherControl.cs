using System.Collections.Generic;
using HotUpdateScripts;
using UnityEngine;

/// <summary> 30个小弟的坐标控制中心 /// </summary>
public class YoungerBrotherControl : AItemPageBase
{
    protected Dictionary<int, Transform> youngerBrotherDic;

    private CampType Camp;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        youngerBrotherDic = new Dictionary<int, Transform>();
        for (int i = 1; i <= TGlobalDataManager.YoungerBrotherHeadCount; i++)
        {
            youngerBrotherDic[i] = UIUtility.Control(i.ToString(), Trans);
        }
    }

    /// <summary>
    /// 设置阵营
    /// </summary>
    public void Set(bool isRed)
    {
        Camp = isRed ? CampType.红 : CampType.蓝;
    }
    
    /// <summary> 获取小弟坐标 /// </summary>
    public Transform? GetYongBrotherPoint(int rank)
    {
        if (youngerBrotherDic.TryGetValue(rank, out var value))
        {
            return value;
        }
        return null;
    }

    //检测轴是否大于1,场景旋转掉头,true大于1
    public void SetPointOffset(bool offset)
    {
        if (offset)
            Trans.localPosition = new Vector3(0,0,-3);
        else
            Trans.localPosition = Vector3.zero;
    }

    public override void Close()
    {
        base.Close();
        Trans.localPosition = Vector3.zero;
    }
}