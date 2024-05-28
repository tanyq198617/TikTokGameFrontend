using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalKey 
{
    public const int DefaultVolume = 1;
    public const float VolumeRatio = 0.3333f;

    public const string AgreeKey = "Login_Agree";   //同意协议
    public const string RiskKey = "Risk_Agree";     //风险协议
    public const string SoundMuteKey = "SoundMuteKey";      //声音
    public const string SoundVolumeKey = "SoundVolumeKey";  //声音
    public const string MusicMuteKey = "MusicMuteKey";      //背景音
    public const string MusicVolumeKey = "MusicVolumeKey";  //背景音
    public const string VoiceMuteKey = "VoiceMuteKey";      //语音
    public const string VoiceVolumeKey = "VoiceVolumeKey";  //语音

    /// <summary> 登录 </summary>
    public static readonly string AccoutName = "AccoutName";               //登录名字
    public static readonly string PhoneNumber = "PhoneNumber";             //手机号
    public static readonly string OpenID = "Accout_OpenID";                //登录名字
    public static readonly string PrivacyRead = "PrivacyRead";             //协议阅读
    public static readonly string NotFirstLogin = "NotFirstLogin";         //非首次登录

    /// <summary> 音频设置 </summary>
    public static readonly string BackGroudSound = "BackGroudSound";        //背景音量
    public static readonly string GameSound = "GameSound";                  //游戏音量
    public static readonly string VoiceSound = "VoiceSound";                //语音音量
    public static readonly string BackGroudSoundTog = "BackGroudSoundTog";  //背景音量开关
    public static readonly string GameSoundTog = "GameSoundTog";            //游戏音量开关
    public static readonly string VoiceSoundTog = "VoiceSoundTog";          //语音音量开关

    /// <summary> 游戏画质 </summary>
    public static readonly string GameQuality = "GameQuality";
}
