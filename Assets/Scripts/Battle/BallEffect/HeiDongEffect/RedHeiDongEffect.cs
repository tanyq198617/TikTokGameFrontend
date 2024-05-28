public class RedHeiDongEffect : AHeiDongEffectBase
{
    public override float effectTime => 1;
    public override void Recycle() => BallEffectFactory.Recycle(this);
}