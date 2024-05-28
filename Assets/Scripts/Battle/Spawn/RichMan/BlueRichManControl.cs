public class BlueRichManControl : ARichManControlBase
{
    protected override CampType camp => CampType.蓝;
    public override void Recycle()
    {
        GameObjectFactory.Recycle(this);
    }
}