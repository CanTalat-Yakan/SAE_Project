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
    #region //Values
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
            if (AttackManager.Instance.m_AnimatorOverrideController != null)
                m_animatorOverrideController = AttackManager.Instance.m_AnimatorOverrideController;
            else
                m_animatorOverrideController = new AnimatorOverrideController(m_PlayerInfo.Ani.runtimeAnimatorController);
            m_PlayerInfo.Ani.runtimeAnimatorController = m_animatorOverrideController;

            m_clipOverrides = new AnimationClipOverrides(m_animatorOverrideController.overridesCount);
            m_animatorOverrideController.GetOverrides(m_clipOverrides);
        }
    }


    #region //Utilities
    /// <summary>
    /// Sets the current Attack animation and state according to the Values of the InputMaster
    /// </summary>
    public void Attack()
    {
        if (m_Attacking)
        {
            ComboAttack();
            return;
        }

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
            StartCoroutine(Special());
    }
    void ComboAttack()
    {

    }
    /// <summary>
    /// Resets the Values of the Players Animation
    /// </summary>
    public void ResetValues()
    {
        m_CurrentState = EAttackStates.NONE;
        m_Attacking = false;

        m_PlayerInfo.Ani.SetBool("Block", false);
        m_PlayerInfo.Ani.SetBool("Attacking", false);

        AttackManager.Instance.DeactivateSpecialVFX(m_PlayerInfo.IsLeft);
    }
    #endregion

    #region //Enumerators
    IEnumerator Block()
    {
        m_CurrentState = EAttackStates.Block;
        m_PlayerInfo.Ani.SetBool("Block", m_Attacking = true);

        yield return new WaitUntil(
            () => m_PlayerInfo.Input.m_attacks.block == false);

        m_CurrentState = EAttackStates.NONE;
        m_PlayerInfo.Ani.SetBool("Block", m_Attacking = false);
        yield return null;
    }
    IEnumerator Special()
    {
        if (m_PlayerInfo.IsLeft)
            m_PlayerInfo = GameManager.Instance.m_Player_L;
        else
            m_PlayerInfo = GameManager.Instance.m_Player_R;

        if (m_PlayerInfo.Special)
        {
            AudioManager.Instance.Play(
                AudioManager.Instance.m_AudioInfo.m_Special_Activation,
                1.5f);

            if (DamageManager.Instance.DealDamage(
                    !m_PlayerInfo.IsLeft,
                    0,
                    1,
                    EDamageStates.Middle))
                TimelineManager.Instance.Play(
                    m_PlayerInfo.IsLeft
                        ? TimelineManager.Instance.m_TimeLineInfo.m_TL_Special_L[
                            Random.Range(
                                0,
                                TimelineManager.Instance.m_TimeLineInfo.m_TL_Special_L.Length)]
                        : TimelineManager.Instance.m_TimeLineInfo.m_TL_Special_R[
                            Random.Range(
                                0,
                                TimelineManager.Instance.m_TimeLineInfo.m_TL_Special_R.Length)]);

            AttackManager.Instance.DeactivateSpecialVFX(m_PlayerInfo.IsLeft);

            m_PlayerInfo.Special = false;
        }

        if (m_PlayerInfo.IsLeft)
            GameManager.Instance.m_Player_L = m_PlayerInfo;
        else
            GameManager.Instance.m_Player_R = m_PlayerInfo;


        yield return null;
    }
    IEnumerator Base(EAttackStates _state)
    {
        SFrameBasedAtackSettings attack =
            m_PlayerInfo.ATK.Attacks[
                (int)_state - 2];

        StartCoroutine(Activation(
            attack.AnimationClip,
            attack.State,
            attack.Dash));

        StartCoroutine(Damage(
            attack.DamageType,
            attack.Damage_Amount,
            attack.Damage_Range,
            attack.Activation_FrameTime,
            attack.Damage_FrameTime,
            attack.FreezeTime)); ;

        StartCoroutine(Recovery(
            attack.State,
            attack.Activation_FrameTime + attack.Damage_FrameTime + attack.Recovery_FrameTime));
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
    IEnumerator Damage(EDamageStates _damageType, float _damageAmount, float _range, float _activationFrameTime, float _damageFrameTime, bool _freezeTime = false)
    {
        for (int i = 0; i < _activationFrameTime; i++)
            yield return new WaitForEndOfFrame();


        bool damaged = false;
        for (int i = 0; i < _damageFrameTime; i++)
        {
            if (damaged = DamageManager.Instance.DealDamage(
                   !m_PlayerInfo.IsLeft,
                   _damageAmount,
                   _range,
                   _damageType))
                break;

            yield return new WaitForEndOfFrame();
        }

        if (damaged)
        {
            PlaySound(_damageType);

            if (_freezeTime)
            {
                Time.timeScale = 0.1f;
                yield return new WaitForSecondsRealtime(0.1f);
                Time.timeScale = 1;
            }
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

    #region //Helper
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
}
