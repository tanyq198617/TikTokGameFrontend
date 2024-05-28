using DG.Tweening;

/// <summary> 游戏结束的过场动画,动画播完才进行排行榜展示 /// </summary>
public class GameOverInterludeAnimation:Singleton<GameOverInterludeAnimation>
{

    private Sequence tween;
    private CampType winCampType;
    /// <summary> 开始播放结束过场 /// </summary>
    public void OnBegin()
    { 
        winCampType = LevelGlobal.Instance.IsRedWin ? CampType.红 : CampType.蓝;
        PlayBallDieTween();
        tween = DOTween.Sequence();
        tween.AppendInterval(0.5f);
        tween.AppendCallback(PlayRichManRuChangAnimator);
        tween.AppendInterval(1.5f);
        tween.AppendCallback(PlayTimeLine);
        tween.AppendInterval(1.5f);
        tween.AppendCallback(PlayRichManJiFeiAnimator);
        tween.AppendInterval(2);
        tween.AppendCallback(InterludeAnimationEnd);
        tween.Play();
    }

    /// <summary> 播放场上球的死亡tween动画,动画时间1秒 /// </summary>
    private void PlayBallDieTween()
    {
        BallFactory.PlayBallDieTweenAll();
    }
    
    /// <summary> 播放获胜方三个大哥的动作 /// </summary>
    private void PlayRichManRuChangAnimator()
    {
        if (winCampType == CampType.红)
            RedCampController.Instance.GetRichMan().PlayRichManAnimator(1);
        else
            BlueCampController.Instance.GetRichMan().PlayRichManAnimator(1);
    }

    /// <summary> 播放timeLine /// </summary>
    private void PlayTimeLine()
    {
        AudioMgr.Instance.PlaySoundName(17);
        if (winCampType == CampType.红)
            GameObjectFactory.GetOrCreate<RedInterludeAnimationEffect>();
        else
            GameObjectFactory.GetOrCreate<BlueInterludeAnimationEffect>();
    }

    /// <summary> 播放输的一方三个大哥的击飞动画 /// </summary>
    private void PlayRichManJiFeiAnimator()
    {
        if (winCampType == CampType.红)
        {
            BlueCampController.Instance.GetRichMan().PlayRichManAnimator(2);
            BlueCampController.Instance.GetReslut<BlueResultPoint>().IsActive = false;
            MapMgr.Instance.gameobject_xuetiao_lan.SetActiveEX(false);
            
        }
        else
        {
            RedCampController.Instance.GetRichMan().PlayRichManAnimator(2);
            MapMgr.Instance.gameobject_xuetiao_red.SetActiveEX(false);
            RedCampController.Instance.GetReslut<RedResultPoint>().IsActive = false;
        }
    }

    /// <summary>
    /// 过场动画结束,回到状态机进行结算
    /// </summary>
    private void InterludeAnimationEnd()
    {
        tween?.Kill(false);
        tween = null;
        EventMgr.Dispatch(BattleEvent.Battle_GameOverAnimatorEnd);
    }
    
}