using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTask
{
    public AudioSource Source;

    private GameObject m_gameobj;

    private float _runTime = 0;
    private float _maxFrame = 0;
    private bool isLoop = false;
    private Action finish = null;

    public bool IsEnd = false;

    public bool IsPlaying(string name) => !IsEnd && Source != null && Source.clip != null && Source.clip.name.Equals(name);

    public void Create(Transform transform)
    {
        if (m_gameobj == null)
            m_gameobj = new GameObject(nameof(AudioSource), typeof(AudioSource));
        UIUtility.Attach(transform, m_gameobj);
        Source = m_gameobj.GetComponent<AudioSource>();
    }

    public void Update()
    {
        if (isLoop) return;
        if (!IsEnd && MathUtility.IsOutTime(ref _runTime, _maxFrame))
        {
            this.finish?.Invoke();
            this.IsEnd = true;
        }
    }

    public void Play(AudioClip audioClip, float volume, bool isLoop = false, Action finish = null)
    {
        Source.clip = audioClip;
        Source.Play();
        Source.volume = volume;
        Source.loop = isLoop;
        this._maxFrame = audioClip.length;
        this._runTime = 0;
        this.IsEnd = false;
        this.isLoop = isLoop;
        this.finish = finish;
    }

    public void SetVolume(float volume)
    {
        if (Source != null)
            Source.volume = volume;
    }

    public void SetMute(bool mute)
    {
        if (Source != null)
            Source.mute = mute;
    }

    public void Clear()
    {
        if (Source != null)
            Source.clip = null;
        this._runTime = 0;
        this._maxFrame = 0;
        this.IsEnd = false;
    }

    public void Destory()
    {
        if (Source != null)
        {
            Source.Stop();
            Source.Destroy();
            Source = null;
        }
        m_gameobj?.Destroy();
        m_gameobj = null;
    }
}
