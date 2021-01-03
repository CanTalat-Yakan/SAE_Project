using System.Collections;
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
public class AttackController : MonoBehaviour
{
    #region -Values
    [HideInInspector] public PlayerInformation m_PlayerInfo;
    [HideInInspector] public EAttackStates m_attackState;

    [HideInInspector] public bool m_Attacking;
    #endregion

    /// <summary>
    /// Sets the current Attack animation and state according to the Values of the InputMaster
    /// </summary>
    public void Attack()
    {
        if (m_PlayerInfo.Input.m_attacks.block)
            StartCoroutine(Base("Block", EAttackStates.Block, Block(), 0.4f));

        if (m_PlayerInfo.Input.m_attacks.light)
            StartCoroutine(Base("Light", EAttackStates.F_LightAttack, Hit(), 0.2f));

        if (m_PlayerInfo.Input.m_attacks.b_light)
            StartCoroutine(Base("Light_B", EAttackStates.B_LightAttack, Hit(), 0.2f));

        if (m_PlayerInfo.Input.m_attacks.heavy)
            StartCoroutine(Base("Heavy", EAttackStates.F_LowAttack, Hit(), 0.2f));

        if (m_PlayerInfo.Input.m_attacks.b_heavy)
            StartCoroutine(Base("Heavy_B", EAttackStates.B_HeavyAttack, Hit(), 0.2f));

        if (m_PlayerInfo.Input.m_attacks.low)
            StartCoroutine(Base("Low", EAttackStates.F_LowAttack, Hit(), 0.2f));

        if (m_PlayerInfo.Input.m_attacks.b_low)
            StartCoroutine(Base("Low_B", EAttackStates.B_LowAttack, Hit(), 0.2f));
    }

    void LateUpdate()
    {
        m_attackState = EAttackStates.NONE;
    }

    #region -Enumerators
    public IEnumerator Base(string _parameterTag, EAttackStates _state, IEnumerator _content, float _duration)
    {
        if (m_attackState != _state)
        {
            m_PlayerInfo.Ani.SetTrigger("Attack");
            m_attackState = _state;

            m_Attacking = true;
            m_PlayerInfo.Ani.SetBool(_parameterTag, true);

            StartCoroutine(_content);
            yield return new WaitForSeconds(_duration);

            m_Attacking = false;
            m_PlayerInfo.Ani.SetBool(_parameterTag, false);

            m_attackState = EAttackStates.NONE;
        }
        yield return null;
    }
    public IEnumerator Block()
    {
        yield return new WaitForSeconds(0.4f);
    }
    public IEnumerator Hit(float _dealDamageAfter = 0.1f)
    {
        yield return new WaitForSeconds(0.1f);

        if (GameManager.Instance.GetDistance(2))
        {
            float targetX = m_PlayerInfo.Player.transform.localPosition.x + m_PlayerInfo.Forward * 0.8f;
            m_PlayerInfo.Player.transform.DOLocalMoveX(targetX, 0.25f).SetEase(Ease.OutCubic);
        }

        yield return new WaitForSeconds(_dealDamageAfter - 0.1f);
        DamageManager.Instance.DealDamage(10, m_PlayerInfo.Forward == -1);

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
