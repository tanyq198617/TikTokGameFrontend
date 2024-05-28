using UnityEngine;

/// <summary> 红方小爆炸前的拖尾 /// </summary>
public class RedSmallExplosionTrail : SmallExplosionTrailBase
{
    public override float effectTime => 1;
 
    protected override Vector3 initPoint => RedCampController.Instance.Trans.position;

    public override void Recycle()
    {
        GameObjectFactory.Recycle(this);
    }
}