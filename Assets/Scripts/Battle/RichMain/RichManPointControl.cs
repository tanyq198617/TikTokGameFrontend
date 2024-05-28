using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 有钱大哥表现层
/// </summary>
public class RichManPointControl : AItemPageBase
{
    /// <summary> 大哥集合, key=位置ID, value=大哥数据 </summary>
    private readonly Dictionary<int, RichManItem> manDict = new Dictionary<int, RichManItem>();

    /// <summary> 阵营 </summary>
    protected CampType Camp;

    private int campType;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        for (int i = 1; i <= 3; i++)
        {
            var item = UIUtility.CreatePageNoClone<RichManItem>(Trans, $"{i}");
            manDict.Add(i, item);
        }
    }

    /// <summary>
    /// 设置阵营
    /// </summary>
    public void Set(bool isRed)
    {
        Camp = isRed ? CampType.红 : CampType.蓝;
        foreach (var kv in manDict)
            kv.Value.Set(Camp);
        campType = Camp.ToInt();
    }

    public Transform GetRichManTrans(int i) => GetItem(i)?.Trans;

    public Transform GetRichManHeadTrans(int i) => GetItem(i)?.headTran;
    public RichManItem GetItem(int i)
    {
        manDict.TryGetValue(i, out var item);
        return item;
    }

    private void ClearRichMan()
    {
        foreach (var kv in manDict)
            kv.Value.IsActive = false;
    }

    public override void Close()
    {
        base.Close();
        ClearRichMan();
    }

    public override void Destory()
    {
        ClearRichMan();
        manDict.Clear();
        base.Destory();
    }
    
    /// <summary> 刷新三个大哥的显示模型,大哥名字 /// </summary>
    public void RefreshRichMan(CampType _type, List<PlayerInfo> infos)
    {
        if (_type == Camp)
        {
            for (int i = 1; i <= 3; i++)
            {
                if (i <= infos.Count)
                    manDict[i].SetPlayerInfo(infos[i - 1]);
                else
                    manDict[i].SetPlayerInfo(null);
            }
        }
    }

    /// <summary> 播放大哥的扔球动画 /// </summary>
    private void PlayRichManAnimator(int camp,int ballType,int ballCount)
    {
        if (camp == campType)
        {
            for (int i = 1; i <= 3; i++)
            {
                manDict[i].PlayerAnimator(3);
            }
        }
    }

    public void PlayRichManAnimator(int type)
    {
        for (int i = 1; i <= 3; i++)
        {
            manDict[i].PlayerAnimator(type);
        }
    }

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener<CampType, List<PlayerInfo>>(BattleEvent.Battle_RickMan_Changed, RefreshRichMan);
        EventMgr.AddEventListener<int, int, int>(BattleEvent.Battle_Ball_ValueChanged, PlayRichManAnimator);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener<CampType, List<PlayerInfo>>(BattleEvent.Battle_RickMan_Changed, RefreshRichMan);
        EventMgr.RemoveEventListener<int, int, int>(BattleEvent.Battle_Ball_ValueChanged, PlayRichManAnimator);
    }
}