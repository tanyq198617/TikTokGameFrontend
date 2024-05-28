public class BlueNianYaQiuRuChangTimeLine : ATimeLineBase
{
    protected override TimeLineEnum timeLineEnum => TimeLineEnum.Ball;

    protected override void PlayEnableAudio()
    {
        AudioMgr.Instance.PlaySoundName(19);
    }

    public override void Recycle()
    {
        BallTimeLineFactory.Recycle(this);
    }
}