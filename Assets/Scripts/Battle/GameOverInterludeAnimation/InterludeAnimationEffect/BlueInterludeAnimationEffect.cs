/// <summary> 蓝方结算动画特效 /// </summary>
public class BlueInterludeAnimationEffect : AInterludeAnimationEffectBase
{
    public override void Recycle()
    {
        GameObjectFactory.Recycle(this);
    }
}