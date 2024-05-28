/// <summary> 红方神龙炸弹循环timeling /// </summary>
public class RedFairyDragonXunHuanEffect : AFairyDragonEffectBase
{
    public override void Recycle() =>FairyDragonEffectFactory.Recycle(this);
}