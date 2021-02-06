using System.Collections;
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
public enum EDamageStates
{
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

    AnimatorOverrideController animatorOverrideController;
    AnimationClipOverrides clipOverrides;
    #endregion

    public void Start()
    {
        if (m_PlayerInfo.Ani != null)
        {
            animatorOverrideController = new AnimatorOverrideController(m_PlayerInfo.Ani.runtimeAnimatorController);
            m_PlayerInfo.Ani.runtimeAnimatorController = animatorOverrideController;

            clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
            animatorOverrideController.GetOverrides(clipOverrides);
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
        {
            clipOverrides["Punching"] = AttackManager.Instance.clips[1];
            animatorOverrideController.ApplyOverrides(clipOverrides);

            StartCoroutine(Base(EAttackStates.F_LowAttack, Hit(EDamageStates.High, 2, true), true, 15, 3));
        }
        if (m_PlayerInfo.Input.m_attacks.b_heavy)
        {
            clipOverrides["Punching"] = AttackManager.Instance.clips[1];
            animatorOverrideController.ApplyOverrides(clipOverrides);

            StartCoroutine(Base(EAttackStates.B_HeavyAttack, Hit(EDamageStates.Middle, 2, true), false, 15, 50));
        }


        if (m_PlayerInfo.Input.m_attacks.light)
        {
            clipOverrides["Punching"] = AttackManager.Instance.clips[0];
            animatorOverrideController.ApplyOverrides(clipOverrides);

            StartCoroutine(Base(EAttackStates.F_LightAttack, Hit(EDamageStates.Middle), true));
        }
        if (m_PlayerInfo.Input.m_attacks.b_light)
        {
            clipOverrides["Punching"] = AttackManager.Instance.clips[0];
            animatorOverrideController.ApplyOverrides(clipOverrides);

            StartCoroutine(Base(EAttackStates.B_LightAttack, Hit(EDamageStates.Middle), false));
        }


        if (m_PlayerInfo.Input.m_attacks.low)
        {
            clipOverrides["Punching"] = AttackManager.Instance.clips[2];
            animatorOverrideController.ApplyOverrides(clipOverrides);

            StartCoroutine(Base(EAttackStates.F_LowAttack, Hit(EDamageStates.Low)));
        }
        if (m_PlayerInfo.Input.m_attacks.b_low)
        {
            clipOverrides["Punching"] = AttackManager.Instance.clips[2];
            animatorOverrideController.ApplyOverrides(clipOverrides);

            StartCoroutine(Base(EAttackStates.B_LowAttack, Hit(EDamageStates.Low)));
        }

        if (m_PlayerInfo.Input.m_attacks.special)
        {
            if (AttackManager.Instance.GetSpecial(m_PlayerInfo.IsLeft))
            {
                TimelineManager.Instance.Play(TimelineManager.Instance.m_TL_Special[Random.Range(0, TimelineManager.Instance.m_TL_Special.Length)]);
                AttackManager.Instance.SetSpecial(m_PlayerInfo.IsLeft, false);
            }
        }
    }

    #region -Enumerators
    public IEnumerator Base(EAttackStates _state, IEnumerator _content, bool _dash = false, float _activation = 8, float _recovery = 4)
    {
        if (m_CurrentState != _state)
        {
            m_PlayerInfo.Player.enabled = false;

            m_CurrentState = _state;
            m_PlayerInfo.Ani.SetBool("Attacking", m_Attacking = true);

            AttackManager.Instance.Dash(m_PlayerInfo, 100);

            for (int i = 0; i < _activation; i++)
                yield return new WaitForEndOfFrame();

            StartCoroutine(_content);

            for (int i = 0; i < _recovery; i++)
                yield return new WaitForEndOfFrame();

            m_CurrentState = EAttackStates.NONE;
            m_PlayerInfo.Ani.SetBool("Attacking", m_Attacking = false);

            m_PlayerInfo.Player.enabled = true;
        }
        yield return null;
    }
    public IEnumerator Block()
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
    public IEnumerator Hit(EDamageStates _damageType, float _damage = 3, bool _freezeTime = false)
    {
        bool tmpDamaged = false;
        for (int i = 0; i < _damage; i++)
        {
            if (!tmpDamaged)
                tmpDamaged = DamageManager.Instance.DealDamage(10, !m_PlayerInfo.IsLeft, _damageType);

            if (tmpDamaged)
            {
                if (_freezeTime)
                {
                    Time.timeScale = 0.1f;
                    yield return new WaitForSecondsRealtime(0.1f);
                    Time.timeScale = 1;
                }
            }

            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
    #endregion

    #region -Utilities
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
