using System.Collections.Generic;
using System.Linq;

/// <summary> 雷霆一击控制脚本 /// </summary>
public class ThunderboltController : Singleton<ThunderboltController>
{
    private readonly HashSet<ThunderboltBase> _smallExplosionHashSet = new HashSet<ThunderboltBase>();

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
    private ThunderboltBase AddSmallExplosionBase(PlayerInfo info)
    {
        if (info.campType == CampType.红)
            return ClassFactory.GetOrCreate<RedThunderbolt>();
        else
            return ClassFactory.GetOrCreate<BlueThunderbolt>();
    }

    /// <summary> 小爆炸播完的回调 /// </summary>
    private void BackAction(ThunderboltBase _class)
    {
        _smallExplosionHashSet.Remove(_class);
    }

    /// <summary>清空未播数据/// </summary>
    public void Recycle()
    {
        /*redSmallExplosion.Recycle();
        blueSmallExplosion.Recycle();
        redSmallExplosion = null;
        blueSmallExplosion = null;*/

        var list = _smallExplosionHashSet.ToList();
        for (int i = list.Count - 1; i >= 0; i--)
        {
            list[i].Recycle();
        }
        list.Clear();
        _smallExplosionHashSet.Clear();
    }
}