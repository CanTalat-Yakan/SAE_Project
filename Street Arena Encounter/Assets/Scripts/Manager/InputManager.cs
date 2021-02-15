using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum EPIIconType
{
    KEYBOARD,
    GAMEPAD
}
public class InputManager : MonoBehaviour
{
    #region //Properties
    public static InputManager Instance { get; private set; }
    #endregion

    #region //Fields
    public GameObject m_Default_Input;
    public InputSelectionHelper m_InputSelectionInfo;

    [HideInInspector] public List<GameObject> m_DestroyGObjCollection = new List<GameObject>();
    [HideInInspector] public PlayerInputManager m_PiManager;

    [Header("Player Input")]
    [SerializeField] public PlayerInput m_PlayerL_Input;
    [SerializeField] public PlayerInput m_PlayerR_Input;

    [Header("Icon Prefabs")]
    [SerializeField] GameObject m_icon_pi_controller;
    [SerializeField] GameObject m_icon_pi_keyboard;
    #endregion


    #region //Messages
    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        m_PiManager = GetComponent<PlayerInputManager>();
        m_PiManager.gameObject.SetActive(true);
        m_PiManager.DisableJoining();
    }
    void OnDestroy()
    {
        Instance.RemoveInputs();
        Instance.m_Default_Input.SetActive(false);
        Instance.m_PiManager.enabled = true;
    }


    void OnPlayerJoined(PlayerInput _playerInput)
    {

        _playerInput.transform.SetParent(transform);

        if (_playerInput.playerIndex == 0)
        //Input_MainPlayer
        {
            m_PlayerL_Input = _playerInput;
            AudioManager.Instance.Play(AudioManager.Instance.m_AudioInfo.m_Joined);
        }
        if (_playerInput.playerIndex == 1)
        //Input_SecondPlayer
        {
            m_PlayerR_Input = _playerInput;
            AudioManager.Instance.Play(AudioManager.Instance.m_AudioInfo.m_Joined);

            m_PiManager.DisableJoining();
            m_InputSelectionInfo.ContinueButton.interactable = true;
            EventSystem.current.SetSelectedGameObject(m_InputSelectionInfo.ContinueButton.gameObject);
        }

        //Spawn Icon Indiactor for Playerinput in InputSelectionPanel
        switch (_playerInput.currentControlScheme)
        {
            case "Keyboard":
                CreateIcon(EPIIconType.KEYBOARD, m_InputSelectionInfo.List.transform);
                break;
            case "Gamepad":
                CreateIcon(EPIIconType.GAMEPAD, m_InputSelectionInfo.List.transform);
                break;
            default:
                break;
        }
    }
    #endregion

    #region //Utilities
    public InputMaster GetDefaultInput()
    {
        Instance.m_PiManager.enabled = false;
        m_Default_Input.SetActive(true);

        return m_Default_Input.GetComponent<InputMaster>();
    }
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
        foreach (GameObject go in m_DestroyGObjCollection)
            if (go != null)
                Destroy(go);


        if (m_PlayerL_Input != null)
            Destroy(m_PlayerL_Input.gameObject);
        m_PlayerL_Input = null;

        if (m_PlayerR_Input != null)
            Destroy(m_PlayerR_Input.gameObject);
        m_PlayerR_Input = null;
    }
    #endregion
}
