using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class UI_RoundManager : MonoBehaviour
{
    #region -Values
    [SerializeField] GameObject m_panel;
    [SerializeField] TextMeshProUGUI m_timerGUI;
    [SerializeField] TextMeshProUGUI m_commentGUI;

    [SerializeField] Image m_playerHealthBar_L;
    [SerializeField] Image m_playerHealthBarShadow_L;
    [SerializeField] Image m_playerHealthBar_R;
    [SerializeField] Image m_playerHealthBarShadow_R;
    [SerializeField] GameObject m_roundsGO_L;
    [SerializeField] GameObject m_roundsGO_R;

    [SerializeField] float m_timer = 60;
    [SerializeField] float m_rounds = 1;

    float tmpValue, tmpValue2;
    string tmpPlayerWON;
    #endregion

    void Start()
    {
        StartCoroutine(StartCoroutine());
    }

    void Update()
    {
        if (GameManager.Instance.STARTED)
            ReadDamage();
    }

    #region -Coroutine
    IEnumerator StartCoroutine()
    {
        m_timerGUI.SetText((GameManager.Instance.m_Init.m_GameMode == EGameModes.TRAINING) ? "/" : GameManager.Instance.m_Init.m_Timer.ToString());
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
        if (GameManager.Instance.m_Init.m_GameMode != EGameModes.TRAINING)
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
            case EGameModes.TRAINING:
                {
                    m_timerGUI.SetText("/");

                    yield return null;
                }
                break;
            case EGameModes.MULTIPLAYER:
                break;
            case EGameModes.LOCAL:
                {
                    yield return new WaitForSeconds(1);

                    m_timer--;
                    m_timerGUI.SetText("{00}", m_timer);

                    if (m_timer != 0)
                        StartCoroutine(Timer());
                    else if (ResultWinner())
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
    IEnumerator Round()
    {
        m_rounds++;

        m_timerGUI.SetText("0");
        m_commentGUI.gameObject.SetActive(true);
        m_commentGUI.SetText(tmpPlayerWON);
        DOTween.Clear();
        GameManager.Instance.DeactivateChars();
        GameManager.Instance.ResetPlayers();
        GameManager.Instance.STARTED = false;

        yield return new WaitForSeconds(3);

        m_timer = GameManager.Instance.m_Init.m_Timer;
        m_timerGUI.SetText("{00}", m_timer);

        StartCoroutine(Begin());

        yield return null;
    }
    IEnumerator End()
    {
        m_timerGUI.SetText("0");
        m_commentGUI.gameObject.SetActive(true);
        m_commentGUI.SetText(EvaluateWinner_End());
        DOTween.Clear();
        GameManager.Instance.DeactivateChars();
        GameManager.Instance.ResetPlayers();
        GameManager.Instance.STARTED = false;
        yield return new WaitForSeconds(6);

        SceneHandler.ChangeSceneByName("EndScreen_Overlay");

        yield return null;
    }
    #endregion

    #region -Utilities
    string EvaluateWinner_Round()
    {
        //Player Left Won
        if (GameManager.Instance.m_Player_L.Health > GameManager.Instance.m_Player_R.Health)
        {
            GameManager.Instance.m_Player_L.RoundsWon++;
            return GameManager.Instance.m_Player_L.Name + "\nWon";
        }
        //Player Right Won
        if (GameManager.Instance.m_Player_L.Health < GameManager.Instance.m_Player_R.Health)
        {
            GameManager.Instance.m_Player_R.RoundsWon++;
            return GameManager.Instance.m_Player_R.Name + "\nWon";
        }

        GameManager.Instance.m_Player_L.RoundsWon++;
        GameManager.Instance.m_Player_R.RoundsWon++;
        return "Draw";
    }
    string EvaluateWinner_End()
    {
        //Draw
        if (GameManager.Instance.m_Player_L.RoundsWon == GameManager.Instance.m_Player_R.RoundsWon)
            return "Draw";

        //Player Left Won
        if (GameManager.Instance.m_Player_L.RoundsWon == GameManager.Instance.m_Init.m_Rounds)
            return GameManager.Instance.m_Player_L.Name + "\nWon";
        //Player Right Won
        if (GameManager.Instance.m_Player_R.RoundsWon == GameManager.Instance.m_Init.m_Rounds)
            return GameManager.Instance.m_Player_R.Name + "\nWon";

        return "Draw";
    }
    bool ResultWinner()
    {
        tmpPlayerWON = EvaluateWinner_Round();
        SetupRound();

        //End
        if (GameManager.Instance.m_Player_L.RoundsWon == GameManager.Instance.m_Init.m_Rounds
            || GameManager.Instance.m_Player_R.RoundsWon == GameManager.Instance.m_Init.m_Rounds)
            return true;

        return false;
    }
    bool PlayerDead()
    {
        if (GameManager.Instance.m_Player_L.RoundsWon == GameManager.Instance.m_Init.m_Rounds
            || GameManager.Instance.m_Player_R.RoundsWon == GameManager.Instance.m_Init.m_Rounds)
            return false;

        //Player Left Dead
        if (GameManager.Instance.m_Player_L.Health <= 0
            || GameManager.Instance.m_Player_R.Health <= 0)
        {
            ResultWinner();
            return true;
        }

        return false;
    }
    void ReadDamage()
    {
        if (tmpValue != (m_playerHealthBar_L.fillAmount = GameManager.Instance.m_Player_L.Health / GameManager.Instance.m_Player_L.GP.Health))
            StartCoroutine(ShadowAni());
        if (tmpValue2 != (m_playerHealthBar_R.fillAmount = GameManager.Instance.m_Player_R.Health / GameManager.Instance.m_Player_R.GP.Health))
            StartCoroutine(ShadowAni());

        tmpValue = m_playerHealthBar_L.fillAmount;
        tmpValue2 = m_playerHealthBar_R.fillAmount;

        if (PlayerDead())
        {
            StopAllCoroutines();
            StartCoroutine(Round());
        }
    }
    void SetupRound()
    {
        switch (GameManager.Instance.m_Init.m_GameMode)
        {
            case EGameModes.SOLO:
                break;
            case EGameModes.MULTIPLAYER:
                break;
            case EGameModes.LOCAL:
                {
                    m_roundsGO_L.transform.GetChild(0).gameObject.SetActive(true);
                    m_roundsGO_R.transform.GetChild(0).gameObject.SetActive(true);
                    GameObject go = m_roundsGO_L.transform.GetChild(0).gameObject;

                    if (m_roundsGO_L.transform.childCount != GameManager.Instance.m_Init.m_Rounds)
                        for (int i = 1; i < GameManager.Instance.m_Init.m_Rounds; i++)
                            Instantiate(go, m_roundsGO_L.transform);
                    if (m_roundsGO_R.transform.childCount != GameManager.Instance.m_Init.m_Rounds)
                        for (int i = 1; i < GameManager.Instance.m_Init.m_Rounds; i++)
                            Instantiate(go, m_roundsGO_R.transform);

                    for (int i = 0; i < GameManager.Instance.m_Player_L.RoundsWon; i++)
                        if (m_roundsGO_L.transform.childCount >= i)
                            m_roundsGO_L.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
                    for (int i = 0; i < GameManager.Instance.m_Player_R.RoundsWon; i++)
                        if (m_roundsGO_R.transform.childCount >= i)
                            m_roundsGO_R.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
                }
                break;
            case EGameModes.TRAINING:
                {

                }
                break;
            default:
                break;
        }
    }
    #endregion

    #region -Enumerators
    IEnumerator ShadowAni()
    {
        yield return new WaitForSeconds(1);

        DOTween.To(() => m_playerHealthBarShadow_L.fillAmount,
            x => m_playerHealthBarShadow_L.fillAmount = x,
                 m_playerHealthBar_L.fillAmount,
                 0.2f);

        DOTween.To(() => m_playerHealthBarShadow_R.fillAmount,
            x => m_playerHealthBarShadow_R.fillAmount = x,
                 m_playerHealthBar_R.fillAmount,
                 0.2f);

        yield return null;
    }
    #endregion
}
