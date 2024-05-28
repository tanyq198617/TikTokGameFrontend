using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 大哥数据
/// </summary>
public class RichManItem : AItemPageBase
{
    protected CampType Camp;
    private ARichManControlBase richManControlBase;
    public Transform headTran;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        headTran = UIUtility.Control("heaTran", Trans);
    }

    public void Set(CampType camp)
    {
        this.Camp = camp;
        CreationDaGeMoXing();
    }

    /// <summary> 预加载大哥模型,名字展示 /// </summary>
    private void CreationDaGeMoXing()
    {
        if (Camp == CampType.红)
            richManControlBase = GameObjectFactory.GetOrCreate<RedRichManControl>();
        else
            richManControlBase = GameObjectFactory.GetOrCreate<BlueRichManControl>();
        
        richManControlBase.OnInit(Trans.position);
        richManControlBase.IsActive = false;
    }

    public void SetPlayerInfo(PlayerInfo info)
    {
        if (info != null)
        {
            richManControlBase.IsActive = true;
        }
        else
        {
            richManControlBase.IsActive = false;
        }
    }

    public void PlayerAnimator(int type)
    {
        if (richManControlBase.IsActive)
            richManControlBase?.PlayerAnimator(type);
    }
    
    
    public override void Close()
    {
        base.Close();
        richManControlBase.Recycle();
        if (Camp == CampType.红)
             GameObjectFactory.Recycle<RedRichManControl>(richManControlBase as RedRichManControl);
        else
            GameObjectFactory.Recycle<BlueRichManControl>(richManControlBase as BlueRichManControl);
    }
}
