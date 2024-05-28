using System;
using System.Collections;
using System.Collections.Generic;
using HotUpdateScripts;
using UnityEngine;
using UnityEngine.Timeline;
using YooAsset;

public class AudioMgr : MonoBehaviour
{
    public static AudioMgr Instance { get; private set; }

    /// <summary> 所有组件 </summary>
    private readonly HashSet<AudioTask> allSouces = new HashSet<AudioTask>();

    /// <summary> 未使用队列 </summary>
    private readonly Queue<AudioTask> queue = new Queue<AudioTask>();

    /// <summary> 当前使用组件 </summary>
    private readonly List<AudioTask> currentList = new List<AudioTask>();

    /// <summary> 音效 </summary>
    private readonly HashSet<AudioTask> soundSet = new HashSet<AudioTask>();

    /// <summary> 背景音 </summary>
    private readonly HashSet<AudioTask> bgmSet = new HashSet<AudioTask>();

    private float soundVolume = 1;
    private float bgmVolume = 0.75f;

    protected bool bgmMute = false;
    protected bool soundMute = false;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        soundVolume = GetSoundVolume();
        SetSoundVolume(soundVolume);
        //SetSoundLevel(GetSoundVolume());
        SetBgmLevel(GetBgmVolume());
    }

    /// <summary> 播放音效,可以多个重复播 /// </summary>
    public void PlaySoundName(string audio)
    {
        //string path = string.Format("Assets/HotUpdateResources/Audio/{0}.Ogg", audio);
        string path = PathConst.GetSoundPath(audio);
        AudioClip clip = YooAssets.LoadAssetSync<AudioClip>(path).GetAssetObject<AudioClip>();
        PlaySound(clip);
    }

    /// <summary> 播放音效,可以多个重复播 /// </summary>
    public void PlaySoundName(int audioIndex)
    {
        if (audioIndex == 0)
            return;
        
        TAudioData data = TAudioDataManager.Instance.GetItem(audioIndex);
        if (data == null)
        {
            return;
        }
        string path = PathConst.GetSoundPath(data.audioName);
        AudioClip clip = YooAssets.LoadAssetSync<AudioClip>(path).GetAssetObject<AudioClip>();
        PlaySound(clip);
    }
    
    /// <summary> 播放音效,可以多个重复播 /// </summary>
    public AudioTask PlayBackSoundName(int audioIndex,bool isLoop)
    {
        if (audioIndex == 0)
            return null;
        
        TAudioData data = TAudioDataManager.Instance.GetItem(audioIndex);
        if (data == null)
            return null;
        
        string path = PathConst.GetSoundPath(data.audioName);
        AudioClip clip = YooAssets.LoadAssetSync<AudioClip>(path).GetAssetObject<AudioClip>();
        return PlayBackground(clip,isLoop);
    }
    
    public AudioTask PlaySound(AudioClip audioClip, Action finish = null) => PlaySound(audioClip, soundVolume, finish);
    public AudioTask PlayBackground(AudioClip audioClip, bool isLoop = false, Action finish = null) => PlayBackground(audioClip, bgmVolume, isLoop, finish);

    public AudioTask PlaySound(AudioClip audioClip, float volume, Action finish = null)
    {
        AudioTask task = GetOrCreate();
        task.Play(audioClip, volume, false, finish);
        task.SetMute(GetSoundMute());
        currentList.Add(task);
        soundSet.Add(task);
        return task;
    }

    public AudioTask PlayBackground(AudioClip audioClip, float volume, bool isLoop, Action finish = null)
    {
        StopBgm();
        AudioTask task = GetOrCreate();
        task.Play(audioClip, volume, isLoop, finish);
        task.SetMute(GetBgmMute());
        currentList.Add(task);
        bgmSet.Add(task);
        return task;
    }

    public void StopSound()
    {
        foreach (var item in soundSet)
            item.IsEnd = true;
    }

    public void StopBgm()
    {
        foreach (var item in bgmSet)
            item.IsEnd = true;
    }

    /// <summary> 针对弹幕有修改 /// </summary>
    public void SetSoundVolume(float volume)
    {
        foreach (var item in soundSet)
            item.SetVolume(volume);
        soundVolume = volume;
        LocalDataMgr.SetFloat(LocalKey.SoundVolumeKey, volume);
    }

    public void SetBgmVolume(int volume)
    {
        foreach (var item in bgmSet)
            item.SetVolume(volume * LocalKey.VolumeRatio);
        LocalDataMgr.SetInt(LocalKey.MusicVolumeKey, volume);
    }

    public void SetSoundMute(bool mute)
    {
        foreach (var item in soundSet)
            item.SetMute(mute);
        LocalDataMgr.SetBool(LocalKey.SoundMuteKey, mute);
    }

    public void SetBgmMute(bool mute)
    {
        foreach (var item in bgmSet)
            item.SetMute(mute);
        LocalDataMgr.SetBool(LocalKey.MusicMuteKey, mute);
    }

    public void SetBgmLevel(int level)
    {
        SetBgmVolume(level);
        SetBgmMute(level == 0);
    }

    public void SetSoundLevel(int level)
    {
        SetSoundVolume(level);
        SetSoundMute(level == 0);
    }

    public void SetVoiceLevel(int level)
    {
        LocalDataMgr.SetInt(LocalKey.VoiceVolumeKey, level);
        LocalDataMgr.GetBool(LocalKey.VoiceMuteKey, level == 0);
    }

    public bool GetBgmMute() => LocalDataMgr.GetBool(LocalKey.MusicMuteKey, false);
    public bool GetSoundMute() => LocalDataMgr.GetBool(LocalKey.SoundMuteKey, false);
    public bool GetVoicedMute() => LocalDataMgr.GetBool(LocalKey.VoiceMuteKey, false);
    public int GetBgmVolume() => LocalDataMgr.GetInt(LocalKey.MusicVolumeKey, LocalKey.DefaultVolume);
    public float GetSoundVolume() => LocalDataMgr.GetFloat(LocalKey.SoundVolumeKey, LocalKey.DefaultVolume);
    public int GetVoiceVolume() => LocalDataMgr.GetInt(LocalKey.VoiceVolumeKey, LocalKey.DefaultVolume);

    public bool IsPlaying(string name)
    {
        for (int i = currentList.Count - 1; i >= 0; i--)
        {
            if (currentList[i] != null && currentList[i].IsPlaying(name))
                return true;
        }
        return false;
    }

    public AudioTask GetOrCreate()
    {
        AudioTask task = null;
        if (queue.Count <= 0)
        {
            task = ClassFactory.GetOrCreate<AudioTask>();
            task.Create(this.transform);
            allSouces.Add(task);
        }
        else
            task = queue.Dequeue();
        return task;
    }

    public void Update()
    {
        if (currentList.Count != 0)
        {
            AudioTask action = null;
            for (int i = currentList.Count - 1; i >= 0; i--)
            {
                if (i < currentList.Count)
                {
                    action = currentList[i];
                    if (action != null) action.Update();
                    if (action != null && action.IsEnd)
                        Recycle(action);
                }
            }
        }
    }

    private void Recycle(AudioTask task)
    {
        if (task == null) return;
        currentList.Remove(task);
        task.Clear();
        queue.Enqueue(task);
    }

    private void OnDisable()
    {
        foreach (var item in allSouces)
        {
            item.Destory();
            ClassFactory.Recycle(item);
        }
        allSouces.Clear();
    }
}
