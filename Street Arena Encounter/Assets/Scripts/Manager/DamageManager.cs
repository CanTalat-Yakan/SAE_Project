using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance { get; private set; }

    public PlayerInformation m_Player_L;
    public PlayerInformation m_Player_R;
    bool m_toLeftSide;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        m_Player_L = GameManager.Instance.m_Player_L;
        m_Player_R = GameManager.Instance.m_Player_R;
    }
    public bool DealDamage(float _amount, bool _toLeftSide)
    {
        m_toLeftSide = _toLeftSide;
        //if (GameManager.Instance.m_Init.m_GameMode != EGameModes.TRAINING)

        if (_toLeftSide)
        {
            if (EvaluateDamage())
            {
                m_Player_L.Health -= _amount;
                AttackManager.Instance.Throwback(m_Player_L, -0.3f, 0.25f, false);
                m_Player_L.Ani.SetTrigger("Damaged");
            }
        }
        else
        {
            if (EvaluateDamage())
            {
                m_Player_R.Health -= _amount;
                AttackManager.Instance.Throwback(m_Player_R, -0.3f, 0.25f, false);
                m_Player_R.Ani.SetTrigger("Damaged");
            }
        }

        return true;
    }

    bool EvaluateDamage()
    {
        if (GameManager.Instance.BoolDistance(2))// && !CompareStates())
            return false;
        return true;
    }

    bool CompareStates()
    {
        if (m_toLeftSide)
        {
            if (m_Player_L.Player.m_AttackController.m_CurrentState == EAttackStates.Block
                || m_Player_L.Player.m_MovementController.m_CurrentState == EMovementStates.MoveBackwards)
                return false;
        }
        else
        {
            if (GameManager.Instance.m_Init.m_GameMode != EGameModes.TRAINING)
            {
                if (m_Player_R.Player.m_AttackController.m_CurrentState == EAttackStates.Block
                    || m_Player_R.Player.m_MovementController.m_CurrentState == EMovementStates.MoveBackwards)
                    return false;
            }
        }

        return true;
    }
}
