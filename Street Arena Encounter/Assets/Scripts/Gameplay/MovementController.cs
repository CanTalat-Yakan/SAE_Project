using UnityEngine;

[System.Flags]
public enum EMovementStates
{
    Idle = 1 << 0,
    Move = 1 << 1,
    MoveBackwards = 1 << 2,
    Jump = 1 << 3,
    Crouch = 1 << 4,
    Lying = 1 << 5,
}
public class MovementController : MonoBehaviour
{
    #region //Fields
    [HideInInspector] public PlayerInformation m_PlayerInfo;

    Vector3 m_desiredDirection = new Vector3();
    float m_force = 0;
    float m_drag = 0;
    bool m_isGrounded = true;
    #endregion

    #region //Properties
    [HideInInspector] public EMovementStates m_CurrentState { get; set; }
    #endregion


    void OnCollisionEnter(Collision _collision)
    {
        if (!_collision.gameObject.CompareTag("Platform"))
            return;

        m_desiredDirection.y = 0;
        m_isGrounded = true;
    }
    void OnCollisionExit(Collision _collision)
    {
        if (!_collision.gameObject.CompareTag("Platform"))
            return;

    }

    #region //Utilities
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

        //StandUp from Laying down
        if(m_PlayerInfo.Input.m_movement.m != 0)
        {
            SetCurrentState(EMovementStates.Lying, false);
            m_PlayerInfo.Ani.SetTrigger("StandUp");
        }

        //Dash
        if (m_PlayerInfo.Input.m_movement.d)
            Force(m_PlayerInfo.GP.DashForce * (m_PlayerInfo.IsLeft ? m_PlayerInfo.Forward : -m_PlayerInfo.Forward), 50);
        //Dash_Back
        if (m_PlayerInfo.Input.m_movement.b_d)
            Force(m_PlayerInfo.GP.DashForce * (m_PlayerInfo.IsLeft ? -m_PlayerInfo.Forward : m_PlayerInfo.Forward), 50);

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
        //Reset Dir
        m_desiredDirection.x = 0;

        //Calculating Gravity
        if (!m_isGrounded)
            m_desiredDirection.y += m_PlayerInfo.GP.GravityForce * Time.deltaTime;
    }
    /// <summary>
    /// Resets the Values of the Players Height and Animation
    /// </summary>
    public void ResetValues()
    {
        //Reset Pos
        m_PlayerInfo.RB.gameObject.transform.position = new Vector3(
            m_PlayerInfo.IsLeft ?
                -m_PlayerInfo.GP.PlayerStartPos :
                m_PlayerInfo.GP.PlayerStartPos,
            0,
            0);

        //Reset Values
        m_desiredDirection.x = 0;
        m_force = 0;

        //Reset Flag
        m_CurrentState = EMovementStates.Move;
        m_CurrentState &= ~EMovementStates.Move; 
        SetHeight();

        //Reset Ani-Params
        m_PlayerInfo.Ani.SetFloat("Move", 0);
        m_PlayerInfo.Ani.SetBool("Crouch", false);
        m_PlayerInfo.Ani.SetBool("Jump", false);
        m_PlayerInfo.Ani.SetTrigger("StandUp");
    }
    /// <summary>
    /// Changes the Flag Enum representing the state the of player
    /// </summary>
    public void SetState()
    {
        SetCurrentState(
            EMovementStates.Idle,
            m_PlayerInfo.Ani.GetFloat("Move") == 0 && !m_PlayerInfo.Ani.GetBool("Crouch") && m_isGrounded);

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
            !m_isGrounded);
    }
    public void SetHeight()
    {
        Vector3 size = new Vector3(
            m_PlayerInfo.GP.PlayerRadius,
            m_PlayerInfo.GP.PlayerHeight,
            1);

        Vector3 center = new Vector3(
            0,
            m_PlayerInfo.GP.PlayerHeight * 0.5f,
            0);

        Vector3 pos = m_PlayerInfo.Ani.gameObject.transform.localPosition;
        pos.y = m_PlayerInfo.GroundOffset;

        if (GetBoolofFlag(EMovementStates.Crouch))
        {
            //size.y *= 0.5f;
            pos.y = m_PlayerInfo.GroundOffset - m_PlayerInfo.GP.CrouchHeight;
        }
        if (GetBoolofFlag(EMovementStates.Lying))
        {
            //size.y *= 0.1f;
            pos.y = m_PlayerInfo.GroundOffset - m_PlayerInfo.GP.CrouchHeight;
        }


        //Set the Size of the Collider from Values in GP-Settings
        m_PlayerInfo.Col.size = size;

        //Set the Center of the Collider from Height of it
        m_PlayerInfo.Col.center = center;

        //Set PlayerOffset from Ground
        m_PlayerInfo.Ani.gameObject.transform.localPosition = pos;
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
    /// <summary>
    /// Lock Target in Movable Space
    /// </summary>
    public void Constraint()
    {
        float xPos = 0;
        float yPos = transform.localPosition.y;
        float offSet = m_PlayerInfo.GP.PlayerRadius * 1.05f;

        if (m_PlayerInfo.IsLeft)
            xPos = Mathf.Clamp(
                transform.localPosition.x,
                transform.localPosition.x - 1,
                GameManager.Instance.m_Player_R.Player.transform.localPosition.x - offSet);
        else if (GameManager.Instance.m_Init.m_GameMode == EGameModes.TRAINING)
            xPos = m_PlayerInfo.GP.PlayerStartPos;
        else
            xPos = Mathf.Clamp(
                transform.localPosition.x,
                GameManager.Instance.m_Player_L.Player.transform.localPosition.x + offSet,
                transform.localPosition.x + 1);

        //yPos = Mathf.Max(
        //        0,
        //        transform.localPosition.y);


        transform.localPosition = new Vector3(
            xPos,
            yPos,
            0);
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
    bool GetBoolofFlag(EMovementStates _compareState)
    {
        return (m_CurrentState & _compareState) != 0;
    }
    #endregion
}
