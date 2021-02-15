using UnityEngine;

[CreateAssetMenu(menuName = "Info/Audio Info", fileName = "Audio Info", order = 2)]
public class Audio_Info : ScriptableObject
{
    [Header("Menu")]
    public AudioClip m_MenuMusic;
    public AudioClip m_ButtonSelect;
    public AudioClip m_ButtonMove;
    public AudioClip m_Joined;

    [Header("Main")]
    public AudioClip[] m_MainMusic = new AudioClip[0];
    public AudioClip[] m_MainAmbient = new AudioClip[0];

    [Header("Attack")]
    public AudioClip m_Special_Activation;
    public AudioClip m_Special_Attack;
    public AudioClip[] m_Block = new AudioClip[0];
    public AudioClip[] m_Heavy_Attack = new AudioClip[0];
    public AudioClip[] m_Light_Attack = new AudioClip[0];
    public AudioClip[] m_Kick_Attack = new AudioClip[0];

    [Header("Female-Character")]
    public AudioClip[] m_Taunts_F = new AudioClip[0];
    [Header("Male-Character")]
    public AudioClip[] m_Taunts_M = new AudioClip[0];

    [Header("Round")]
    public AudioClip[] m_Count = new AudioClip[10];
    public AudioClip[] m_Begin = new AudioClip[0];
    public AudioClip[] m_RoundX = new AudioClip[5];
    public AudioClip m_Round;
    public AudioClip m_FinalRound;
    public AudioClip m_Tie;
    public AudioClip m_Won;
    public AudioClip m_Lose;
    public AudioClip[] m_Player = new AudioClip[2];
}
