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
    public bool special;

    public bool block;

    public void ResetValues()
    {
        light =
        b_light =
        heavy =
        b_heavy =
        low =
        b_low = 
        special = false;
    }
}
public class InputMaster : MonoBehaviour
{
    #region //Values
    [HideInInspector] public PlayerInput m_input;
    public SControls m_movement;
    public SAttacks m_attacks;
    #endregion

    void Awake()
    {
        m_input = GetComponent<PlayerInput>();
    }

    void LateUpdate()
    {
        m_attacks.ResetValues();
        m_movement.j = false;
    }

    #region //Movement
    void OnMovement(InputValue _i)
    {
        m_movement.m = _i.Get<Vector2>().x;
        m_movement.j = _i.Get<Vector2>().y > 0;
        m_movement.c = _i.Get<Vector2>().y < 0;
    }
    void OnLStick(InputValue _i)
    {
        m_movement.m = _i.Get<Vector2>().x > 0.25f ? 1 : _i.Get<Vector2>().x < -0.25f ? -1 : 0;
        m_movement.j = _i.Get<Vector2>().y > 0.75f ? true : false;
        m_movement.c = _i.Get<Vector2>().y < -0.75f ? true : false;
    }
    void OnDPad(InputValue _i)
    {
        m_movement.m = _i.Get<Vector2>().x;
        m_movement.j = _i.Get<Vector2>().y == 1 ? true : false;
        m_movement.c = _i.Get<Vector2>().y == -1 ? true : false;
    }
    //void OnDash(InputValue _i)
    //{
    //    m_controls.d = _i.Get<float>() != 0;
    //}
    #endregion

    #region //Attacks
    void OnLight(InputValue _i)
    {
        if (m_movement.m < 0)
            m_attacks.b_light = true;
        else
            m_attacks.light = true;
    }
    void OnHeavy(InputValue _i)
    {
        if (m_movement.m < 0)
            m_attacks.b_heavy = true;
        else
            m_attacks.heavy = true;
    }
    void OnBlock(InputValue _i)
    {
        m_attacks.block = _i.Get<float>() == 1;
    }
    void OnLow(InputValue _i)
    {
        if (m_movement.m < 0)
            m_attacks.b_low = true;
        else
            m_attacks.low = true;
    }

    void OnSpecial(InputValue _i)
    {
        m_attacks.special = true;
    }
    #endregion
}
