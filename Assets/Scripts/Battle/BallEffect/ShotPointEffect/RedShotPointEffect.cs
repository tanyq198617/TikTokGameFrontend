/// <summary> 红色发射点的特效 /// </summary>
public class RedShotPointEffect : ABallEffectBase
{
    public override float effectTime => 0.1f;
    public override void Recycle()
    {
        GameObjectFactory.Recycle(this);
    }
}