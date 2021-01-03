using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region -Values
    [SerializeField] GameObject m_panel;
    [SerializeField] TextMeshProUGUI m_timerGUI;
    [SerializeField] TextMeshProUGUI m_commentGUI;
    [SerializeField] float m_timer = 60;
    [SerializeField] float m_rounds = 1;
    [SerializeField] Image m_playerHealthBar_L;
    [SerializeField] Image m_playerHealthBar_R;
    [SerializeField] GameObject m_roundsGO_L;
    [SerializeField] GameObject m_roundsGO_R;
    #endregion

    void Start()
    {
        StartCoroutine(StartCoroutine());
    }

    void Update()
    {
        ReadDamage();
    }

    #region -Coroutine
    IEnumerator StartCoroutine()
    {
        m_timerGUI.SetText((GameManager.Instance.m_Init.m_GameMode == E_GameModes.TRAINING) ? "/" : GameManager.Instance.m_Init.m_Timer.ToString());
        m_panel.SetActive(false);
        if (!GameManager.Instance.m_SkipIntro)
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
        SetupRound();

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
                    else if (m_rounds == GameManager.Instance.m_Init.m_Rounds * 2 - 1)
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
        GameManager.Instance.ResetPlayers();
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
        GameManager.Instance.ResetPlayers();
        GameManager.Instance.STARTED = false;
        yield return new WaitForSeconds(3);

        m_timer = GameManager.Instance.m_Init.m_Timer;
        m_timerGUI.SetText("{00}", m_timer);
        StartCoroutine(Begin());

        yield return null;
    }
    #endregion

    #region -Utilities
    string EvaluateWinner()
    {
        //End
        if (GameManager.Instance.m_Player_L.RoundsWon == GameManager.Instance.m_Init.m_Rounds)
            return "Player Left\nWon";
        //End
        if (GameManager.Instance.m_Player_R.RoundsWon == GameManager.Instance.m_Init.m_Rounds)
            return "Player Right\nWon";

        //Round
        if (GameManager.Instance.m_Player_L.Health > GameManager.Instance.m_Player_R.Health)
            return "Player Left\nWon";
        //Round
        if (GameManager.Instance.m_Player_L.Health < GameManager.Instance.m_Player_R.Health)
            return "Player Left\nWon";
        //Round
        if (GameManager.Instance.m_Player_L.Health == GameManager.Instance.m_Player_R.Health)
            return "Draw";

        return "Draw";
    }
    void SetupRound()
    {
        GameObject go = m_roundsGO_L.transform.GetChild(0).gameObject;

        if (m_roundsGO_L.transform.childCount != GameManager.Instance.m_Init.m_Rounds)
            for (int i = 1; i < GameManager.Instance.m_Init.m_Rounds; i++)
                Instantiate(go, m_roundsGO_L.transform);
        if (m_roundsGO_R.transform.childCount != GameManager.Instance.m_Init.m_Rounds)
            for (int i = 1; i < GameManager.Instance.m_Init.m_Rounds; i++)
                Instantiate(go, m_roundsGO_R.transform);

        for (int i = 0; i < GameManager.Instance.m_Player_L.RoundsWon; i++)
            m_roundsGO_L.transform.GetChild(i);
        for (int i = 0; i < GameManager.Instance.m_Player_R.RoundsWon; i++)
            m_roundsGO_R.transform.GetChild(i);
    }
    void ReadDamage()
    {
        m_playerHealthBar_L.fillAmount = GameManager.Instance.m_Player_L.Health / GameManager.Instance.m_Player_L.GP.PlayerHealth;
        m_playerHealthBar_R.fillAmount = GameManager.Instance.m_Player_L.Health / GameManager.Instance.m_Player_R.GP.PlayerHealth;
    }
    #endregion
}
