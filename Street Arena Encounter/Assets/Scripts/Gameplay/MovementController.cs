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

        //Chrouch Movement
        if (desiredDirection.x * m_PlayerInfo.Forward < 0 && m_PlayerInfo.Input.m_controls.c)
        {
            m_PlayerInfo.Ani.SetFloat("Move", 0);
            m_PlayerInfo.Ani.SetBool("Crouch", true);
            m_PlayerInfo.Char.height = m_PlayerInfo.GP.CrouchHeight;
            Fall();
            return;
        }

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

        //SetAnimator Pramater for Movement
        m_PlayerInfo.Ani.SetFloat("Move", m_PlayerInfo.Input.m_controls.m * m_PlayerInfo.Forward);

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

        //Lock target in movable space
        Constraint();

        //Set State of current Movement
        SetState();
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


        SetState();
    }

    #region -Utilities
    /// <summary>
    /// Calculates the distance of the player and returns a bool when a threshold is stepped over for Max Distance between the two players
    /// </summary>
    void Constraint()
    {
        float xPos = 0;
        float offSet = m_PlayerInfo.GP.MinDistance * 0.5f;

        if (m_PlayerInfo.IsLeft)
            xPos = Mathf.Clamp(
                transform.localPosition.x, 
                -9 + offSet, 
                GameManager.Instance.m_Player_R.Player.transform.localPosition.x - offSet);
        else
            xPos = Mathf.Clamp(
                transform.localPosition.x, 
                GameManager.Instance.m_Player_L.Player.transform.localPosition.x + offSet, 
                9 - offSet);

        Vector3 newPos = new Vector3(xPos, transform.localPosition.y, 0);
        transform.localPosition = newPos;
    }
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
    #endregion
}
