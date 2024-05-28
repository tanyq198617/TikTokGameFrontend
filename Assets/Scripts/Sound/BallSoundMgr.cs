using HotUpdateScripts;

public class BallSoundMgr : Singleton<BallSoundMgr>
{
    //药丸球音效上次播放时间
    private float PillBallTime;
    //碾压球音效上次播放时间
    private float GrindBallTime;

    /// <summary> 药丸球播放音效 /// </summary>
    public void PlayPillBallBegin(int audioIndex)
    {
        if(CityGlobal.Instance.IsOver)
            return;
        if (MathUtility.IsOutTime(ref PillBallTime, TGlobalDataManager.PillBallSoundInterval))
        {
            PillBallTime = 0;
            AudioMgr.Instance.PlaySoundName(audioIndex);
        }
    }

    /// <summary> 碾压球播放音效 /// </summary>
    public void PlayGrindBallBegin(int audioIndex)
    {
        if(CityGlobal.Instance.IsOver)
            return;
        if (MathUtility.IsOutTime(ref PillBallTime, TGlobalDataManager.GrindBallSoundInterval))
        {
            GrindBallTime = 0;
            AudioMgr.Instance.PlaySoundName(audioIndex);
        }
    }
    
    public void Recycle()
    {
        PillBallTime = 0;
        GrindBallTime = 0;
    }
    
}