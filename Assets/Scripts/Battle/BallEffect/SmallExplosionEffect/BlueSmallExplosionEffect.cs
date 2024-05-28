public class BlueSmallExplosionEffect : SmallExplosionEffectBase
{
    public override float effectTime => 1;
    protected override int ballLayerMask => Layer.RedBall | 1 << Layer.RedReslut;
    protected override CampType campType => CampType.蓝;
    public override void Recycle()
    {
        GameObjectFactory.Recycle(this);
    }
}