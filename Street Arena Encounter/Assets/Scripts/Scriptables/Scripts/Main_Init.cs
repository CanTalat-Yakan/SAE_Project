﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct SInitInfo
{
    public string Name;
}

[CreateAssetMenu(menuName = "Init/Main Init", fileName = "Main Init", order = 0)]
public class Main_Init : ScriptableObject
{
    public int GameMode { set => m_GameMode = (EGameModes)value; }
    public EGameModes m_GameMode;
    public int Level { set => m_Level = (ELevels)value; }
    public ELevels m_Level;

    public int m_Rounds;
    public int m_Timer;

    public int m_LoadingScreenTime;
    public bool m_SkipIntro;

    public SInitInfo m_Player_L;
    public SInitInfo m_Player_R;
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