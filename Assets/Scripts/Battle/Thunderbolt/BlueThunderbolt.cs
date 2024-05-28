public class BlueThunderbolt : ThunderboltBase
{
    public override void Init(CampType camp)
    {
        isRed = false;
        attackLayerMask = Layer.RedBall;
        bossPoint = RedCampController.Instance.Trans.position;
        base.Init(camp);
    }
    protected override void GetExplosionClass()
    {
        effect = GameObjectFactory.GetOrCreate<BlueThunderboltEffect>();
    }
}