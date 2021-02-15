using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

/// <summary>
/// https://docs.unity3d.com/ScriptReference/AnimatorOverrideController.html
/// </summary>
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
    #region //Fields
    [HideInInspector] public PlayerInformation m_PlayerInfo;

    [HideInInspector] public bool m_Attacking;

    AnimatorOverrideController m_animatorOverrideController;
    AnimationClipOverrides m_clipOverrides;
    #endregion

    #region //Properties
    [HideInInspector] public EAttackStates m_CurrentState { get; private set; }
    [HideInInspector] public EDamageStates m_CurrentDamageState { get; private set; }
    #endregion


    void Start()
    {
        if (m_PlayerInfo.Ani != null)
        {
            if (m_PlayerInfo.AOC)
                m_animatorOverrideController = m_PlayerInfo.AOC;
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

        yield return new WaitForSeconds(0.3f);

        m_CurrentState = EAttackStates.NONE;
        m_PlayerInfo.Ani.SetBool("Block", m_Attacking = false);


        yield return null;
    }
    IEnumerator Special()
    {
        if (!m_PlayerInfo.Special)
            yield return null;


        m_PlayerInfo.Special = false;
        m_Attacking = true;

        AudioManager.Instance.Play(
            AudioManager.Instance.m_AudioInfo.m_Special_Activation,
            1.5f);


        bool damaged = DamageManager.Instance.DealDamage(
                !m_PlayerInfo.IsLeft,
                0,
                2,
                EDamageStates.Middle);

        if (!damaged)
        {
            yield return new WaitForSeconds(1);
            m_Attacking = false;
        }
        else
        {
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

            StartCoroutine(UIManager.Instance.WaitForTimeLine());

            yield return new WaitUntil(
                () => TimelineManager.Instance.m_IsPlaying == false);

            if (m_PlayerInfo.IsLeft)
                GameManager.Instance.m_Player_R.Health -= 35;
            else
                GameManager.Instance.m_Player_L.Health -= 35;

            UIManager.Instance.UpdatePlayer_Health();
        }

        m_Attacking = false;
        AttackManager.Instance.DeactivateSpecialVFX(m_PlayerInfo.IsLeft);
        DamageManager.Instance.FallBack(!m_PlayerInfo.IsLeft);


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
            if (_freezeTime)
            {
                Time.timeScale = 0.1f;

                for (int i = 0; i < 10; i++)
                    yield return new WaitForEndOfFrame();

                Time.timeScale = 1;
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
}