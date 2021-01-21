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
    [SerializeField] GameObject m_pi_controller;
    [SerializeField] GameObject m_pi_keyboard;
    [SerializeField] Main_Init m_Init;

    private void Awake()
    {
        m_pi_manager = GetComponent<PlayerInputManager>();
        m_pi_manager.DisableJoining();
        m_continue_Button.interactable = false;
    }

    void OnPlayerJoined(PlayerInput _playerInput)
    {
        _playerInput.gameObject.name += _playerInput.playerIndex.ToString();

        if (_playerInput.playerIndex == 0)
        {
            m_Init.m_Player_L.Input = _playerInput;
        }
        if (_playerInput.playerIndex == 1)
        {
            m_Init.m_Player_R.Input = _playerInput;
            m_pi_manager.DisableJoining();
            m_continue_Button.interactable = true;
            EventSystem.current.SetSelectedGameObject(m_continue_Button.gameObject);
        }

        if (_playerInput.currentControlScheme == "Gamepad")
            Instantiate(m_pi_controller, m_list.transform);
        if (_playerInput.currentControlScheme == "Keyboard")
            Instantiate(m_pi_keyboard, m_list.transform);
    }

    public void ActivateJoining()
    {
        m_pi_manager.EnableJoining();
    }
}
