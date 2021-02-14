using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    #region //Values
    int m_timer = 0;
    int m_currentRound = 1;
    bool? m_roundResult = null;
    bool? m_endResult = null;
    bool m_endResult_Tie = false;
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
        m_currentRound = 1;
        m_roundResult = null;
    }

    #region //Coroutine
    IEnumerator PHASE_Awake()
    {
        ResetValues();

        yield return new WaitUntil(
            () => UIManager.Instance != null);

        if (!GameManager.Instance.m_Init.m_SkipIntro)
            if (GameManager.Instance.m_Init.m_GameMode != EGameModes.TRAINING)
                TimelineManager.Instance.Play(
                    TimelineManager.Instance.m_TimeLineInfo.m_TL_Beginning[
                        Random.Range(
                            0,
                            TimelineManager.Instance.m_TimeLineInfo.m_TL_Beginning.Length)]);

        UIManager.Instance.DeativateTimer();
        UIManager.Instance.SetPlayer_Name();
        StartCoroutine(UIManager.Instance.WaitForTimeLine());

        yield return new WaitUntil(
            () => TimelineManager.Instance.m_IsPlaying == false);

        StartCoroutine(PHASE_Start());


        yield return null;
    }
    IEnumerator PHASE_Start()
    {
        GameManager.Instance.DeactivatePlayers();

        UIManager.Instance.ResetPlayer_Health();

        AudioManager.Instance.LowerMainMusicVolume(0.7f);

        if (GameManager.Instance.m_Init.m_GameMode != EGameModes.TRAINING)
        {
            UIManager.Instance.Setup();
            UIManager.Instance.SetRound(m_currentRound);

            yield return new WaitForSeconds(1.5f);
        }

        StartCoroutine(UIManager.Instance.CountDown(3, "Goo"));

        yield return new WaitUntil(
            () => UIManager.Instance.LOCKED == false);

        AudioManager.Instance.ResetMainMusicVolume();

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
        yield return new WaitUntil(
            () => TimelineManager.Instance.m_IsPlaying == false);

        m_currentRound++;

        UIManager.Instance.SetTimer(0);

        if (m_roundResult == null)
            UIManager.Instance.SetComment_Tie();
        else
            UIManager.Instance.SetComment_PlayerWon((bool)m_roundResult);

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
        yield return new WaitUntil(
            () => TimelineManager.Instance.m_IsPlaying == false);

        UIManager.Instance.SetTimer(0);

        if (m_endResult_Tie)
            UIManager.Instance.SetComment_Tie();
        else
            UIManager.Instance.SetComment_PlayerWon((bool)m_endResult);

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
    void CheckWinner()
    {
        GameManager.Instance.STARTED = false;


        Check_Health(ref m_roundResult);

        Update_RoundsWon();

        Check_RoundsWon(ref m_endResult);


        StopAllCoroutines();
        StartCoroutine(
            m_endResult != null ?
                PHASE_End_Game() :
                PHASE_End_Round());
    }

    void Check_Health(ref bool? _roundResult)
    {
        //Tie
        _roundResult = null;
        //Player_L Won
        if (GameManager.Instance.m_Player_L.Health > GameManager.Instance.m_Player_R.Health)
            _roundResult = true;
        //Player_R Won
        if (GameManager.Instance.m_Player_L.Health < GameManager.Instance.m_Player_R.Health)
            _roundResult = false;
    }

    void Update_RoundsWon()
    {
        if (m_roundResult != null)
            AddRoundWon((bool)m_roundResult);
        else
        {
            AddRoundWon(true);
            AddRoundWon(false);
        }

        UIManager.Instance.Setup();
    }

    void Check_RoundsWon(ref bool? _endResult)
    {
        _endResult = null;

        if (GameManager.Instance.m_Player_L.RoundsWon == GameManager.Instance.m_Init.m_Rounds)
            _endResult = true;
        if (GameManager.Instance.m_Player_R.RoundsWon == GameManager.Instance.m_Init.m_Rounds)
            _endResult = false;

        if (GameManager.Instance.m_Player_L.RoundsWon == GameManager.Instance.m_Init.m_Rounds
           && GameManager.Instance.m_Player_R.RoundsWon == GameManager.Instance.m_Init.m_Rounds)
            m_endResult_Tie = true;
    }

    void ReadDamage()
    {
        if (Check_PlayerDead())
            CheckWinner();
    }
    #endregion

    #region //Helper
    void AddRoundWon(bool _toLeft)
    {
        if (_toLeft)
            ++GameManager.Instance.m_Player_L.RoundsWon;
        else
            ++GameManager.Instance.m_Player_R.RoundsWon;
    }
    bool Check_PlayerDead()
    {
        return GameManager.Instance.m_Player_L.Health <= 0
               || GameManager.Instance.m_Player_R.Health <= 0;
    }
    #endregion
}
