using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject m_panel;
    [SerializeField] TextMeshProUGUI m_timerGUI;
    [SerializeField] TextMeshProUGUI m_commentGUI;
    [SerializeField] float m_timer = 60;
    [SerializeField] float m_rounds = 1;

    void Start()
    {
        m_timerGUI.SetText((GameManager.Instance.m_Init.m_GameMode == E_GameModes.TRAINING) ? "/" : GameManager.Instance.m_Init.m_Timer.ToString());

        StartCoroutine(StartCoroutine());
    }

    IEnumerator StartCoroutine()
    {
        m_panel.SetActive(false);
        yield return new WaitForSeconds(10);
        m_panel.SetActive(true);
        StartCoroutine(Begin());
        m_timer = GameManager.Instance.m_Init.m_Timer;
        m_rounds = 1;

        yield return null;
    }

    IEnumerator Begin()
    {
        GameManager.Instance.DeactivateChars();

        m_commentGUI.SetText("In 3");
        yield return new WaitForSeconds(1);
        m_commentGUI.SetText("In 2");
        yield return new WaitForSeconds(1);
        m_commentGUI.SetText("In 1");
        yield return new WaitForSeconds(1);
        m_commentGUI.SetText("Goo");

        GameManager.Instance.STARTED = true;
        GameManager.Instance.ActivateChars();

        yield return new WaitForSeconds(1);
        m_commentGUI.gameObject.SetActive(false);
        StartCoroutine(Timer());


        yield return null;
    }

    IEnumerator Timer()
    {
        switch (GameManager.Instance.m_Init.m_GameMode)
        {
            case E_GameModes.TRAINING:
                {
                    m_timerGUI.SetText("{/}");

                    yield return null;
                }
                break;
            case E_GameModes.MULTIPLAYER:
                break;
            case E_GameModes.LOCAL:
                {
                    m_timer--;
                    m_timerGUI.SetText("{00}", m_timer);
                    yield return new WaitForSeconds(1);

                    if (m_timer > 1)
                        StartCoroutine(Timer());
                    else if (m_rounds == GameManager.Instance.m_Init.m_Rounds)
                        StartCoroutine(End());
                    else
                        StartCoroutine(Round());

                    yield return null;
                }
                break;
            default:
                break;
        }
    }

    IEnumerator End()
    {
        m_timerGUI.SetText("0");
        m_commentGUI.gameObject.SetActive(true);
        m_commentGUI.SetText(EvaluateWinner());
        GameManager.Instance.ResetPlayer();
        GameManager.Instance.STARTED = false;
        yield return new WaitForSeconds(3);

        SceneHandler.ChangeSceneByName("EndScreen_Overlay");

        yield return null;
    }
    IEnumerator Round()
    {
        m_rounds++;

        m_timerGUI.SetText("0");
        m_commentGUI.gameObject.SetActive(true);
        m_commentGUI.SetText(EvaluateWinner());
        GameManager.Instance.ResetPlayer();
        GameManager.Instance.STARTED = false;
        yield return new WaitForSeconds(3);

        m_timer = GameManager.Instance.m_Init.m_Timer;
        m_timerGUI.SetText("{00}", m_timer);
        StartCoroutine(Begin());

        yield return null;
    }

    string EvaluateWinner()
    {
        return "Draw";
    }
}
