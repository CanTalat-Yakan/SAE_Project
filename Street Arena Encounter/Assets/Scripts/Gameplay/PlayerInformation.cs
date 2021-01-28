using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct PlayerInformation
{
    public PlayerController Player;
    public CharacterController Char;
    public Rigidbody RB;
    public Animator Ani;
    public InputMaster Input;
    public GP_Settings GP;
    [HideInInspector] public float Forward { get => Ani.transform.localScale.x; set => Forward = value; }

    [Header("Current State in Game")]
    public float Health;
    public int RoundsWon;
    public string Name;
    public void ResetValues()
    {
        Health = 100;
        RoundsWon = 0;
    }
}
