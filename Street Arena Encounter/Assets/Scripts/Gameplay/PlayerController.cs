using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(AttackController))]
public class PlayerController : MonoBehaviour
{
    #region -Values
    public MovementController m_MovementController;
    public AttackController m_AttackController;

    [SerializeField] bool m_leftSide;
    #endregion

    void Start()
    {
        m_MovementController = GetComponent<MovementController>();
        m_AttackController = GetComponent<AttackController>();

        m_MovementController.m_PlayerInfo = m_leftSide ? GameManager.Instance.m_Player_L : GameManager.Instance.m_Player_R;
        m_AttackController.m_PlayerInfo = m_leftSide ? GameManager.Instance.m_Player_L : GameManager.Instance.m_Player_R;
    }

    void Update()
    {
        if (!GameManager.Instance.STARTED || GameManager.Instance.LOCKED)
            return;

        m_AttackController.Attack();
        if (!m_AttackController.m_Attacking)
            m_MovementController.Move();
        else
            m_MovementController.Fall();

        m_MovementController.SetState();
    }

    /// <summary>
    /// Reset Values of CharacterController, AttackController and MovementController
    /// </summary>
    public void ResetValues()
    {
        m_MovementController.ResetValues(m_leftSide);
        m_AttackController.ResetValues();
        GameManager.Instance.m_Player_L.Health = GameManager.Instance.m_Player_L.GP.Health;
        GameManager.Instance.m_Player_R.Health = GameManager.Instance.m_Player_R.GP.Health;
    }
}