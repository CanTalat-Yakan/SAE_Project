using UnityEngine;
using UnityEngine.InputSystem;

public struct SControls
{
    public float m;
    public bool j;
    public bool c;
    public bool d;
}
public struct SAttacks
{
    public bool light;
    public bool b_light;
    public bool heavy;
    public bool b_heavy;
    public bool low;
    public bool b_low;

    public bool block;

    public void ResetValues()
    {
        light =
        b_light =
        heavy =
        b_heavy =
        low =
        b_low =
        block = false;
    }
}
public class InputMaster : MonoBehaviour
{
    #region -Values
    [HideInInspector] public PlayerInput m_input;
    public SControls m_controls;
    public SAttacks m_attacks;
    #endregion

    void Awake()
    {
        m_input = GetComponent<PlayerInput>();
    }

    void LateUpdate()
    {
        m_attacks.ResetValues();
    }

    void OnPlayerJoined()
    {

    }

    #region --Movement
    void OnMovement(InputValue _i)
    {
        m_controls.m = _i.Get<Vector2>().x;
        m_controls.j = _i.Get<Vector2>().y > 0;
        m_controls.c = _i.Get<Vector2>().y < 0;
    }
    void OnLStick(InputValue _i)
    {
        m_controls.m = _i.Get<Vector2>().x > 0.25f ? 1 : _i.Get<Vector2>().x < -0.25f ? -1 : 0;
        m_controls.j = _i.Get<Vector2>().y > 0.75f ? true : false;
        m_controls.c = _i.Get<Vector2>().y < -0.75f ? true : false;
    }
    void OnDPad(InputValue _i)
    {
        m_controls.m = _i.Get<Vector2>().x;
        m_controls.j = _i.Get<Vector2>().y == 1 ? true : false;
        m_controls.c = _i.Get<Vector2>().y == -1 ? true : false;
    }
    //void OnDash(InputValue _i)
    //{
    //    m_controls.d = _i.Get<float>() != 0;
    //}
    #endregion

    #region --Attacks
    void OnLight(InputValue _i)
    {
        if (m_controls.m < 0)
            m_attacks.b_light = true;
        else
            m_attacks.light = true;
    }
    void OnHeavy(InputValue _i)
    {
        if (m_controls.m < 0)
            m_attacks.b_heavy = true;
        else
            m_attacks.heavy = true;
    }
    void OnBlock(InputValue _i)
    {
        m_attacks.block = true;
    }
    void OnLow(InputValue _i)
    {
        if (m_controls.m < 0)
            m_attacks.b_low = true;
        else
            m_attacks.low = true;
    }
    #endregion
}
