using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(AttackController))]
public class PlayerController : MonoBehaviour
{
    #region //Values
    [HideInInspector] public MovementController m_MovementController;
    [HideInInspector] public AttackController m_AttackController;
    [HideInInspector] public bool m_IsActive;
    #endregion

    void Update()
    {
        if (!GameManager.Instance.STARTED
            || GameManager.Instance.LOCKED
            || !m_IsActive)
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
        m_MovementController.SetHeight();
        m_MovementController.Constraint();
    }
    void FixedUpdate()
    {
        m_MovementController.Move();
    }
}