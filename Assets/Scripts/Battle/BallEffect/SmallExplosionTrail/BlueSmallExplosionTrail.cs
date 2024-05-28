using UnityEngine;

/// <summary> 蓝方小爆炸前的拖尾 /// </summary>
public class BlueSmallExplosionTrail : SmallExplosionTrailBase
{
    public override float effectTime => 1;

    protected override Vector3 initPoint => BlueCampController.Instance.Trans.position;

    public override void Recycle()
    {
        GameObjectFactory.Recycle(this);
    }
}