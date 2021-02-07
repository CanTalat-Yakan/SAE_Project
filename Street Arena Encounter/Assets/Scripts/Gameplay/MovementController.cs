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

    Vector3 m_desiredDirection = new Vector3();
    float m_force = 0;
    float m_drag = 0;
    bool m_isGrounded = true;
    #endregion

    /// <summary>
    /// Changes the velocity of the players rigidbody according to the desired direction and force
    /// </summary>
    public void Move()
    {
        //Move Player with velocity of desired direction and force
        m_PlayerInfo.RB.velocity = m_desiredDirection + Vector3.right * m_force;
    }
    /// <summary>
    /// Calculates the desired Direction with Input, Gravity, Jump and Crouch, also changes the according parameters for the current state
    /// </summary>
    public void CalculateDir()
    {
        //InputValues
        m_desiredDirection.x = m_PlayerInfo.Input.m_movement.m * m_PlayerInfo.GP.MovementSpeed;

        m_PlayerInfo.Ani.SetFloat("Move", m_PlayerInfo.Input.m_movement.m * m_PlayerInfo.Forward); //SetAnimator Pramater for Movement


        //Chrouching
        m_PlayerInfo.Ani.SetBool("Crouch", m_PlayerInfo.Input.m_movement.c); //SetAnimator Pramater for Crouching
        if (m_desiredDirection.x * m_PlayerInfo.Forward < 0)
            if (m_PlayerInfo.Input.m_movement.c)
            {
                m_desiredDirection.x = 0;
                m_PlayerInfo.Ani.SetFloat("Move", 0);
            }

        //Jumping
        m_PlayerInfo.Ani.SetBool("Jump", false); //SetAnimator Pramater for Jumping
        if (m_isGrounded && !m_PlayerInfo.Input.m_movement.c)
            if (m_PlayerInfo.Input.m_movement.j)
            {
                m_PlayerInfo.Ani.SetBool("Jump", true); //SetAnimator Pramater for Jumping
                m_isGrounded = false;
                m_desiredDirection.y = m_PlayerInfo.GP.JumpForce; //Set InputValues for Jumping
                Force(m_PlayerInfo.GP.JumpDashForce * m_desiredDirection.x, 10); //Jumping Dashing forward/ backward
            }

        //Calculating Gravity
        if (!m_isGrounded)
            m_desiredDirection.y += m_PlayerInfo.GP.GravityForce * Time.deltaTime;
    }
    /// <summary>
    /// Sets the default desired Direction and zero force for Move to calculate only the custom gravity
    /// </summary>
    public void DefaultDir()
    {
        //Reset Values
        m_desiredDirection.x = 0;

        //Calculating Gravity
        if (!m_isGrounded)
            m_desiredDirection.y += m_PlayerInfo.GP.GravityForce * Time.deltaTime;
    }

    #region //Utilities
    /// <summary>
    /// Resets the Values of the Players Height and Animation
    /// </summary>
    public void ResetValues()
    {
        m_PlayerInfo.RB.gameObject.transform.position = new Vector3(m_PlayerInfo.IsLeft ? -m_PlayerInfo.GP.PlayerStartPos : m_PlayerInfo.GP.PlayerStartPos, 0, 0);

        m_PlayerInfo.Ani.SetFloat("Move", 0);
        m_PlayerInfo.Ani.SetBool("Crouch", false);
    }
    /// <summary>
    /// Changes the Flag Enum representing the state the of player
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
            m_PlayerInfo.Input.m_movement.j);
    }
    public void SetHeight()
    {
        this.m_PlayerInfo.Col.size = new Vector3(m_PlayerInfo.GP.PlayerRadius, m_PlayerInfo.GP.PlayerHeight, 1);
        this.m_PlayerInfo.Col.center = new Vector3(0, m_PlayerInfo.GP.PlayerHeight * 0.5f, 0);
    }
    public void Force(float _dir, float _drag = 30)
    {
        m_force = _dir;
        m_drag = _drag;
    }
    public void Drag()
    {
        if (m_force > 0)
        {
            m_force -= m_drag * Time.unscaledDeltaTime;
            m_force = Mathf.Clamp(m_force, 0, m_force);
        }
        if (m_force < 0)
        {
            m_force += m_drag * Time.unscaledDeltaTime;
            m_force = Mathf.Clamp(m_force, m_force, 0);
        }
    }
    void OnCollisionEnter(Collision _collision)
    {
        if (!_collision.gameObject.CompareTag("Platform"))
            return;

        m_desiredDirection.y = 0;
        m_isGrounded = true;
    }
    #endregion

    #region //Helper
    void SetCurrentState(EMovementStates _currentState, bool _b)
    {
        if (_b)
            m_CurrentState |= _currentState;
        else
            m_CurrentState &= ~_currentState;
    }
    #endregion
}
