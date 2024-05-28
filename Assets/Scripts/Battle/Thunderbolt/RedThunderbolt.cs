public class RedThunderbolt : ThunderboltBase
{
    public override void Init(CampType camp)
    {
        isRed = true;
        attackLayerMask = Layer.BlueBall;
        bossPoint = BlueCampController.Instance.Trans.position;
        base.Init(camp);
    }

    protected override void GetExplosionClass()
    {
        effect = GameObjectFactory.GetOrCreate<RedThunderboltEffect>();
    }
}