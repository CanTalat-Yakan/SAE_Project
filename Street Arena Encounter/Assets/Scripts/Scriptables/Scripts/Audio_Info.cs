using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Info/Audio Info", fileName = "Audio Info", order = 2)]
public class Audio_Info : ScriptableObject
{
    [Header("Menu")]
    public AudioClip m_MenuMusic;
    public AudioClip m_ButtonSelect;
    public AudioClip m_ButtonMove;
    public AudioClip m_PanelSelect;
    public AudioClip m_PanelBack;

    [Header("Main")]
    public AudioClip[] m_MainMusic = new AudioClip[0];
    public AudioClip[] m_MainAmbient = new AudioClip[0];
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
}
