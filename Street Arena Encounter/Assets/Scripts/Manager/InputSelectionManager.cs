using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputSelectionManager : MonoBehaviour
{
    PlayerInputManager m_pi_manager;
    [SerializeField] GameObject m_list;
    [SerializeField] Button m_continue_Button;
    [SerializeField] Main_Init m_Init;


    void Awake()
    {
        m_pi_manager = GetComponent<PlayerInputManager>();
        m_pi_manager.DisableJoining();

        m_continue_Button.interactable = false;
    }

    void OnPlayerJoined(PlayerInput _playerInput)
    {
        CreateInput(_playerInput);
    }

    #region //Utilities
    void CreateInput(PlayerInput _playerInput)
    {
        _playerInput.transform.SetParent(InputManager.Instance.gameObject.transform);

        if (_playerInput.playerIndex == 0)
        //Input_MainPlayer
        {
            InputManager.Instance.m_Player_L_Input = _playerInput;
            AudioManager.Instance.Play(AudioManager.Instance.m_AudioInfo.m_Joined);
        }
        if (_playerInput.playerIndex == 1)
        //Input_SecondPlayer
        {
            InputManager.Instance.m_Player_R_Input = _playerInput;
            AudioManager.Instance.Play(AudioManager.Instance.m_AudioInfo.m_Joined);

            m_pi_manager.DisableJoining();
            m_continue_Button.interactable = true;
            EventSystem.current.SetSelectedGameObject(m_continue_Button.gameObject);
        }

        //Spawn Icon Indiactor for Playerinput in InputSelectionPanel
        switch (_playerInput.currentControlScheme)
        {
            case "Keyboard":
                InputManager.Instance.CreateIcon(EPIIconType.KEYBOARD, m_list.transform);
                break;
            case "Gamepad":
                InputManager.Instance.CreateIcon(EPIIconType.GAMEPAD, m_list.transform);
                break;
            default:
                break;
        }
    }
    public void ActivateJoining()
    {
        m_pi_manager.EnableJoining();
    }
    #endregion

}
