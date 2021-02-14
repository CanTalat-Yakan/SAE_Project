using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputSelectionManager : MonoBehaviour
{
    PlayerInputManager m_piManager;
    [SerializeField] GameObject m_list;
    [SerializeField] Button m_continueButton;


    void Awake()
    {
        m_piManager = GetComponent<PlayerInputManager>();
        DeactivateJoining();
    }

    void OnPlayerJoined(PlayerInput _playerInput)
    {
        if (!InputManager.Instance)
            Destroy(_playerInput);
        else if (!InputManager.Instance.m_RegisterInput)
            Destroy(_playerInput);
        else
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

            m_piManager.DisableJoining();
            m_continueButton.interactable = true;
            EventSystem.current.SetSelectedGameObject(m_continueButton.gameObject);
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
        m_piManager.EnableJoining();
    }
    public void DeactivateJoining()
    {
        m_piManager.DisableJoining();
        m_continueButton.interactable = false;
    }
    #endregion

}
