using UnityEngine;

[CreateAssetMenu(menuName = "Init/Main Init", fileName = "Main Init", order = 0)]
public class Main_Init : ScriptableObject
{
    public int GameMode { set => m_GameMode = (EGameModes)value; }
    [Header("Level Attributes")]
    public EGameModes m_GameMode;
    public int Level { set => m_Level = (ELevels)value; }
    public ELevels m_Level;

    public int m_Rounds;
    public int m_Timer;

    [Header("Main Attributes")]
    public int m_LoadingScreenTime;
    public bool m_SkipIntro;


    [Header("Fighter Attributes")]
    public Fighter_Info m_Player_L;
    public Fighter_Info m_Player_R;


    public void RandomLevel()
    {
        m_Level = (ELevels)Random.Range(0, 5);
    }
}
public enum EGameModes
{
    SOLO,
    MULTIPLAYER,
    LOCAL,
    TRAINING,
}
public enum ELevels
{
    Level_01,
    Level_02,
    Level_03,
    Level_04,
    Level_05,
    Level_06,
    Level_07,
    Level_08,
    Level_09,
}