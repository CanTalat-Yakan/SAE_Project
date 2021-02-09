using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance { get; private set; }

    #region //Values
    [SerializeField] ParticleSystem[] m_ps_L = new ParticleSystem[3];
    [SerializeField] ParticleSystem[] m_ps_R = new ParticleSystem[3];

    CinemachineBasicMultiChannelPerlin m_noise;
    float m_originalShakeIntensity;
    #endregion


    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        m_noise = GameManager.Instance.m_CMVCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        m_originalShakeIntensity = m_noise.m_FrequencyGain;
    }

    public bool DealDamage(bool _toLeftSide, float _amount, EDamageStates _damageType)
    {
        if (!GameManager.Instance.BoolDistance(2) && CompareStates(_toLeftSide))
        {
            StartCoroutine(PerformDamage(
                _toLeftSide,
                _amount,
                _damageType));
            return true;
        }

        return false;
    }

    bool CompareStates(bool _toLeftSide)
    {
        PlayerInformation playerInfo = _toLeftSide ?
                    GameManager.Instance.m_Player_L :
                    GameManager.Instance.m_Player_R;

        EAttackStates currentState_Attack = playerInfo.Player.m_AttackController.m_CurrentState;
        EMovementStates currentState_Movement = playerInfo.Player.m_MovementController.m_CurrentState;

        if (currentState_Attack == EAttackStates.Block
            || currentState_Movement == EMovementStates.MoveBackwards)
            return false;

        return true;
    }

    public void FallBack(PlayerInformation _playerInfo, float _force = 5, float _drag = 20)
    {
        _playerInfo.Player.m_MovementController.Force(_force * -_playerInfo.Forward, _drag);
    }

    IEnumerator Shake(float _intensity, float _duration)
    {
        m_noise.m_FrequencyGain = _intensity;

        yield return new WaitForSeconds(_duration);

        m_noise.m_FrequencyGain = m_originalShakeIntensity;


        yield return null;
    }
    IEnumerator PerformDamage(bool _toLeftSide, float _damageAmount, EDamageStates _damageType)
    {
        PlayerInformation playerInfo = _toLeftSide ?
                    GameManager.Instance.m_Player_L :
                    GameManager.Instance.m_Player_R;

        //Damage
        if (GameManager.Instance.m_Init.m_GameMode != EGameModes.TRAINING)
            playerInfo.Health -= _damageAmount; //substracs health with the amount of performed damage 

        playerInfo.Ani.SetTrigger("Damaged"); //plays damage animation


        //Sepcial
        if (playerInfo.Health <= 20 && playerInfo.Special)
        {
            AttackManager.Instance.SetSpecial(playerInfo.IsLeft, true);
            playerInfo.Special = false;
        }

        //ParticleSystem
        int i = Mathf.Abs((int)_damageType - 1); //the index of the particleSystem-Array
        if (playerInfo.IsLeft)
            m_ps_L[i].Play(); //plays the damage particleSystem
        else
            m_ps_R[i].Play(); //plays the damage particleSystem

        //Force
        FallBack(playerInfo); //Forces the player back
        //Shake
        StartCoroutine(Shake(25, 0.2f)); //Shakes the vmcamera


        if (_toLeftSide)
            GameManager.Instance.m_Player_L = playerInfo;
        else
            GameManager.Instance.m_Player_R = playerInfo;

        UIManager.Instance.SetPlayer_Health();

        yield return null;
    }
}
