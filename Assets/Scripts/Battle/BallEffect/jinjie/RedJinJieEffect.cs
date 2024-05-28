/// <summary>红方进阶特效/// </summary>
public class RedJinJieEffect : ABallEffectBase
{
    public override float effectTime => 1;

    public override void Recycle()
    {
        BallEffectFactory.Recycle(this);
    }
}