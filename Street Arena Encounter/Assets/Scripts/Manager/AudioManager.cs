using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    #region -Values
    [Header("Menu")]
    AudioSource m_menuMusicSource;
    [SerializeField] AudioClip m_MenuMusic;
    public AudioClip m_ButtonSelect;
    public AudioClip m_ButtonMove;
    public AudioClip m_PanelSelect;
    public AudioClip m_PanelBack;

    [Header("Main")]
    AudioSource m_mainMusicSource;
    public AudioClip[] m_MainMusic = new AudioClip[0];
    public AudioClip[] m_Attack = new AudioClip[3];
    public AudioClip m_Block;
    public AudioClip m_Special;
    public AudioClip[] m_Damage = new AudioClip[0];
    public AudioClip[] m_Shout = new AudioClip[0];

    [Header("Round")]
    public AudioClip[] m_Count = new AudioClip[10];
    public AudioClip[] m_Begin = new AudioClip[0];
    public AudioClip m_Round;
    public AudioClip m_FinalRound;
    public AudioClip m_Tie;
    public AudioClip m_Won;
    public AudioClip m_Lose;
    public AudioClip[] m_Player = new AudioClip[2];
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
        if (SceneManager.GetSceneByName("Menu").isLoaded)
            PlayMenuMusic();
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

    public void PlaySequence(AudioClip _clip, AudioClip _clip2, float _volume = 1)
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
        audioSource.clip = m_MenuMusic;
        audioSource.volume = _volume;
        audioSource.pitch = 1;
        audioSource.loop = true;
        audioSource.Play();
        m_menuMusicSource = audioSource;
        Destroy(audioSource, m_MenuMusic.length);
    }

    public void PlayMainMusic(float _volume)
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
        audioSource.clip = m_MainMusic[(int)GameManager.Instance.m_Init.m_Level];
        audioSource.volume = _volume;
        audioSource.pitch = 1;
        audioSource.loop = true;
        audioSource.Play();
        m_mainMusicSource = audioSource;
        Destroy(audioSource, m_MainMusic[(int)GameManager.Instance.m_Init.m_Level].length);
    }

    public void StopMainMusic()
    {
        Destroy(m_mainMusicSource);
    }
}
