using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 组合球
/// 能力药丸修改成组合球（羊羊精灵+小爆破）
/// </summary>
public class SmallExplosionController : Singleton<SmallExplosionController>
{

    private readonly List<SmallExplosionBase> _smallExplosionHashSet = new List<SmallExplosionBase>();

    public void OnInit()
    {
        _smallExplosionHashSet.Clear();
    }
    
    /// <summary> 添加一个小爆炸数据到队列里面 /// </summary>
    public void AddSmallExplosionData(PlayerInfo info, long gift_num)
    {
        for (int i = 0; i < gift_num; i++)
        {
            var _smallExplosion = AddSmallExplosionBase(info);
            _smallExplosion.AddPlayerInfo(info, BackAction);
            _smallExplosionHashSet.Add(_smallExplosion);
        }
    }

    /// <summary> 红蓝方小爆炸的控制脚本 /// </summary>
    private SmallExplosionBase AddSmallExplosionBase(PlayerInfo info)
    {
        if (info.campType == CampType.红)
            return ClassFactory.GetOrCreate<RedSmallExplosion>();
        else
            return ClassFactory.GetOrCreate<BlueSmallExplosion>();
    }
    
    /// <summary> 小爆炸播完的回调 /// </summary>
    private void BackAction(SmallExplosionBase _class)
    {
        _smallExplosionHashSet.Remove(_class);
    }

    /// <summary>清空未播数据/// </summary>
    public void Recycle()
    {
        var list = _smallExplosionHashSet;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            list[i].Recycle();
        }
        list.Clear();
        _smallExplosionHashSet.Clear();
    }
}