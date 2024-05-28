public class RedThunderboltEffect : ThunderboltEffectBase
{
    public override float effectTime => 1;
    protected override int ballLayerMask => Layer.BlueBall | 1 << Layer.BlueResult;
    
    protected override CampType campType => CampType.红;
    public override void Recycle()
    {
        GameObjectFactory.Recycle(this);
    }
} 