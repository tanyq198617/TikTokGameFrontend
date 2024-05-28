public class RedShotPointEffectController : AShotPointEffectControllerBase
{
    protected override int campType => CampType.红.ToInt();
    protected override void AddShotPointEffect(int camp, int ballType, int ballCount)
    {
        base.AddShotPointEffect(camp, ballType, ballCount);
        if (isPlayEffectIng)
            return;
        if (camp == campType)
        {
            RedShotPointEffect effect = GameObjectFactory.GetOrCreate<RedShotPointEffect>();
            effect.OnInit(transformPoint, 0.1f);
            effect.backAction = EffectBackAction;
            isPlayEffectIng = true;
        }
    }
}