/// <summary> 蓝方进阶特效 /// </summary>
public class BlueJinJIeEffect : ABallEffectBase
{
    public override float effectTime => 1;

    public override void Recycle()
    {
        BallEffectFactory.Recycle(this);
    }
}