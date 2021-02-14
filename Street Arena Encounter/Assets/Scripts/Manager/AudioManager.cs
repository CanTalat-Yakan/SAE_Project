using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    #region //Values
    public Audio_Info m_AudioInfo;

    AudioSource m_menuMusicSource;
    AudioSource m_mainMusicSource;
    AudioSource m_mainAmbientSource;

    float m_tmpVolume;
    #endregion

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        if (SceneManager.GetSceneByName("Menu").isLoaded 
            && !SceneManager.GetSceneByName("Main").isLoaded)
            PlayMenuMusic(0.3f);
        else
            StopMenuMusic();

        if (SceneManager.GetSceneByName("Main").isLoaded)
        {
            if (!SceneManager.GetSceneByName("Menu").isLoaded)
            {
                PlayMainAmbient();
                if (!TimelineManager.Instance.m_IsPlaying)
                    PlayMainMusic(0.3f);
            }
        }
        else
        {
            StopMainMusic();
            StopMainAmbient();
        }
    }

    public AudioSource Play(AudioClip _clip, float _volume = 1)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.rolloffMode = AudioRolloffMode.Custom;
        audioSource.spatialBlend = 0.1f;
        audioSource.maxDistance = 130;
        audioSource.spread = 1;
        audioSource.dopplerLevel = 0;
        audioSource.reverbZoneMix = 1;
        audioSource.clip = _clip;
        audioSource.volume = _volume;
        audioSource.pitch = 1;
        audioSource.Play();
        Destroy(audioSource, _clip.length);


        return audioSource;
    }

    public AudioSource[] PlaySequence(AudioClip _clip, AudioClip _clip2, float _volume = 1)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        AudioSource audioSource2 = gameObject.AddComponent<AudioSource>();

        audioSource.rolloffMode = AudioRolloffMode.Custom;
        audioSource.spatialBlend = 0.1f;
        audioSource.maxDistance = 130;
        audioSource.spread = 1;
        audioSource.dopplerLevel = 0;
        audioSource.reverbZoneMix = 1;
        audioSource.clip = _clip;
        audioSource.volume = _volume;
        audioSource.pitch = 1;

        audioSource2 = audioSource;
        audioSource2.clip = _clip2;

        Destroy(audioSource, _clip.length);
        Destroy(audioSource2, _clip.length + _clip2.length);

        audioSource.Play();
        audioSource2.Play((ulong)_clip.length);


        return new AudioSource[2]{
            audioSource,
            audioSource2};
    }

    public void PlayMenuMusic(float _volume = 1)
    {
        if (m_menuMusicSource != null)
            return;

        AudioSource audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.rolloffMode = AudioRolloffMode.Custom;
        audioSource.spatialBlend = 0.1f;
        audioSource.maxDistance = 130;
        audioSource.spread = 1;
        audioSource.dopplerLevel = 0;
        audioSource.reverbZoneMix = 1;
        audioSource.clip = m_AudioInfo.m_MenuMusic;
        audioSource.volume = _volume;
        audioSource.pitch = 1;
        audioSource.loop = true;
        audioSource.Play();
        m_menuMusicSource = audioSource;
        Destroy(audioSource, m_AudioInfo.m_MenuMusic.length);
    }
    public void StopMenuMusic()
    {
        Destroy(m_menuMusicSource);
    }

    public void PlayMainMusic(float _volume = 1)
    {
        if (m_mainMusicSource != null)
            return;

        AudioSource audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.rolloffMode = AudioRolloffMode.Custom;
        audioSource.spatialBlend = 0.1f;
        audioSource.maxDistance = 130;
        audioSource.spread = 1;
        audioSource.dopplerLevel = 0;
        audioSource.reverbZoneMix = 1;
        audioSource.clip = m_AudioInfo.m_MainMusic[(int)GameManager.Instance.m_Init.m_Level];
        audioSource.volume = m_tmpVolume = _volume;
        audioSource.pitch = 1;
        audioSource.loop = true;
        audioSource.Play();
        m_mainMusicSource = audioSource;
        Destroy(audioSource, m_AudioInfo.m_MainMusic[(int)GameManager.Instance.m_Init.m_Level].length);
    }
    public void StopMainMusic()
    {
        Destroy(m_mainMusicSource);
    }
    public void LowerMainMusicVolume(float _percentage)
    {
        m_mainMusicSource.volume = m_tmpVolume * _percentage;
    }
    public void ResetMainMusicVolume()
    {
        m_mainMusicSource.volume = m_tmpVolume;
    }

    public void PlayMainAmbient(float _volume = 1)
    {
        if (m_mainAmbientSource != null)
            return;

        AudioSource audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.rolloffMode = AudioRolloffMode.Custom;
        audioSource.spatialBlend = 0.1f;
        audioSource.maxDistance = 130;
        audioSource.spread = 1;
        audioSource.dopplerLevel = 0;
        audioSource.reverbZoneMix = 1;
        audioSource.clip = m_AudioInfo.m_MainAmbient[(int)GameManager.Instance.m_Init.m_Level];
        audioSource.volume = _volume;
        audioSource.pitch = 1;
        audioSource.loop = true;
        audioSource.Play();
        m_mainAmbientSource = audioSource;
        Destroy(audioSource, m_AudioInfo.m_MainAmbient[(int)GameManager.Instance.m_Init.m_Level].length);
    }
    public void StopMainAmbient()
    {
        Destroy(m_mainAmbientSource);
    }
}
