using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 声音管理器
/// </summary>
public class SoundMgr : Singleton<SoundMgr>
{
    private Dictionary<int, AudioClip> audios = new Dictionary<int, AudioClip>();

    //private Dictionary<string, LoadTask> loadTaskDict = new Dictionary<string, LoadTask>();


    public void CrossFadeSound(string name, Action callback = null)
    {
        //if (string.IsNullOrEmpty(name)) return;
        //if (!loadTaskDict.TryGetValue(name, out var task))
        //{
        //    string path = PathConst.GetSoundPath(name);
        //    task = LoadMgr.GetLoadTask(path);
        //    loadTaskDict.Add(name, task);
        //}
        //var clip = task.Asset as AudioClip;
        //if (clip == null) return;
        //AudioMgr.Instance.PlaySound(clip, callback);
    }

    public void CrossFadeBGM(string name, bool isLoop = true, Action callback = null)
    {
        //if (string.IsNullOrEmpty(name))
        //    return;
        //if (!loadTaskDict.TryGetValue(name, out var task))
        //{
        //    string path = PathConst.GetMusicPath(name);
        //    task = LoadMgr.GetLoadTask(path);
        //    loadTaskDict.Add(name, task);
        //}
        //var clip = task.Asset as AudioClip;
        //if (clip == null) return;
        //AudioMgr.Instance.PlayBackground(clip, isLoop, callback);
    }

    public bool IsPlaying(int id)
    {
        //if (!audios.TryGetValue(id, out var clip))
            return false;
        //return BGMManager.Instance.IsPlaying(clip.name);
    }

    public void BGMStop()
    {
        AudioMgr.Instance.StopBgm();
    }

    public void Default_BGM()
    {
    }

    public void PlaySound(int id)
    {
    }

    public void MuteBGM()
    {
        SetBGMVolume(0);
    }

    public void NoMuteBGM()
    {
    }

    public void SetBGMVolume(float volume)
    {
        //BGMManager.Instance.ChangeBaseVolume(volume * 0.01f);
    }


    public void MuteSound()
    {
        SetSoundVolume(0);
    }

    public void NoMuteSound()
    {
    }

    public void SetSoundVolume(float volume)
    {
    }
}