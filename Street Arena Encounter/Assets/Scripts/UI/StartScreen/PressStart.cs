using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class PressStart : MonoBehaviour
{
    [SerializeField] InputSystemUIInputModule m_uiInput;

    void Update()
    {
        if (m_uiInput.submit.action.triggered)
            SceneHandler.ChangeSceneByName("Menu");
    }
}
