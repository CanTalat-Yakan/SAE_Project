using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EPIIconType
{
    KEYBOARD,
    GAMEPAD
}
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public PlayerInput m_Player_L_Input;
    public PlayerInput m_Player_R_Input;
    [SerializeField] GameObject m_icon_pi_controller;
    [SerializeField] GameObject m_icon_pi_keyboard;


    void Awake()
    {
        if (Instance)
            Destroy(Instance.gameObject);
        Instance = this;
    }
    public void OnDestroy()
    {
        Destroy(m_Player_L_Input);
        Destroy(m_Player_R_Input);
    }

    public void CreateIcon(EPIIconType _piIconType, Transform _parent)
    {
        switch (_piIconType)
        {
            case EPIIconType.KEYBOARD:
                Instantiate(m_icon_pi_keyboard, _parent);
                break;
            case EPIIconType.GAMEPAD:
                Instantiate(m_icon_pi_controller, _parent);
                break;
            default:
                break;
        }
    }
}
