using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class PlayerInformation
{
    #region //Fields
    [Header("Players Components and Assets")]
    public PlayerController Player;
    public Rigidbody RB;
    public Animator Ani;
    public BoxCollider Col;
    public InputMaster Input;
    public GP_Settings GP;
    public Attack_Settings ATK;

    [Header("Runtime Values")]
    public float Health;
    public int RoundsWon;
    bool specialWasUsed;
    #endregion

    #region //Properties
    public bool Special { get => Health <= 20 && !specialWasUsed; set => specialWasUsed = !value; }
    public bool SpecialVFX { get => Health <= 20 && !specialWasUsed; }
    public float Forward { get => Ani.transform.localScale.x; }
    public bool IsLeft { get => Forward == 1; }
    public Material GetMaterial { get => Ani.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material; }
    public string Name { get => IsLeft ? GameManager.Instance.m_Init.m_Player_L.Name : GameManager.Instance.m_Init.m_Player_R.Name; }
    #endregion


    #region //Utilitites
    public void GatherComponents(GameObject _obj)
    {
        //Get Scriptable Object References with Information of GamePlay and Attacks
        GP = GameManager.Instance.m_GP;
        ATK = GameManager.Instance.m_ATK;

        //Get Component Rigidbody and BoxCollider
        RB = _obj.GetComponent<Rigidbody>();
        Col = _obj.GetComponent<BoxCollider>();

        //Get Animator in Model
        Ani = _obj.transform.GetChild(0).GetComponent<Animator>();

        //Get PlayerController using Movement and Attack
        Player = _obj.GetComponent<PlayerController>();
        Player.m_IsActive = Input != null;
        //Passing Playercontroller the movementController
        Player.m_MovementController = _obj.GetComponent<MovementController>();
        Player.m_MovementController.m_PlayerInfo = this;
        //Passign Playercontrolelr the attackController
        Player.m_AttackController = _obj.GetComponent<AttackController>();
        Player.m_AttackController.m_PlayerInfo = this;
    }
    public void ResetPlayerInformation()
    {
        Health = 100;
        RoundsWon = 0;
        Special = true;
    }
    public void ResetValues()
    {
        Player.m_MovementController.ResetValues();
        Player.m_AttackController.ResetValues();

        Health = GameManager.Instance.m_GP.Health;
        Special = true;
    }
    #endregion
}
