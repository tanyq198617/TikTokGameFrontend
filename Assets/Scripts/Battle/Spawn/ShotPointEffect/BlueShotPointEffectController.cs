public class BlueShotPointEffectController : AShotPointEffectControllerBase
{
    protected override int campType => CampType.蓝.ToInt();
    protected override void AddShotPointEffect(int camp, int ballType, int ballCount)
    {
        base.AddShotPointEffect(camp, ballType, ballCount);
        if (isPlayEffectIng)
            return;
        if (camp == campType)
        {
            BlueShotPointEffect effect = GameObjectFactory.GetOrCreate<BlueShotPointEffect>();
            effect.OnInit(transformPoint, 0.1f);
            effect.backAction = EffectBackAction;
            isPlayEffectIng = true;
        }
    }
}