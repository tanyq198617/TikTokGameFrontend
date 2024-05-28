/// <summary> 红方结算动画特效 /// </summary>
public class RedInterludeAnimationEffect : AInterludeAnimationEffectBase
{
    public override void Recycle()
    {
        GameObjectFactory.Recycle(this);
    }
}