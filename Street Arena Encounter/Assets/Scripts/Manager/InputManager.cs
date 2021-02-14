using System;
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

    public bool m_RegisterInput { get; set; }
    [HideInInspector] public List<GameObject> m_DestroyGObjCollection = new List<GameObject>();
    [HideInInspector] public PlayerInputManager m_PiManager;

    [HideInInspector] public PlayerInput m_Player_L_Input;
    [HideInInspector] public PlayerInput m_Player_R_Input;
    [SerializeField] GameObject m_icon_pi_controller;
    [SerializeField] GameObject m_icon_pi_keyboard;


    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        m_PiManager = FindObjectOfType<PlayerInputManager>();
    }
    public void OnDestroy()
    {
        RemoveInputs();
    }

    #region //Utilities
    public void CreateIcon(EPIIconType _piIconType, Transform _parent)
    {
        switch (_piIconType)
        {
            case EPIIconType.KEYBOARD:
                m_DestroyGObjCollection.Add(Instantiate(m_icon_pi_keyboard, _parent));
                break;
            case EPIIconType.GAMEPAD:
                m_DestroyGObjCollection.Add(Instantiate(m_icon_pi_controller, _parent));
                break;
            default:
                break;
        }
    }

    public void RemoveInputs()
    {
        for (int i = 0; i < m_DestroyGObjCollection.Count; i++)
            Destroy(m_DestroyGObjCollection[i]);

        if (m_Player_L_Input != null)
            Destroy(m_Player_L_Input);
        if (m_Player_R_Input != null)
            Destroy(m_Player_R_Input);
    }
    #endregion
}
