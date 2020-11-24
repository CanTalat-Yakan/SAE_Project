using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject m_panel;
    [SerializeField] TextMeshProUGUI m_timerGUI;
    [SerializeField] TextMeshProUGUI m_commentGUI;
    float m_timer = 60;

    void Start()
    {
        StartCoroutine(StartCoroutine());
    }

    IEnumerator StartCoroutine()
    {
        m_panel.SetActive(false);
        yield return new WaitForSeconds(10);
        m_panel.SetActive(true);
        StartCoroutine(Comment());

        yield return null;
    }

    IEnumerator Comment()
    {
        GameManager.Instance.DeactivateChars();

        m_commentGUI.SetText("In 3");
        yield return new WaitForSeconds(1);
        m_commentGUI.SetText("In 2");
        yield return new WaitForSeconds(1);
        m_commentGUI.SetText("In 1");
        yield return new WaitForSeconds(1);
        m_commentGUI.SetText("Goo");

        GameManager.Instance.ActivateChars();

        yield return new WaitForSeconds(1);
        m_commentGUI.gameObject.SetActive(false);
        StartCoroutine(Timer());


        yield return null;
    }

    IEnumerator Timer()
    {
        m_timer--;
        m_timerGUI.SetText("{00}", m_timer);
        yield return new WaitForSeconds(1);

        if (m_timer > 0)
            StartCoroutine(Timer());
        else
            StartCoroutine(End());

        yield return null;
    }

    IEnumerator End()
    {
        m_commentGUI.gameObject.SetActive(true);
        m_commentGUI.SetText("WIN WIN");
        GameManager.Instance.DeactivateChars();
        GameManager.Instance.m_playerLEFT.gameObject.transform.position = new Vector3(-4, 0, 0);
        GameManager.Instance.m_playerRIGHT.gameObject.transform.position = new Vector3(4, 0, 0);

        yield return new WaitForSeconds(3);

        m_timerGUI.SetText("0");
        SceneHandler.ChangeSceneByName("EndScreen_Overlay");

        yield return null;
    }
}
