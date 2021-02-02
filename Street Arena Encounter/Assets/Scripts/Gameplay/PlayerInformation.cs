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
    public Animator Ani;
    public InputMaster Input;
    public GP_Settings GP;
    [HideInInspector] public float Forward { get => Ani.transform.localScale.x; set => Forward = value; }
    [HideInInspector] public bool IsLeft { get => Forward == 1; set => IsLeft = value; }
    [HideInInspector] public Material getMaterial { get => Ani.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material; set => getMaterial = value; }

    [Header("Current State in Game")]
    public float Health;
    public int RoundsWon;
    public string Name;
    public bool Special;
    public void ResetValues()
    {
        Health = 100;
        RoundsWon = 0;
        Special = true;
    }
}
