using Cinemachine;
using System.Collections;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance { get; private set; }

    #region //Fields
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


    #region //Utilities
    /// <summary>
    /// Checks distance and Compares state and performs Damage on enemy when able
    /// </summary>
    /// <param name="_toLeftSide">the enemy to substract health from</param>
    /// <param name="_amount">the amount of damage to substract health</param>
    /// <param name="_range">the range of the attack</param>
    /// <param name="_damageType">the height of the attack</param>
    /// <returns></returns>
    public bool DealDamage(bool _toLeftSide, float _amount, float _range, EDamageStates _damageType)
    {
        if (GameManager.Instance.BoolDistance(_range))
            return false;


        PlayerInformation playerInfo = _toLeftSide
            ? GameManager.Instance.m_Player_L
            : GameManager.Instance.m_Player_R;

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
    public void PlayerIsDead(bool _toLeftSide)
    {
        PlayerInformation playerInfo = _toLeftSide ?
                    GameManager.Instance.m_Player_L :
                    GameManager.Instance.m_Player_R;


        AttackManager.Instance.DeactivateSpecialVFX(_toLeftSide);

        playerInfo.Player.m_MovementController.m_CurrentState = EMovementStates.Lying;

        playerInfo.Ani.SetTrigger("Lying");
    }
    /// <summary>
    /// Compares state of enemy with attack
    /// </summary>
    /// <param name="_playerInfo">enemy</param>
    /// <param name="_enemyAttackType">the height of the attack</param>
    /// <returns></returns>
    bool? CompareStates(PlayerInformation _playerInfo, EDamageStates _enemyAttackType)
    {
        EAttackStates currentState_Attack = _playerInfo.Player.m_AttackController.m_CurrentState;
        EMovementStates currentState_Movement = _playerInfo.Player.m_MovementController.m_CurrentState;


        switch (_enemyAttackType)
        {
            case EDamageStates.High:
                {
                    if (GetBoolofFlag(currentState_Movement, EMovementStates.Crouch)
                        || GetBoolofFlag(currentState_Movement, EMovementStates.Lying)
                        || GetBoolofFlag(currentState_Movement, EMovementStates.Lying))
                        return false;
                    break;
                }
            case EDamageStates.Middle:
                {
                    if (currentState_Attack == EAttackStates.Block
                        || GetBoolofFlag(currentState_Movement, EMovementStates.MoveBackwards)
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

    /// <summary>
    /// dashes the enemy back
    /// </summary>
    /// <param name="_playerInfo">enemy</param>
    /// <param name="_force">the force applied to RB</param>
    /// <param name="_drag">drag applied to RB</param>
    void PushBack(PlayerInformation _playerInfo, float _force = 5, float _drag = 20)
    {
        _playerInfo.Player.m_MovementController.Force(_force * -_playerInfo.Forward, _drag);
    }
    /// <summary>
    /// dashes the enemy back and puts him on the ground
    /// </summary>
    /// <param name="_playerInfo">enemy</param>
    /// <param name="_force">the force applied to RB</param>
    /// <param name="_drag">drag applied to RB</param>
    public void FallBack(bool _toLeft, float _force = 5, float _drag = 20)
    {
        PlayerInformation playerInfo = _toLeft
                ? GameManager.Instance.m_Player_L
                : GameManager.Instance.m_Player_R;


        playerInfo.Player.m_MovementController.Force(_force * -playerInfo.Forward, _drag);

        playerInfo.Player.m_MovementController.m_CurrentState = EMovementStates.Lying;
        playerInfo.Ani.SetTrigger("Lying");
    }

    /// <summary>
    /// Plays a certain Sound according to the damageType
    /// </summary>
    /// <param name="_damageType"></param>
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
    #endregion

    #region //Coroutines
    /// <summary>
    /// Shakes the VMCamera
    /// </summary>
    /// <param name="_intensity">the tmpIntensity when shaking</param>
    /// <param name="_duration">the duration of shaking</param>
    /// <returns></returns>
    IEnumerator Shake(float _intensity, float _duration)
    {
        m_noise.m_FrequencyGain = _intensity;

        yield return new WaitForSeconds(_duration);

        m_noise.m_FrequencyGain = m_originalShakeIntensity;


        yield return null;
    }
    /// <summary>
    /// Substracts Damage, plays VFX and PS, Updates UI, Plays Sound and Force
    /// </summary>
    /// <param name="_playerInfo"></param>
    /// <param name="_damageType"></param>
    /// <param name="_damageAmount"></param>
    /// <returns></returns>
    IEnumerator PerformDamage(PlayerInformation _playerInfo, EDamageStates _damageType, float _damageAmount)
    {
        //Play AttackSound
        PlaySound(_damageType);

        //Damage
        if (GameManager.Instance.m_Init.m_GameMode != EGameModes.TRAINING)
            _playerInfo.Health -= _damageAmount; //substracs health with the amount of performed damage 

        //Set Player Dead
        if (_playerInfo.Health <= 0)
            PlayerIsDead(_playerInfo.IsLeft);

        //plays damage animation
        _playerInfo.Ani.SetTrigger("Damaged");

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
        PushBack(_playerInfo);

        //Shake
        StartCoroutine(Shake(25, 0.2f)); //Shakes the vmcamera

        UIManager.Instance.UpdatePlayer_Health();


        yield return null;
    }
    /// <summary>
    /// Failed Damge, plays VFX and PS, Plays Sound
    /// </summary>
    /// <param name="_playerInfo"></param>
    /// <param name="_damageType"></param>
    /// <returns></returns>
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

        //Shake
        StartCoroutine(Shake(25, 0.2f)); //Shakes the vmcamera

        //PenaltyTime
        _playerInfo.Player.m_AttackController.m_Penalty = 14;


        yield return null;
    }
    #endregion

    #region //Helper
    bool GetBoolofFlag(EMovementStates _currentState_Movement, EMovementStates _compareState)
    {
        return (_currentState_Movement & _compareState) != 0;
    }
    #endregion
}
