using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    #region //Values
    int m_timer;
    int m_rounds;
    bool? m_result;
    #endregion


    void Start()
    {
        StartCoroutine(PHASE_Awake());
    }

    void Update()
    {
        if (GameManager.Instance.STARTED)
            ReadDamage();
    }

    void ResetValues()
    {
        m_timer = GameManager.Instance.m_Init.m_Timer;
        m_rounds = 1;
    }

    #region //Coroutine
    IEnumerator PHASE_Awake()
    {
        yield return new WaitUntil(
            () => UIManager.Instance != null);

        UIManager.Instance.DeativateTimer();
        StartCoroutine(UIManager.Instance.WaitForTimeLine());

        yield return new WaitUntil(
            () => TimelineManager.Instance.m_IsPlaying == false);

        StartCoroutine(PHASE_Start());

        ResetValues();


        yield return null;
    }
    IEnumerator PHASE_Start()
    {
        GameManager.Instance.DeactivatePlayers();

        if (GameManager.Instance.m_Init.m_GameMode != EGameModes.TRAINING)
            UIManager.Instance.Setup();

        UIManager.Instance.ResetPlayer_Health();
        StartCoroutine(UIManager.Instance.CountDown(3, "Goo"));

        yield return new WaitUntil(
            () => UIManager.Instance.LOCKED == false);

        GameManager.Instance.STARTED = true;
        GameManager.Instance.ActivatePlayers();

        yield return new WaitForSeconds(1);

        UIManager.Instance.DeativateComment();

        StartCoroutine(PHASE_Loop());


        yield return null;
    }
    IEnumerator PHASE_Loop()
    {
        switch (GameManager.Instance.m_Init.m_GameMode)
        {
            case EGameModes.TRAINING:
                UIManager.Instance.DeativateTimer();
                break;
            case EGameModes.MULTIPLAYER:
                break;
            case EGameModes.LOCAL:
                {
                    yield return new WaitForSeconds(1);
                    --m_timer;

                    UIManager.Instance.SetTimer(m_timer);

                    if (m_timer != 0)
                        StartCoroutine(PHASE_Loop());
                    else
                        CheckWinner();
                }
                break;
            default:
                break;
        }


        yield return null;
    }
    IEnumerator PHASE_End_Round()
    {
        m_rounds++;

        UIManager.Instance.SetTimer(0);

        UIManager.Instance.SetComment_PlayerWon(m_result);

        DOTween.Clear();
        GameManager.Instance.STARTED = false;
        GameManager.Instance.DeactivatePlayers();

        yield return new WaitForSeconds(3);

        GameManager.Instance.ResetPlayers();

        UIManager.Instance.SetTimer(
            m_timer = GameManager.Instance.m_Init.m_Timer);

        StartCoroutine(PHASE_Start());


        yield return null;
    }
    IEnumerator PHASE_End_Game()
    {
        UIManager.Instance.SetTimer(0);

        UIManager.Instance.SetComment_PlayerWon(m_result);

        DOTween.Clear();
        GameManager.Instance.STARTED = false;
        GameManager.Instance.DeactivatePlayers();
        GameManager.Instance.ResetPlayers();

        yield return new WaitForSeconds(6);

        SceneHandler.ChangeSceneByName("EndScreen_Overlay");


        yield return null;
    }
    #endregion

    #region //Utilities
    void AddRoundWon(bool _toLeft)
    {
        if (_toLeft)
            GameManager.Instance.m_Player_L.RoundsWon++;
        else
            GameManager.Instance.m_Player_R.RoundsWon++;
    }

    void CheckWinner()
    {
        Check_Health();

        Update_RoundsWon();

        StopAllCoroutines();
        StartCoroutine(
            Check_RoundsWon() ?
                PHASE_End_Game() :
                PHASE_End_Round());

    }

    bool? Check_Health()
    {
        //Player_L Won
        if (GameManager.Instance.m_Player_L.Health > GameManager.Instance.m_Player_R.Health)
            return true;
        //Player_R Won
        if (GameManager.Instance.m_Player_L.Health < GameManager.Instance.m_Player_R.Health)
            return false;

        //Tie
        return null;
    }

    void Update_RoundsWon()
    {
        if (m_result != null)
            AddRoundWon((bool)m_result);
        else
        {
            AddRoundWon(true);
            AddRoundWon(false);
        }

        UIManager.Instance.Setup();
    }

    bool Check_RoundsWon()
    {
        return GameManager.Instance.m_Player_L.RoundsWon == GameManager.Instance.m_Init.m_Rounds
               || GameManager.Instance.m_Player_R.RoundsWon == GameManager.Instance.m_Init.m_Rounds;
    }

    void ReadDamage()
    {
        UIManager.Instance.SetPlayer_Health();

        if (Check_PlayerDead())
            CheckWinner();
    }
    bool Check_PlayerDead()
    {
        return GameManager.Instance.m_Player_L.Health <= 0
               || GameManager.Instance.m_Player_R.Health <= 0;
    }
    #endregion
}
