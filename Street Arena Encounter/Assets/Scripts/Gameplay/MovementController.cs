using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum EMovementStates
{
    Idle = 1 << 0,
    Move = 1 << 1,
    Jump = 1 << 2,
    Crouch = 1 << 3,
}
public class MovementController : MonoBehaviour
{
    #region -Values
    [HideInInspector] public PlayerInformation m_PlayerInfo;
    [HideInInspector] public EMovementStates m_movementState;

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

        //Lock target in movable space
        if (Constraint())
            return;

        //Chrouch Movement
        if (desiredDirection.x * m_PlayerInfo.Forward < 0 && m_PlayerInfo.Input.m_controls.c)
        {
            m_PlayerInfo.Ani.SetFloat("Move", 0);
            m_PlayerInfo.Ani.SetBool("Crouch", true);
            m_PlayerInfo.Char.height = m_PlayerInfo.GP.CrouchHeight;
            Fall();
            return;
        }

        //SetAnimator Pramater for Movement
        m_PlayerInfo.Ani.SetFloat("Move", m_PlayerInfo.Input.m_controls.m * m_PlayerInfo.Forward);

        //Jumping
        m_PlayerInfo.Ani.SetBool("Jump", false);
        if (m_PlayerInfo.Char.isGrounded && !m_PlayerInfo.Input.m_controls.c)
        {
            if (m_PlayerInfo.Input.m_controls.j)
            {
                m_PlayerInfo.Ani.SetBool("Jump", true);
                desiredDirection.y = m_PlayerInfo.GP.JumpForce;
                //Jumping Force forward or backward
                m_force = desiredDirection.x * m_PlayerInfo.GP.JumpDashForce;
            }
            if (m_PlayerInfo.Input.m_controls.d)
                //Dashing forward or backward
                m_force = desiredDirection.x * m_PlayerInfo.GP.DashForce;
        }

        //SetAnimator Pramater for Crouching
        m_PlayerInfo.Ani.SetBool("Crouch", m_PlayerInfo.Input.m_controls.c);
        //Crouching
        m_PlayerInfo.Char.height = m_PlayerInfo.Ani.GetBool("Crouch") ? m_PlayerInfo.GP.CrouchHeight : m_PlayerInfo.GP.PlayerHeight;

        //Calculating Gravity
        desiredDirection.y += m_PlayerInfo.GP.GravityForce * Time.deltaTime;
        desiredDirection.y = Mathf.Clamp(desiredDirection.y, m_PlayerInfo.GP.GravityForce, 10);
        //Calculating Force for Dashing
        desiredDirection.x += m_force;
        m_force -= m_force * 3 * Time.deltaTime;

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
        desiredDirection.y = Mathf.Clamp(desiredDirection.y, m_PlayerInfo.GP.GravityForce, 10);

        //hand over desiredDirection to charController;
        m_PlayerInfo.Char.Move(desiredDirection * Time.deltaTime);

        m_PlayerInfo.Char.height = m_PlayerInfo.GP.PlayerHeight;
    }

    #region -Utilities
    /// <summary>
    /// Changes the Flag Enum representing the state the player is currently in
    /// </summary>
    public void SetState()
    {
        if (m_PlayerInfo.Ani.GetFloat("Move") != 0)
            m_movementState |= EMovementStates.Move;
        else
            m_movementState &= ~EMovementStates.Move;

        if (m_PlayerInfo.Ani.GetBool("Crouch"))
            m_movementState |= EMovementStates.Crouch;
        else
            m_movementState &= ~EMovementStates.Crouch;

        if (m_PlayerInfo.Input.m_controls.j)
            m_movementState |= EMovementStates.Jump;
        else
            m_movementState &= ~EMovementStates.Jump;
    }
    /// <summary>
    /// Resets the Values of the Players Height and Animation
    /// </summary>
    /// <param name="_leftSide">to place the palyer in the correct starting position</param>
    public void ResetValues(bool _leftSide)
    {
        m_PlayerInfo.Char.gameObject.transform.position = new Vector3(_leftSide ? -m_PlayerInfo.GP.PlayerStartPos : m_PlayerInfo.GP.PlayerStartPos, 0, 0);
        m_PlayerInfo.Char.height = m_PlayerInfo.GP.PlayerHeight;

        m_PlayerInfo.Ani.SetFloat("Move", 0);
        m_PlayerInfo.Ani.SetBool("Crouch", false);
    }
    /// <summary>
    /// Calculates the distance of the player and returns a bool when a threshold is stepped over for Max Distance between the two players
    /// </summary>
    bool Constraint()
    {
        if (!GameManager.Instance.GetDistance(m_PlayerInfo.GP.MinDistance))
            if ((m_PlayerInfo.Forward != 1) ? desiredDirection.x < 0 : desiredDirection.x > 0)
            {
                m_PlayerInfo.Ani.SetFloat("Move", 0);
                m_PlayerInfo.Ani.SetBool("Crouch", false);
                Fall();
                return true;
            }

        if (GameManager.Instance.GetDistance(m_PlayerInfo.GP.MaxDistance))
            if ((m_PlayerInfo.Forward != 1) ? desiredDirection.x > 0 : desiredDirection.x < 0)
            {
                m_PlayerInfo.Ani.SetFloat("Move", 0);
                m_PlayerInfo.Ani.SetBool("Crouch", false);
                Fall();
                return true;
            }

        return false;
    }
    #endregion
}
