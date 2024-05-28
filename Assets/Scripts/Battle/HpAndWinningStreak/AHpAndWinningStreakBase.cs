using TMPro;
using UnityEngine;

public abstract class AHpAndWinningStreakBase : AItemPageBase
{
    /// <summary> 结算点阵营 /// </summary>
    protected abstract CampType camp { get; }

    private TextMeshPro tx_hp;
    private TextMeshPro tx_WinningStreak;
    private TextMeshPro tx_shield;

    private Transform transform_WinningStreak_tx;
   
    private Vector3 WinningStreak_tx_defaultScale;
   
    private Vector3 WinningStreak_tx_Scale;
  
    private Transform object_rotation;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        tx_hp = UIUtility.GetComponent<TextMeshPro>(Trans, "tx_hp");
        tx_WinningStreak = UIUtility.GetComponent<TextMeshPro>(Trans, "tx_WinningStreak");
        tx_shield = UIUtility.GetComponent<TextMeshPro>(Trans, "tx_shield");
        object_rotation = UIUtility.Control("object_rotation", Trans);
        transform_WinningStreak_tx = tx_WinningStreak.transform;
        WinningStreak_tx_defaultScale = transform_WinningStreak_tx.localScale;
      
        WinningStreak_tx_Scale = new Vector3(WinningStreak_tx_defaultScale.x * -1, WinningStreak_tx_defaultScale.y * -1,
            WinningStreak_tx_defaultScale.z);
        
    }

    public override void Refresh()
    {
        base.Refresh();
        int MaxHP = LevelGlobal.Instance.DataBase.GetMaxHP();
        
        transform_WinningStreak_tx.localScale = WinningStreak_tx_defaultScale;
        UIUtility.Safe_UGUI(ref tx_hp, MaxHP);
        var value = PlayerModel.Instance.GetWinningTreak(camp);
        UIUtility.Safe_UGUI(ref tx_WinningStreak, $"(连胜数:{value})");
        UIUtility.Safe_UGUI(ref tx_shield, $"");
        Debuger.Log($"AHpAndWinningStreakBase:刷新连胜数;连胜数据:{value}");
    }

    public void RefreshHp(int hp, int shield)
    {
        if (hp <= 0)
            hp = 0;
        UIUtility.Safe_UGUI(ref tx_hp, hp);
        if(shield > 0)
            UIUtility.Safe_UGUI(ref tx_shield, $"(+{shield})");
        else
            UIUtility.Safe_UGUI(ref tx_shield, $"");
    }

    /// <summary> 刷新连胜数量 /// </summary>
    public void RefreshWinningTreak(CampType campType, long num)
    {
        if (campType == camp)
            UIUtility.Safe_UGUI(ref tx_WinningStreak, $"(连胜数:{num})");
    }

    /// <summary> 刷新连胜,血量文本的旋转角度,避免相机看到的字体会倒过来/// </summary>
    private void RefreshTextProRotation(bool _boo)
    {
        if (_boo)
        {
            transform_WinningStreak_tx.localScale = WinningStreak_tx_Scale;
            object_rotation.localEulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform_WinningStreak_tx.localScale = WinningStreak_tx_defaultScale;
            object_rotation.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener<CampType, long>(BattleEvent.Battle_WinningTreak_Changed, RefreshWinningTreak);
        EventMgr.AddEventListener<bool>(BattleEvent.Battle_Camera_Rotation, RefreshTextProRotation);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener<CampType, long>(BattleEvent.Battle_WinningTreak_Changed, RefreshWinningTreak);
        EventMgr.RemoveEventListener<bool>(BattleEvent.Battle_Camera_Rotation, RefreshTextProRotation);
    }
}