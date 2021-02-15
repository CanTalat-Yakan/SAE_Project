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

    public bool DealDamage(bool _toLeftSide, float _amount, float _range, EDamageStates _damageType)
    {
        if (GameManager.Instance.BoolDistance(_range))
            return false;


        PlayerInformation playerInfo = _toLeftSide ?
                    GameManager.Instance.m_Player_L :
                    GameManager.Instance.m_Player_R;

        bool? result = CompareStates(playerInfo, _damageType);

        if (result is null)
            return false;
        else if ((bool)result)
            StartCoroutine(PerformDamage(
                playerInfo,
                _damageType,
                _amount));
        else
            StartCoroutine(FailedDamage(
                playerInfo,
                _damageType));

        return (bool)result;
    }

    bool? CompareStates(PlayerInformation _playerInfo, EDamageStates _enemyAttackType)
    {
        EAttackStates currentState_Attack = _playerInfo.Player.m_AttackController.m_CurrentState;
        EMovementStates currentState_Movement = _playerInfo.Player.m_MovementController.m_CurrentState;


        if (currentState_Attack == EAttackStates.Block
            || GetBoolofFlag(currentState_Movement, EMovementStates.MoveBackwards))
            return false;

        switch (_enemyAttackType)
        {
            case EDamageStates.High:
                {
                    if (GetBoolofFlag(currentState_Movement, EMovementStates.Crouch)
                        || GetBoolofFlag(currentState_Movement, EMovementStates.Lying))
                        return false;
                    break;
                }
            case EDamageStates.Low:
                {
                    if (GetBoolofFlag(currentState_Movement, EMovementStates.Jump))
                        return false;
                    break;
                }
            default:
                break;
        }


        return true;
    }

    public void Repulsion(PlayerInformation _playerInfo, float _force = 5, float _drag = 20)
    {
        _playerInfo.Player.m_MovementController.Force(_force * -_playerInfo.Forward, _drag);
    }
    public void FallBack(PlayerInformation _playerInfo, float _force = 5, float _drag = 20)
    {
        _playerInfo.Player.m_MovementController.Force(_force * -_playerInfo.Forward, _drag);
    }

    void PlaySound(EDamageStates _damageType)
    {
        switch (_damageType)
        {
            case EDamageStates.High:
                AudioManager.Instance.Play(
                    AudioManager.Instance.m_AudioInfo.m_Heavy_Attack[
                        Random.Range(0, AudioManager.Instance.m_AudioInfo.m_Heavy_Attack.Length)]);
                break;
            case EDamageStates.Middle:
                AudioManager.Instance.Play(
                    AudioManager.Instance.m_AudioInfo.m_Light_Attack[
                        Random.Range(0, AudioManager.Instance.m_AudioInfo.m_Heavy_Attack.Length)]);
                break;
            case EDamageStates.Low:
                AudioManager.Instance.Play(
                    AudioManager.Instance.m_AudioInfo.m_Kick_Attack[
                        Random.Range(0, AudioManager.Instance.m_AudioInfo.m_Heavy_Attack.Length)]);
                break;
            default:
                break;
        }
    }

    IEnumerator Shake(float _intensity, float _duration)
    {
        m_noise.m_FrequencyGain = _intensity;

        yield return new WaitForSeconds(_duration);

        m_noise.m_FrequencyGain = m_originalShakeIntensity;


        yield return null;
    }
    IEnumerator PerformDamage(PlayerInformation _playerInfo, EDamageStates _damageType, float _damageAmount)
    {
        //Play AttackSound
        PlaySound(_damageType);

        //Damage
        if (GameManager.Instance.m_Init.m_GameMode != EGameModes.TRAINING)
            _playerInfo.Health -= _damageAmount; //substracs health with the amount of performed damage 

        _playerInfo.Ani.SetTrigger("Damaged"); //plays damage animation


        //Sepcial
        if (_playerInfo.SpecialVFX)
            AttackManager.Instance.ActivateSpecialVFX(_playerInfo.IsLeft);

        //ParticleSystem
        int i = Mathf.Abs((int)_damageType - 1); //the index of the particleSystem-Array
        if (_playerInfo.IsLeft)
            m_ps_L[i].Play(); //plays the damage particleSystem
        else
            m_ps_R[i].Play(); //plays the damage particleSystem

        //Force
        Repulsion(_playerInfo); //Forces the player back
        //Shake
        StartCoroutine(Shake(25, 0.2f)); //Shakes the vmcamera

        UIManager.Instance.UpdatePlayer_Health();


        yield return null;
    }
    IEnumerator FailedDamage(PlayerInformation _playerInfo, EDamageStates _damageType)
    {
        //Play Sound
        AudioManager.Instance.Play(
            AudioManager.Instance.m_AudioInfo.m_Block[
                Random.Range(0, AudioManager.Instance.m_AudioInfo.m_Block.Length)]);

        //ParticleSystem
        int i = Mathf.Abs((int)_damageType - 1); //the index of the particleSystem-Array
        if (_playerInfo.IsLeft)
            m_ps_L[i].Play(); //plays the damage particleSystem
        else
            m_ps_R[i].Play(); //plays the damage particleSystem

        //Force
        Repulsion(_playerInfo); //Forces the player back
        //Shake
        StartCoroutine(Shake(25, 0.2f)); //Shakes the vmcamera

        UIManager.Instance.UpdatePlayer_Health();


        yield return null;
    }


    bool GetBoolofFlag(EMovementStates _currentState_Movement, EMovementStates _compareState)
    {
        return (_currentState_Movement & _compareState) != 0;
    }
}
