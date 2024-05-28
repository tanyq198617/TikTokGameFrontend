using UnityEngine;

public class BlueSmallExplosion : SmallExplosionBase
{
    public override void Init(CampType camp)
    {
        isRed = false;
        attackLayerMask = Layer.RedBall;
        bossPoint = RedCampController.Instance.Trans.position;
        myBossPoint =  BlueCampController.Instance.Trans.position;
        checkRaystartPoint = new Vector3(10, myBossPoint.y+ 0.5f, myBossPoint.z);
        base.Init(camp);
    }

    protected override void GetExplosionTrailer()
    {
        traileffect =  GameObjectFactory.GetOrCreate<BlueSmallExplosionTrail>();
    }
    
    protected override void GetExplosionClass()
    {
        effect = GameObjectFactory.GetOrCreate<BlueSmallExplosionEffect>();
    }
    
    protected override void RecycleClass()
    {
        base.RecycleClass();
        ClassFactory.Recycle(this);
    }
}