using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = System.Random;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;

public class Sound : Singleton<Sound>
{
    public float pitchMin = -3;
    public float pitchMax = 3;
    public float volumeMin = 0;
    public float volumnMax = 1;
    protected override void Awake()
    {
        base.Awake();

        m_bgSound = this.gameObject.AddComponent<AudioSource>();
        m_bgSound.playOnAwake = false;
        m_bgSound.loop = true;

        m_effectSound = this.gameObject.AddComponent<AudioSource>();


        LoadAllAudio();

    }

    public string ResourceDir = "";

    AudioSource m_bgSound;
    AudioSource m_effectSound;

    Dictionary<string, AudioClip> m_Audios = new Dictionary<string, AudioClip>();

    //读取所有音效文件


    //音乐大小
    public float BgVolume
    {
        get { return m_bgSound.volume; }
        set { m_bgSound.volume = value; }
    }

    //音效大小
    public float EffectVolume
    {
        get { return m_effectSound.volume; }
        set { m_effectSound.volume = value; }
    }

    //播放音乐
    public void PlayBg(string audioName)
    {

        //当前正在播放的音乐文件
        string oldName;
        if (m_bgSound.clip == null)
            oldName = "";
        else
            oldName = m_bgSound.clip.name;

        if (oldName != audioName)
        {
            //音乐文件路径
            //string path;
            //if (string.IsNullOrEmpty(ResourceDir))
            //    path = audioName;
            //else
            //    path = ResourceDir + "/" + audioName;

            ////加载音乐

            //AudioClip clip = Resources.Load<AudioClip>(path);



            //音频
            if (!m_Audios.ContainsKey(audioName))
            {
                return;

            }
            AudioClip clip = m_Audios[audioName];


            //播放
            if (clip != null)
            {
                m_bgSound.clip = clip;
                m_bgSound.Play();
            }
        }
    }

    //停止音乐
    public void StopBg()
    {
        m_bgSound.Stop();
        m_bgSound.clip = null;
    }






    //播放音效
    public void PlayEffect(string audioName, bool isRandom)
    {





        //路径
        //string path;
        //if (string.IsNullOrEmpty(ResourceDir))
        //    path = audioName;
        //else
        //    path = ResourceDir + "/" + audioName;

        //音频
        //  AudioClip clip = Resources.Load<AudioClip>(path);

        if (!m_Audios.ContainsKey(audioName))
        {
            return;

        }
        AudioClip clip = m_Audios[audioName];



        // 随机化
        if (isRandom)
        {
            RandomEffect();
        }
        //播放
        m_effectSound.PlayOneShot(clip);
    }

    public void RandomEffect()
    {
        m_effectSound.pitch = UnityEngine.Random.Range(pitchMin, pitchMax);
        m_effectSound.volume = UnityEngine.Random.Range(volumeMin, volumnMax);

    }


    public void LoadAllAudio()
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
            m_Audios.Add(item.name, item);
        }
    }
}
