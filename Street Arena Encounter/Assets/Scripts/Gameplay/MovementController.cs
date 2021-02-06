using System;
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

    Vector3 desiredDirection;
    float m_force;
    #endregion

    /// <summary>
    /// Uses the charController and performs a Movement with the given Values of the InputMaster
    /// </summary>
    public void Move()
    {
        //InputValues
        desiredDirection.x = m_PlayerInfo.Input.m_controls.m;
        //SetAnimator Pramater for Movement
        m_PlayerInfo.Ani.SetFloat("Move", m_PlayerInfo.Input.m_controls.m * m_PlayerInfo.Forward);

        //Chrouch Movement
        if (desiredDirection.x * m_PlayerInfo.Forward >= 0)
        {
            //SetAnimator Pramater for Crouching
            m_PlayerInfo.Ani.SetBool("Crouch", m_PlayerInfo.Input.m_controls.c);
        }
        else if (m_PlayerInfo.Input.m_controls.c)
        {
            desiredDirection.x = 0;
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
                desiredDirection.y = m_PlayerInfo.GP.JumpForce;
                //Jumping Dashing forward/ backward
                AttackManager.Instance.Dash(m_PlayerInfo, m_PlayerInfo.GP.JumpDashDistance * desiredDirection.x, 0.25f);
            }
            if (m_PlayerInfo.Input.m_controls.d)
            {
                //Dashing forward/ backward
                AttackManager.Instance.Dash(m_PlayerInfo, m_PlayerInfo.GP.DashDistance * desiredDirection.x, 0.25f);
            }
        }

        //Calculating Gravity
        if (!IsGrounded())
        desiredDirection.y += m_PlayerInfo.GP.GravityForce * Time.deltaTime;
            //desiredDirection.y = Mathf.Max(desiredDirection.y, -25);


        //hand over desiredDirection to charController;
        m_PlayerInfo.Char.Move(desiredDirection * Time.deltaTime);
    }
    /// <summary>
    /// When the Contraint is true then perform Fall() instead of Move()
    /// </summary>
    public void Fall()
    {
        //Reset Values
        m_force = 0;
        desiredDirection.x = 0;

        //Calculating Gravity
        desiredDirection.y += m_PlayerInfo.GP.GravityForce * Time.deltaTime;

        //hand over desiredDirection to charController;
        m_PlayerInfo.Char.Move(desiredDirection * Time.deltaTime);

        //m_PlayerInfo.Char.height = m_PlayerInfo.GP.PlayerHeight;
    }

    #region -Utilities

    /// <summary>
    /// Changes the Flag Enum representing the state the player is currently in
    /// </summary>
    public void SetState()
    {
        if (m_PlayerInfo.Ani.GetFloat("Move") != 0)
            m_CurrentState |= EMovementStates.Move;
        else
            m_CurrentState &= ~EMovementStates.Move;

        if (m_PlayerInfo.Ani.GetFloat("Move") < 0)
            m_CurrentState |= EMovementStates.MoveBackwards;
        else
            m_CurrentState &= ~EMovementStates.MoveBackwards;

        if (m_PlayerInfo.Ani.GetBool("Crouch"))
            m_CurrentState |= EMovementStates.Crouch;
        else
            m_CurrentState &= ~EMovementStates.Crouch;

        if (m_PlayerInfo.Input.m_controls.j)
            m_CurrentState |= EMovementStates.Jump;
        else
            m_CurrentState &= ~EMovementStates.Jump;
    }
    public void SetHeight()
    {
        //Crouching
        m_PlayerInfo.Char.height = m_PlayerInfo.Ani.GetBool("Crouch") ? m_PlayerInfo.GP.CrouchHeight : m_PlayerInfo.GP.PlayerHeight;
        m_PlayerInfo.Char.radius = m_PlayerInfo.GP.MinDistance;
    }
    /// <summary>
    /// Resets the Values of the Players Height and Animation
    /// </summary>
    /// <param name="_leftSide">to place the palyer in the correct starting position</param>
    public void ResetValues()
    {
        m_PlayerInfo.Char.gameObject.transform.position = new Vector3(m_PlayerInfo.IsLeft ? -m_PlayerInfo.GP.PlayerStartPos : m_PlayerInfo.GP.PlayerStartPos, 0, 0);
        m_PlayerInfo.Char.height = m_PlayerInfo.GP.PlayerHeight;

        m_PlayerInfo.Ani.SetFloat("Move", 0);
        m_PlayerInfo.Ani.SetBool("Crouch", false);
    }
    public bool IsGrounded()
    {
        return GameManager.Instance.BoolRayCast(
            transform.localPosition + Vector3.up * 0.005f, 
            Vector3.down, 
            0.005f);
    }
    #endregion
}
