/// <summary> 蓝方升级timeline特效 /// </summary>
public class BlueShenJiTimeLine : ATimeLineBase
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