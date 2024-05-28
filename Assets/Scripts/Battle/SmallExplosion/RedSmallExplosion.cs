using UnityEngine;

public class RedSmallExplosion : SmallExplosionBase
{
    public override void Init(CampType camp)
    {
        isRed = true;
        attackLayerMask = Layer.BlueBall;
        bossPoint = BlueCampController.Instance.Trans.position;
        myBossPoint =  RedCampController.Instance.Trans.position;
        checkRaystartPoint = new Vector3(10, myBossPoint.y+0.5f, myBossPoint.z);
        base.Init(camp);
    }

    /// <summary> 添加拖尾特效 /// </summary>
    protected override void GetExplosionTrailer()
    {
        traileffect =  GameObjectFactory.GetOrCreate<RedSmallExplosionTrail>();
    }
    
    protected override void GetExplosionClass()
    {
        effect = GameObjectFactory.GetOrCreate<RedSmallExplosionEffect>();
    }
    protected override void RecycleClass()
    {
        base.RecycleClass();
        ClassFactory.Recycle(this);
    }
}