public class BlueNianYaQiuJinJieTimeLine : ATimeLineBase
{
    protected override TimeLineEnum timeLineEnum => TimeLineEnum.Ball;

    protected override void PlayEnableAudio()
    {
        AudioMgr.Instance.PlaySoundName(20);
    }
    public override void Recycle()
    {
        BallTimeLineFactory.Recycle(this);
    }
}