﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct SInitInfo
{
    public PlayerInput Input;
    public string Name;
}

[CreateAssetMenu(menuName = "Init/Main Init", fileName = "Main Init", order = 0)]
public class Main_Init : ScriptableObject
{
    public int GameMode { set => m_GameMode = (E_GameModes)value; }
    public E_GameModes m_GameMode;
    public int Level { set => m_Level = (E_Levels)value; }
    public E_Levels m_Level;

    public int m_Rounds;
    public int m_Timer;

    public SInitInfo m_Player_L;
    public SInitInfo m_Player_R;
}
public enum E_GameModes
{
    SOLO,
    MULTIPLAYER,
    LOCAL,
    TRAINING,
}
public enum E_Levels
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