using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PlayerInformation
{
    public CharController Player;
    public CharacterController Char;
    public Animator Ani;
    public InputMaster Input;
    public GP_Settings GP;
    [HideInInspector] public float Forward { get => Ani.transform.localScale.x; set => Forward = value; }

    [Header("Current State in Game")]
    public float Health;
    public int RoundsWon;
}
