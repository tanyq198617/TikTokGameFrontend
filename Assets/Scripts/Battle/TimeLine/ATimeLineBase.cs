using DG.Tweening;
using SprotoType;
using UnityEngine;
using UnityEngine.Playables;

/// <summary> timeline特效基类 /// </summary>
public abstract class ATimeLineBase : APoolGameObjectBase
{
    /// <summary> timeline组件 /// </summary>
    private PlayableDirector _playableDirector;

    /// <summary> timeline上的专属相机/// </summary>
    private Camera _camera;

    /// <summary> tween /// </summary>
    private Sequence tween;

    protected abstract TimeLineEnum timeLineEnum { get; }

    /// <summary> 回收timeline特效 /// </summary>
    public abstract void Recycle();

    /// <summary> 播放入场音效 /// </summary>
    protected virtual void PlayEnableAudio() {}

    public override void setObj(GameObject gameObject)
    {
        base.setObj(gameObject);
        _playableDirector = UIUtility.GetComponent<PlayableDirector>(Trans, "TimeLine");
        _camera = UIUtility.GetComponent<Camera>(Trans, "Camera_3D");
    }
    
    /// <summary> 计时回收TimeLine /// </summary>
    private void PlayTween()
    {
        tween = DOTween.Sequence();
        double _effectTime = _playableDirector ? _playableDirector.duration : 1;
        tween.AppendInterval((float)_effectTime);
        tween.OnComplete(Recycle);
    }

    #region 生命周期

    public override void OnEnable()
    {
        base.OnEnable();
        if (_camera)
            CameraMgr.Instance.AddCameraStack(_camera, timeLineEnum);
        PlayTween();
        PlayEnableAudio();
    }

    public override void OnDisable()
    {
        if (_camera)
            CameraMgr.Instance.RemoveCameraStack(_camera, timeLineEnum);
        tween?.Kill(false);
        tween = null;
        base.OnDisable();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _camera = null;
        _playableDirector = null;
    }

    #endregion
}