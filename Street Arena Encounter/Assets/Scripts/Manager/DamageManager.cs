﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance { get; private set; }
    [SerializeField] ParticleSystem[] m_ps_L = new ParticleSystem[3];
    [SerializeField] ParticleSystem[] m_ps_R = new ParticleSystem[3];

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public bool DealDamage(bool _toLeftSide, float _amount, EDamageStates _damageType)
    {
        PlayerInformation target = _toLeftSide ?
                    GameManager.Instance.m_Player_L :
                    GameManager.Instance.m_Player_R;

        if (!GameManager.Instance.BoolDistance(2) && CompareStates(target))
        {
            StartCoroutine(PerformDamage(
                target,
                _amount,
                _damageType));
            return true;
        }

        return false;
    }

    bool CompareStates(PlayerInformation _playerInfo)
    {
        EAttackStates currentState_Attack = _playerInfo.Player.m_AttackController.m_CurrentState;
        EMovementStates currentState_Movement = _playerInfo.Player.m_MovementController.m_CurrentState;

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
        CinemachineBasicMultiChannelPerlin noise = GameManager.Instance.m_CMVCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        noise.m_FrequencyGain = _intensity;

        yield return new WaitForSeconds(_duration);
        noise.m_FrequencyGain = 1;

        yield return null;
    }
    IEnumerator PerformDamage(PlayerInformation _playerInfo, float _damageAmount, EDamageStates _damageType)
    {
        //Damage
        if (GameManager.Instance.m_Init.m_GameMode != EGameModes.TRAINING)
            _playerInfo.Health -= _damageAmount; //substracs health with the amount of performed damage 

        _playerInfo.Ani.SetTrigger("Damaged"); //plays damage animation


        //Sepcial
        if (_playerInfo.Health <= 20 && _playerInfo.Special)
        {
            AttackManager.Instance.SetSpecial(_playerInfo.IsLeft, true);
            _playerInfo.Special = false;
        }

        //ParticleSystem
        int i = Mathf.Abs((int)_damageType - 1); //the index of the particleSystem-Array
        if (_playerInfo.IsLeft)
            m_ps_L[i].Play(); //plays the damage particleSystem
        else
            m_ps_R[i].Play(); //plays the damage particleSystem

        //Force
        FallBack(_playerInfo); //Forces the player back
        //Shake
        StartCoroutine(Shake(25, 0.2f)); //Shakes the vmcamera

        yield return null;
    }
}
