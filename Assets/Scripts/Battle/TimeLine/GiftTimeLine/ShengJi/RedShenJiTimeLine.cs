/// <summary> 红方升级timeline /// </summary>
public class RedShenJiTimeLine : ATimeLineBase
{
    protected override TimeLineEnum timeLineEnum => TimeLineEnum.Gift;
    public override void OnEnable()
    {
        base.OnEnable();
        AudioMgr.Instance.PlaySoundName(7);
    }
    public override void Recycle()
    {
        GiftTimeLineFactory.Recycle(this);
    }
}