using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class TabManager : MonoBehaviour
{
    #region //Variables
    [SerializeField] bool firstPanelOpen = true;
    [SerializeField] InputSystemUIInputModule m_uiInput;
    [SerializeField] List<GameObject> m_panels = new List<GameObject>();

    int? m_panelIndex = 0;
    int m_previousPanelIndex;
    #endregion

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_uiInput = FindObjectOfType<InputSystemUIInputModule>();
        m_previousPanelIndex = m_panelIndex.Value;

        if (m_panels.Count == 0)
            return;

        if (firstPanelOpen)
        {
            m_panels[0].gameObject.SetActive(true);

            if (m_panels[0].GetComponent<FirstSelected>() != null)
                EventSystem.current.SetSelectedGameObject(m_panels[0].GetComponent<FirstSelected>().m_firstSelected);

            m_panelIndex = 0;
            for (int i = 1; i < m_panels.Count; i++)
                m_panels[i].gameObject.SetActive(false);
        }
        else
            CloseAllPanelsAndIndexNull(true);
    }

    void Update()
    {
        if (m_panels.Count == 0)
            return;

        if (m_uiInput.cancel.action.ReadValue<float>() != 0)
        {
            if (m_panelIndex <= 3)
                SetPreviousIndex();
            else
            {
                SetPageIndex(0);
                SetPageIndex(0);
            }
        }

        if (m_panelIndex == 3)
            InputManager.Instance.RemoveInputs();
    }

    #region //Utilities
    void ShowCurrentPanel()
    {
        for (int i = 0; i < m_panels.Count; i++)
            if (i == m_panelIndex.Value && m_panelIndex != null)
            {
                if (m_panels[i])
                {
                    m_panels[i].SetActive(true);
                    if (m_panels[i].GetComponent<FirstSelected>() != null)
                        EventSystem.current.SetSelectedGameObject(m_panels[i].GetComponent<FirstSelected>().m_firstSelected);
                }
            }
            else if (m_panels[i] != null)
            {
                CheckForTabManagersInHierachy(i);
                m_panels[i].gameObject.SetActive(false);
            }
    }

    public void SetPageIndex(int _index)
    {
        m_previousPanelIndex = m_panelIndex.Value;

        //when closing current panel, set index from  null to tmpIndex and show Panels again
        if (m_panelIndex == null)
        {
            m_panelIndex = m_previousPanelIndex;
            ShowCurrentPanel();
        }

        //update panelIndex
        m_panelIndex = _index;

        //show  current panel onyl  one  time
        if (m_previousPanelIndex != m_panelIndex)
            ShowCurrentPanel();
    }

    public void CloseAllPanelsAndIndexNull(bool _tmp)
    {
        if (!_tmp)
            return;

        for (int i = 0; i < m_panels.Count; i++)
            m_panels[i].gameObject.SetActive(false);

        m_panelIndex = null;
        _tmp = false;
    }

    public void SetPreviousIndex()
    {
        if (SceneManager.GetSceneByName("Settings_Overlay").isLoaded)
            SceneManager.UnloadSceneAsync("Settings_Overlay");

        m_panelIndex = m_previousPanelIndex;
        ShowCurrentPanel();
    }

    void CheckForTabManagersInHierachy(int _i)
    {
        TabManager tmpTM;
        if (tmpTM = m_panels[_i].GetComponentInChildren<TabManager>())
            tmpTM.ResetIndex();
    }

    public void ResetIndex()
    {
        m_panelIndex = 0;
        m_previousPanelIndex = 0;
        ShowCurrentPanel();
    }
    #endregion
}
