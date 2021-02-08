using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum EAttackStates
{
    NONE,
    Block,
    F_HeavyAttack,
    B_HeavyAttack,
    F_LightAttack,
    B_LightAttack,
    F_LowAttack,
    B_LowAttack,
}
public enum EDamageStates
{
    NONE,
    High,
    Middle,
    Low,
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
    [HideInInspector] public EAttackStates m_CurrentState;
    [HideInInspector] public EDamageStates m_CurrentDamageState;

    [HideInInspector] public bool m_Attacking;

    AnimatorOverrideController m_animatorOverrideController;
    AnimationClipOverrides m_clipOverrides;
    #endregion


    void Start()
    {
        if (m_PlayerInfo.Ani != null)
        {
            m_animatorOverrideController = new AnimatorOverrideController(m_PlayerInfo.Ani.runtimeAnimatorController);
            m_PlayerInfo.Ani.runtimeAnimatorController = m_animatorOverrideController;

            m_clipOverrides = new AnimationClipOverrides(m_animatorOverrideController.overridesCount);
            m_animatorOverrideController.GetOverrides(m_clipOverrides);
        }
    }

    /// <summary>
    /// Sets the current Attack animation and state according to the Values of the InputMaster
    /// </summary>
    public void Attack()
    {
        if (m_PlayerInfo.Input.m_attacks.block)
            StartCoroutine(Block());

        if (m_PlayerInfo.Input.m_attacks.heavy)
            StartCoroutine(Base(EAttackStates.F_HeavyAttack));
        if (m_PlayerInfo.Input.m_attacks.b_heavy)
            StartCoroutine(Base(EAttackStates.B_HeavyAttack));


        if (m_PlayerInfo.Input.m_attacks.light)
            StartCoroutine(Base(EAttackStates.F_LightAttack));
        if (m_PlayerInfo.Input.m_attacks.b_light)
            StartCoroutine(Base(EAttackStates.B_LightAttack));


        if (m_PlayerInfo.Input.m_attacks.low)
            StartCoroutine(Base(EAttackStates.F_LowAttack));
        if (m_PlayerInfo.Input.m_attacks.b_low)
            StartCoroutine(Base(EAttackStates.B_LowAttack));

        if (m_PlayerInfo.Input.m_attacks.special)
            if (AttackManager.Instance.GetSpecial(m_PlayerInfo.IsLeft))
            {
                TimelineManager.Instance.Play(
                    TimelineManager.Instance.m_TL_Special[
                        Random.Range(
                            0,
                            TimelineManager.Instance.m_TL_Special.Length)]);

                AttackManager.Instance.SetSpecial(
                    m_PlayerInfo.IsLeft,
                    false);
            }
    }

    #region //Enumerators
    IEnumerator Block()
    {
        if (m_CurrentState != EAttackStates.Block)
        {
            m_CurrentState = EAttackStates.Block;
            m_PlayerInfo.Ani.SetBool("Block", m_Attacking = true);

            while (m_PlayerInfo.Input.m_attacks.block)
                yield return new WaitForEndOfFrame();

            m_CurrentState = EAttackStates.NONE;
            m_PlayerInfo.Ani.SetBool("Block", m_Attacking = false);
        }
        yield return null;
    }
    IEnumerator Base(EAttackStates _state)
    {
        if (m_CurrentState != _state)
        {
            SFrameBasedAtackSettings attack = m_PlayerInfo.ATK.Attacks[(int)_state - 2];

            StartCoroutine(Activation(
                attack.AnimationClip,
                attack.State,
                attack.Dash));

            StartCoroutine(Damage(
                attack.DamageType,
                attack.Damage_Amount,
                attack.Activation_FrameTime,
                attack.Damage_FrameTime,
                attack.FreezeTime)); ;

            StartCoroutine(Recovery(
                attack.State,
                attack.Activation_FrameTime + attack.Damage_FrameTime + attack.Recovery_FrameTime));
        }
        yield return null;
    }
    IEnumerator Activation(AnimationClip _animationClip, EAttackStates _state, bool _dash = false)
    {
        m_CurrentState = _state;

        if (_dash)
            AttackManager.Instance.Dash(m_PlayerInfo);

        m_clipOverrides["Punching"] = _animationClip;
        m_animatorOverrideController.ApplyOverrides(m_clipOverrides);

        m_PlayerInfo.Ani.SetBool("Attacking", m_Attacking = true);


        yield return null;
    }
    IEnumerator Damage(EDamageStates _damageType, float _damageAmount, float _activationFrameTime, float _damageFrameTime, bool _freezeTime = false)
    {
        for (int i = 0; i < _activationFrameTime; i++)
            yield return new WaitForEndOfFrame();


        bool tmpDamaged = false;
        for (int i = 0; i < _damageFrameTime; i++)
        {
            if (!tmpDamaged)
                tmpDamaged = DamageManager.Instance.DealDamage(
                    !m_PlayerInfo.IsLeft, 
                    _damageAmount, 
                    _damageType);

            if (tmpDamaged)
                if (_freezeTime)
                {
                    Time.timeScale = 0.1f;
                    yield return new WaitForSecondsRealtime(0.1f);
                    Time.timeScale = 1;
                }

            yield return new WaitForEndOfFrame();
        }


        yield return null;
    }
    IEnumerator Recovery(EAttackStates _state, float _recoveryFrameTime)
    {
        for (int i = 0; i < _recoveryFrameTime; i++)
            yield return new WaitForEndOfFrame();


        m_CurrentState = EAttackStates.NONE;

        m_PlayerInfo.Ani.SetBool("Attacking", m_Attacking = false);


        yield return null;
    }
    #endregion

    #region //Utilities
    /// <summary>
    /// Resets the Values of the Players Animation
    /// </summary>
    public void ResetValues()
    {
        m_CurrentState = EAttackStates.NONE;
        m_PlayerInfo.Ani.SetBool("Block", false);
        AttackManager.Instance.SetSpecial(m_PlayerInfo.IsLeft, false);
        m_Attacking = false;
    }
    #endregion
}
