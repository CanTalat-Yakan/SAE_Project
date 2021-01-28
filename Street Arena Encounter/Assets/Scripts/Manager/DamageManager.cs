using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance { get; private set; }

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
    public bool DealDamage(float _amount, bool _toLeftSide)
    {
        m_toLeftSide = _toLeftSide;

        if (EvaluateDamage())
        {
            StartCoroutine(PerformDamage(_amount));
            return true;
        }

        return false;
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
            if (GameManager.Instance.m_Player_L.Player.m_AttackController.m_CurrentState == EAttackStates.Block
                || GameManager.Instance.m_Player_L.Player.m_MovementController.m_CurrentState == EMovementStates.MoveBackwards)
                return false;
        }
        else
        {
            if (GameManager.Instance.m_Player_R.Player.m_AttackController.m_CurrentState == EAttackStates.Block
                || GameManager.Instance.m_Player_R.Player.m_MovementController.m_CurrentState == EMovementStates.MoveBackwards)
                return false;
        }

        return true;
    }

    IEnumerator PerformDamage(float _damageAmount)
    {
        if (m_toLeftSide)
        {
            if (GameManager.Instance.m_Init.m_GameMode != EGameModes.TRAINING)
                GameManager.Instance.m_Player_L.Health -= _damageAmount;
            GameManager.Instance.m_Player_L.Ani.SetTrigger("Damaged");
        }
        else
        {
            if (GameManager.Instance.m_Init.m_GameMode != EGameModes.TRAINING)
                GameManager.Instance.m_Player_R.Health -= _damageAmount;
            GameManager.Instance.m_Player_R.Ani.SetTrigger("Damaged");
        }

        AttackManager.Instance.Throwback(GameManager.Instance.m_Player_R, -0.3f, 0.25f, false);

        StartCoroutine(Shake(55, 0.2f));

        yield return null;
    }

    IEnumerator Shake(float _intensity, float _duration)
    {
        CinemachineBasicMultiChannelPerlin noise = GameManager.Instance.m_CMVCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        float tmpValue = noise.m_FrequencyGain;
        noise.m_FrequencyGain = _intensity;

        yield return new WaitForSeconds(_duration);
        noise.m_FrequencyGain = tmpValue;

        yield return null;
    }
}
