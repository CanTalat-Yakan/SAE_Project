using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputSelectionHelper : MonoBehaviour
{
    public GameObject List;
    public Button ContinueButton;

    void Start()
    {
        InputManager.Instance.m_InputSelectionInfo = this;
    }

    public void EnableJoining()
    {
        InputManager.Instance.m_PiManager.EnableJoining();
    }
    public void DisableJoining()
    {
        InputManager.Instance.m_PiManager.DisableJoining();
    }
}
