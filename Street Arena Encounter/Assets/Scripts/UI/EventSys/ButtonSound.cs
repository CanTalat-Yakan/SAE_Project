using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class ButtonSound : MonoBehaviour
{
    #region //Fields
    GameObject m_currentGObj;
    InputSystemUIInputModule m_isuim;
    #endregion


    void Start()
    {
        m_isuim = GetComponent<InputSystemUIInputModule>();
    }

    void Update()
    {
        PlayMoveSound();
        PlaySelectSound();
    }


    #region //Utilities
    /// <summary>
    /// Plays a certain Sound when navigating through UI-Elements
    /// </summary>
    void PlayMoveSound()
    {
        GameObject gobj = EventSystem.current.currentSelectedGameObject;
        if (gobj == null)
            return;

        if (gobj != m_currentGObj)
            AudioManager.Instance.Play(AudioManager.Instance.m_AudioInfo.m_ButtonMove);
        m_currentGObj = gobj;
    }
    /// <summary>
    /// Plays a certain Sound when selecting a UI-Element
    /// </summary>
    void PlaySelectSound()
    {
        if (m_isuim.submit.action.triggered)
            AudioManager.Instance.Play(AudioManager.Instance.m_AudioInfo.m_ButtonSelect);
    }
    #endregion
}
