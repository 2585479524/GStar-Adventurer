using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SoundSet
{
    public bool BgMusicOn = true;
    public bool SfxOn = true;
}

public class SoundManager : Singleton<SoundManager>
{
    [Header("声音文件目录")]
    public string ResourceDir = "";
    [Header("声音设置")]
    public SoundSet Set;

    [Header("音乐音量")]
    [Range(0, 1)]
    public float MusicVolume = 1f;

    [Header("音效音量")]  
    [Range(0, 1)]
    public float SfxVolume = 1f;

    private AudioSource backgroundMusic;
    private AudioSource playerStepSfx;

    private Dictionary<string, AudioClip> audios = new Dictionary<string, AudioClip>();



    protected override void Awake()
    {
        base.Awake();
        //创建背景音Audiosource
        backgroundMusic = gameObject.AddComponent<AudioSource>();
        backgroundMusic.loop = true;

        //加载声音文件
        LoadAllAudio();
        DontDestroyOnLoad(gameObject);
    }

    private void LoadAllAudio()
    {
        //路径
        string path;
        if (string.IsNullOrEmpty(ResourceDir))
            path = "./";
        else
            path = ResourceDir + "/";

        AudioClip[] clip = Resources.LoadAll<AudioClip>(path);

        foreach (var item in clip)
        {
            audios.Add(item.name, item);
        }
    }

    /// <summary>
    /// 播放音乐
    /// </summary>
    /// <param name="clipName"></param>
    public void PlayBackgroundMusic(string clipName)
    {
        if (!Set.BgMusicOn||!audios.ContainsKey(clipName))
        {
            return;
        }

        if (backgroundMusic.isPlaying)
        {
            backgroundMusic.Stop();
        }

        backgroundMusic.loop = true;
        backgroundMusic.clip = audios[clipName];
        backgroundMusic.volume = MusicVolume;
        backgroundMusic.Play();
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="sfx"></param>
    /// <param name="pos"></param>
    public void PlaySfx(string clipName, Vector3 pos)
    {
        if (!Set.SfxOn)
        {
            return;
        }
        GameObject audioContainer = new GameObject("AudioContainer");
        audioContainer.transform.position = pos;

        AudioSource audioSource = audioContainer.AddComponent<AudioSource>();
        audioSource.clip = audios[clipName];
        audioSource.volume = SfxVolume;
        audioSource.Play();
        Destroy(audioContainer, audios[clipName].length);
    }

    public void PlayStepSfx(AudioSource stepAudiosorce,string clipName)
    {
        if (!Set.SfxOn)
        {
            return;
        }

        if (playerStepSfx == null)
        {
            playerStepSfx = stepAudiosorce;
        }
        if (playerStepSfx.isPlaying)
        {
            return;
        }
        else
        {
            playerStepSfx.clip = audios[clipName];
            playerStepSfx.volume = SfxVolume;
            playerStepSfx.Play();
        }
    }

    /// <summary>
    /// 设置背景音
    /// </summary>
    /// <param name="value"></param>
    public void SetBGMusic(bool value)
    {
        Set.BgMusicOn = value;
        if (!Set.BgMusicOn)
        {
            backgroundMusic.Stop();
        }
        else
        {
            backgroundMusic.Play();
        }
    }

    /// <summary>
    /// 设置音效
    /// </summary>
    /// <param name="value"></param>
    public void SetSFX(bool value)
    {
        Set.SfxOn = value;
    }

}
