using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(AttackController))]
public class PlayerController : MonoBehaviour
{
    #region -Values
    [HideInInspector] public MovementController m_MovementController;
    [HideInInspector] public AttackController m_AttackController;
    [HideInInspector] public bool m_IsLeft;
    #endregion

    void Update()
    {
        if (!GameManager.Instance.STARTED 
            || GameManager.Instance.LOCKED 
            || (!m_IsLeft 
                && GameManager.Instance.m_Player_R.Input == null))
            return;

        m_AttackController.Attack();

        if (!m_AttackController.m_Attacking)
            m_MovementController.CalculateDir();
        else
            m_MovementController.DefaultDir();
    }
    void LateUpdate()
    {
        m_MovementController.Drag();
        m_MovementController.SetState();
        //m_MovementController.SetHeight();
    }
    void FixedUpdate()
    {
        m_MovementController.Move();
    }

    /// <summary>
    /// Reset Values of CharacterController, AttackController and MovementController
    /// </summary>
    public void ResetValues()
    {
        m_MovementController.ResetValues();
        m_AttackController.ResetValues();
        GameManager.Instance.m_Player_L.Health = GameManager.Instance.m_Player_L.GP.Health;
        GameManager.Instance.m_Player_R.Health = GameManager.Instance.m_Player_R.GP.Health;
    }
}