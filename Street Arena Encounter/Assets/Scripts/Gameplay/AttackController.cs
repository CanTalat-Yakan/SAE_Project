﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum EAttackStates
{
    NONE,
    Block,
    F_LightAttack,
    B_LightAttack,
    F_HeavyAttack,
    B_HeavyAttack,
    F_LowAttack,
    B_LowAttack,
}
public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimationClipOverrides(int capacity) : base(capacity) { }

    public AnimationClip this[string name]
    {
        get { return this.Find(x => x.Key.name.Equals(name)).Value; }
        set
        {
            int index = this.FindIndex(x => x.Key.name.Equals(name));
            if (index != -1)
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
        }
    }
}
public class AttackController : MonoBehaviour
{
    #region -Values
    [HideInInspector] public PlayerInformation m_PlayerInfo;
    [HideInInspector] public EAttackStates m_currentState;

    [HideInInspector] public bool m_Attacking;

    protected AnimatorOverrideController animatorOverrideController;
    protected AnimationClipOverrides clipOverrides;
    #endregion

    public void Start()
    {
        animatorOverrideController = new AnimatorOverrideController(GameManager.Instance.m_Player_L.Ani.runtimeAnimatorController);
        GameManager.Instance.m_Player_L.Ani.runtimeAnimatorController = animatorOverrideController;

        clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(clipOverrides);
    }

    /// <summary>
    /// Sets the current Attack animation and state according to the Values of the InputMaster
    /// </summary>
    public void Attack()
    {
        if (m_PlayerInfo.Input.m_attacks.block)
            StartCoroutine(Base(EAttackStates.Block, Block()));

        if (m_PlayerInfo.Input.m_attacks.light)
            StartCoroutine(Base(EAttackStates.F_LightAttack, Hit()));

        //if (m_PlayerInfo.Input.m_attacks.b_light)
        //    StartCoroutine(Base(EAttackStates.B_LightAttack, Hit()));

        if (m_PlayerInfo.Input.m_attacks.heavy)
            StartCoroutine(Base(EAttackStates.F_LowAttack, Hit()));

        //if (m_PlayerInfo.Input.m_attacks.b_heavy)
        //    StartCoroutine(Base(EAttackStates.B_HeavyAttack, Hit()));

        //if (m_PlayerInfo.Input.m_attacks.low)
        //    StartCoroutine(Base(EAttackStates.F_LowAttack, HitLow()));

        //if (m_PlayerInfo.Input.m_attacks.b_low)
        //    StartCoroutine(Base(EAttackStates.B_LowAttack, HitLow()));
    }

    void LateUpdate()
    {
        //m_currentState = EAttackStates.NONE;
    }

    #region -Enumerators
    public IEnumerator Base(EAttackStates _state, IEnumerator _content, float _activation = 8, float _recovery = 4)
    {
        if (m_currentState != _state)
        {
            //clipOverrides["Punching"] = AttackManager.Instance.clips[Random.Range(0, 5)];
            //animatorOverrideController.ApplyOverrides(clipOverrides);
            animatorOverrideController.clips[5].overrideClip = AttackManager.Instance.clips[Random.Range(0, 5)];
            m_PlayerInfo.Ani.runtimeAnimatorController = animatorOverrideController;

            GameManager.Instance.DeactivateChars();
            m_currentState = _state;
            m_PlayerInfo.Ani.SetBool((_state != EAttackStates.Block) ? "Attacking" : "Block", m_Attacking = true);

            for (int i = 0; i < _activation; i++)
                yield return new WaitForEndOfFrame();

            StartCoroutine(_content);

            m_PlayerInfo.Ani.SetBool((_state != EAttackStates.Block) ? "Attacking" : "Block", m_Attacking = false);

            for (int i = 0; i < _recovery; i++)
                yield return new WaitForEndOfFrame();

            m_currentState = EAttackStates.NONE;
            GameManager.Instance.ActivateChars();
        }
        yield return null;
    }
    public IEnumerator Block()
    {
        yield return new WaitForSeconds(0.4f);
    }
    public IEnumerator Hit(float _damage = 3)
    {
        m_PlayerInfo.Char.height = m_PlayerInfo.GP.PlayerHeight;

        if (GameManager.Instance.GetDistance(m_PlayerInfo.GP.MinDistance))
        {
            float targetX = m_PlayerInfo.Char.gameObject.transform.localPosition.x + m_PlayerInfo.Forward * 0.8f;
            m_PlayerInfo.Char.gameObject.transform.DOLocalMoveX(targetX, 0.25f).SetEase(Ease.OutCubic);
        }

        bool tmpDamaged = false;
        for (int i = 0; i < _damage; i++)
        {
            if (!tmpDamaged)
                tmpDamaged = DamageManager.Instance.DealDamage(10, m_PlayerInfo.Forward == -1);
            yield return new WaitForEndOfFrame();
        }

        //m_PlayerInfo.Char.height = m_PlayerInfo.Input.m_controls.c ? m_PlayerInfo.GP.PlayerHeight : m_PlayerInfo.GP.CrouchHeight;
        yield return null;
    }
    #endregion

    #region -Utilities
    /// <summary>
    /// Resets the Values of the Players Animation
    /// </summary>
    public void ResetValues()
    {
        m_PlayerInfo.Ani.SetBool("Block", false);
        m_Attacking = false;
    }
    #endregion
}
