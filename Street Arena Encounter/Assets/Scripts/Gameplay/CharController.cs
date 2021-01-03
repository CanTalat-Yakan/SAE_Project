using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(AttackController))]
public class CharController : MonoBehaviour
{
    #region -Values
    MovementController m_movementController;
    AttackController m_attackController;

    [SerializeField] bool m_leftSide;
    #endregion

    void Awake()
    {
        m_movementController = GetComponent<MovementController>();
        m_attackController = GetComponent<AttackController>();
    }

    void Start()
    {
        m_movementController.m_PlayerInfo = m_leftSide ? GameManager.Instance.m_Player_L : GameManager.Instance.m_Player_R;
        m_attackController.m_PlayerInfo = m_leftSide ? GameManager.Instance.m_Player_L : GameManager.Instance.m_Player_R;
    }

    void Update()
    {
        if (!GameManager.Instance.STARTED || GameManager.Instance.LOCKED)
            return;

        m_attackController.Attack();
        if (!m_attackController.m_Attacking)
            m_movementController.Move();
        else
            m_movementController.Fall();

        m_movementController.SetState();
    }

    /// <summary>
    /// Reset Values of CharacterController, AttackController and MovementController
    /// </summary>
    public void ResetValues()
    {
        m_movementController.ResetValues(m_leftSide);
        m_attackController.ResetValues();
        GameManager.Instance.m_Player_L.Health = GameManager.Instance.m_Player_L.GP.Health;
        GameManager.Instance.m_Player_R.Health = GameManager.Instance.m_Player_R.GP.Health;
    }
}