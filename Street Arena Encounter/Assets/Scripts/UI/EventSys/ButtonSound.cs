using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class ButtonSound : MonoBehaviour
{
    GameObject m_currentGObj;
    InputSystemUIInputModule m_isuim;

    void Start()
    {
        m_isuim = GetComponent<InputSystemUIInputModule>();
    }

    void Update()
    {
        PlayMoveSound();
        PlaySelectSound();
    }

    void PlayMoveSound()
    {
        GameObject gobj = EventSystem.current.currentSelectedGameObject;
        if (gobj == null)
            return;

        if (gobj != m_currentGObj)
            AudioManager.Instance.Play(AudioManager.Instance.m_AudioInfo.m_ButtonMove);
        m_currentGObj = gobj;
    }

    void PlaySelectSound()
    {
        if (m_isuim.submit.action.triggered)
            AudioManager.Instance.Play(AudioManager.Instance.m_AudioInfo.m_ButtonSelect);
    }
}
