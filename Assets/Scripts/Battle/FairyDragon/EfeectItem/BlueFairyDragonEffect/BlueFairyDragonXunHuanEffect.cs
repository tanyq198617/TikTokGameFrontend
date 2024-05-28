
/// <summary> 蓝方神龙炸弹循环timeling /// </summary>
public class BlueFairyDragonXunHuanEffect : AFairyDragonEffectBase
{
    public override void Recycle() =>FairyDragonEffectFactory.Recycle(this);
}