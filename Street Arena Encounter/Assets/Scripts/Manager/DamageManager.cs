using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance { get; private set; }
    [SerializeField] ParticleSystem[] m_ps_L = new ParticleSystem[3];
    [SerializeField] ParticleSystem[] m_ps_R = new ParticleSystem[3];

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
    public bool DealDamage(float _amount, bool _toLeftSide, EDamageStates _damageType)
    {
        m_toLeftSide = _toLeftSide;

        if (EvaluateDamage())
        {
            StartCoroutine(PerformDamage(_amount, _damageType));
            return true;
        }

        return false;
    }

    bool EvaluateDamage()
    {
        if (!GameManager.Instance.BoolDistance(2))// && CompareStates())
            return true;
        return false;
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

    IEnumerator PerformDamage(float _damageAmount, EDamageStates _damageType)
    {
        if (m_toLeftSide)
        {
            if (GameManager.Instance.m_Init.m_GameMode != EGameModes.TRAINING)
                GameManager.Instance.m_Player_L.Health -= _damageAmount;
            GameManager.Instance.m_Player_L.Ani.SetTrigger("Damaged");

            m_ps_L[(int)_damageType].Play();
        }
        else
        {
            if (GameManager.Instance.m_Init.m_GameMode != EGameModes.TRAINING)
                GameManager.Instance.m_Player_R.Health -= _damageAmount;
            GameManager.Instance.m_Player_R.Ani.SetTrigger("Damaged");

            m_ps_R[(int)_damageType].Play();
        }

        AttackManager.Instance.Dash(GameManager.Instance.m_Player_R, -0.8f, 0.25f, false);

        StartCoroutine(Shake(25, 0.2f));

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
