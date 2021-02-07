using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct PlayerInformation
{
    public PlayerController Player;
    public Rigidbody RB;
    public Animator Ani;
    public BoxCollider Col;
    public InputMaster Input;
    public GP_Settings GP;
    public Attack_Settings ATK;
    [HideInInspector] public float Forward { get => Ani.transform.localScale.x; }
    [HideInInspector] public bool IsLeft { get => Forward == 1; }
    [HideInInspector] public Material GetMaterial { get => Ani.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material; }

    [Header("Current State in Game")]
    public float Health;
    public int RoundsWon;
    public string Name;
    public bool Special;

    public void GatherComponents(GameObject _obj)
    {
        GP = GameManager.Instance.m_GP;
        ATK = GameManager.Instance.m_ATK;

        Player = _obj.GetComponent<PlayerController>();
        Player.m_IsLeft = IsLeft;

        Player.m_MovementController = _obj.GetComponent<MovementController>();
        Player.m_MovementController.m_PlayerInfo = this;

        Player.m_AttackController = _obj.GetComponent<AttackController>();
        Player.m_AttackController.m_PlayerInfo = this;

        RB = _obj.GetComponent<Rigidbody>();
        Col = _obj.GetComponent<BoxCollider>();

        Ani = _obj.transform.GetChild(0).GetComponent<Animator>();

        Input = null;
    }

    public void ResetValues()
    {
        Health = 100;
        RoundsWon = 0;
        Special = true;
    }

    public void Constraint()
    {
        float xPos = 0;
        float offSet = GP.PlayerRadius * 0.5f;

        if (IsLeft)
            xPos = Mathf.Clamp(
                Player.transform.localPosition.x,
                -9 + offSet,
                GameManager.Instance.m_Player_R.Player.transform.localPosition.x - offSet);
        else
            xPos = Mathf.Clamp(
                Player.transform.localPosition.x,
                GameManager.Instance.m_Player_L.Player.transform.localPosition.x + offSet,
                9 - offSet);

        Vector3 newPos = new Vector3(xPos, Mathf.Max(0, Player.transform.localPosition.y), 0);
        Player.transform.localPosition = newPos;
    }
}
