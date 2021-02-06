﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum EMovementStates
{
    Idle = 1 << 0,
    Move = 1 << 1,
    MoveBackwards = 1 << 2,
    Jump = 1 << 3,
    Crouch = 1 << 4,
}
public class MovementController : MonoBehaviour
{
    #region -Values
    [HideInInspector] public PlayerInformation m_PlayerInfo;
    [HideInInspector] public EMovementStates m_CurrentState;

    Vector3 m_desiredDirection;
    float m_force;
    float m_drag;
    #endregion

    /// <summary>
    /// Uses the charController and performs a Movement with the given Values of the InputMaster
    /// </summary>
    public void Move()
    {
        Debug.Log(m_force);

        //InputValues
        m_desiredDirection.x = m_PlayerInfo.Input.m_controls.m;
        //SetAnimator Pramater for Movement
        m_PlayerInfo.Ani.SetFloat("Move", m_PlayerInfo.Input.m_controls.m * m_PlayerInfo.Forward);

        //Chrouch Movement
        if (m_desiredDirection.x * m_PlayerInfo.Forward >= 0)
        {
            //SetAnimator Pramater for Crouching
            m_PlayerInfo.Ani.SetBool("Crouch", m_PlayerInfo.Input.m_controls.c);
        }
        else if (m_PlayerInfo.Input.m_controls.c)
        {
            m_desiredDirection.x = 0;
            m_PlayerInfo.Ani.SetFloat("Move", 0);
            m_PlayerInfo.Ani.SetBool("Crouch", m_PlayerInfo.Input.m_controls.c);
            return;
        }
        else
        {
            m_PlayerInfo.Ani.SetBool("Crouch", m_PlayerInfo.Input.m_controls.c);
        }

        //SetAnimator Pramater for Jumping
        m_PlayerInfo.Ani.SetBool("Jump", false);
        //Jumping
        if (IsGrounded() && !m_PlayerInfo.Input.m_controls.c)
        {
            if (m_PlayerInfo.Input.m_controls.j)
            {
                //SetAnimator Pramater for Jumping
                m_PlayerInfo.Ani.SetBool("Jump", true);
                //Set InputValues for Jumping
                m_desiredDirection.y = m_PlayerInfo.GP.JumpForce;
                //Jumping Dashing forward/ backward
                Force(m_PlayerInfo.GP.JumpDashForce * m_desiredDirection.x);
                //m_PlayerInfo.RB.AddForce(m_PlayerInfo.GP.JumpDashDistance * desiredDirection.x * Vector3.right, ForceMode.Impulse);
                //AttackManager.Instance.Dash(m_PlayerInfo, m_PlayerInfo.GP.JumpDashDistance * desiredDirection.x, 0.25f);
            }
            if (m_PlayerInfo.Input.m_controls.d)
                //Dashing forward/ backward
                AttackManager.Instance.Dash(m_PlayerInfo, m_PlayerInfo.GP.DashForce * m_desiredDirection.x);
        }

        //Calculating Gravity
        if (!IsGrounded())
            m_desiredDirection.y += m_PlayerInfo.GP.GravityForce * Time.deltaTime;

        //Move Player with velocity of desired direction
        m_PlayerInfo.RB.velocity = m_desiredDirection + Vector3.right * m_force;
    }
    /// <summary>
    /// When the Contraint is true then perform Fall() instead of Move()
    /// </summary>
    public void Fall()
    {
        //Reset Values
        m_force = 0;
        m_desiredDirection.x = 0;

        //Calculating Gravity
        if (!IsGrounded())
            m_desiredDirection.y += m_PlayerInfo.GP.GravityForce * Time.deltaTime;

        m_PlayerInfo.RB.velocity = m_desiredDirection + Vector3.right * m_force;
    }

    #region //Utilities
    /// <summary>
    /// Changes the Flag Enum representing the state the player is currently in
    /// </summary>
    public void SetState()
    {
        SetCurrentState(
            EMovementStates.Move, 
            m_PlayerInfo.Ani.GetFloat("Move") != 0);
        SetCurrentState(
            EMovementStates.MoveBackwards, 
            m_PlayerInfo.Ani.GetFloat("Move") < 0);
        SetCurrentState(
            EMovementStates.Crouch, 
            m_PlayerInfo.Ani.GetBool("Crouch"));
        SetCurrentState(
            EMovementStates.Jump, 
            m_PlayerInfo.Input.m_controls.j);
    }
    public void SetHeight()
    {
        //Crouching
        //m_PlayerInfo.Char.height = m_PlayerInfo.Ani.GetBool("Crouch") ? m_PlayerInfo.GP.CrouchHeight : m_PlayerInfo.GP.PlayerHeight;
        //m_PlayerInfo.Char.radius = m_PlayerInfo.GP.MinDistance;
    }
    public bool IsGrounded()
    {
        return GameManager.Instance.BoolRayCast(
            transform.localPosition + Vector3.up * 0.005f,
            Vector3.down,
            0.005f);
    }
    public void Force(float _dir, float _drag = 30)
    {
        m_force = _dir;
        m_drag = _drag;
    }
    public void Drag()
    {
        if (m_force > 0)
            m_force -= m_drag * Time.deltaTime;
        if (m_force < 0)
            m_force += m_drag * Time.deltaTime;

        if (Mathf.Abs(m_force) < 0.31f)
            m_force = 0;
    }
    void OnCollisionEnter()
    {
        m_desiredDirection.y = 0;
    }
    #endregion

    #region //Helper
    void SetCurrentState(EMovementStates _currentState, bool _b)
    {
        if(_b) 
            m_CurrentState |= _currentState;
        else
            m_CurrentState &= ~_currentState;
    } 
    /// <summary>
    /// Resets the Values of the Players Height and Animation
    /// </summary>
    /// <param name="_leftSide">to place the palyer in the correct starting position</param>
    public void ResetValues()
    {
        m_PlayerInfo.RB.gameObject.transform.position = new Vector3(m_PlayerInfo.IsLeft ? -m_PlayerInfo.GP.PlayerStartPos : m_PlayerInfo.GP.PlayerStartPos, 0, 0);

        m_PlayerInfo.Ani.SetFloat("Move", 0);
        m_PlayerInfo.Ani.SetBool("Crouch", false);
    }
    #endregion
}
