using DG.Tweening;
using System.Collections;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    #region //Fields
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


    #region //Utilities
    /// <summary>
    /// Resets the timer, currentRound and any kind of Results
    /// </summary>
    void ResetValues()
    {
        m_timer = GameManager.Instance.m_Init.m_Timer;
        m_currentRound = 1;
        m_roundResult = null;
        m_endResult = null;
        m_endResult_Tie = false;
        m_roundResult = null;
    }
    /// <summary>
    /// Checks if there is a winner in round and sets the result of round or endgame to the local field values
    /// when there is a endgame winner then the endGame Coroutine plays
    /// </summary>
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
    /// <summary>
    /// checks health of players and returns the result of the calculation
    /// </summary>
    /// <param name="_roundResult">the roundResult nullable bool to past the result onto</param>
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

        //Play DeadAni
        if (_roundResult != null && Check_PlayerDead())
            DamageManager.Instance.PlayerIsDead((bool)!_roundResult);
    }
    /// <summary>
    /// Updates the round in ui and local variables
    /// </summary>
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
    /// <summary>
    /// Check if there is a winner of the game that won all rounds
    /// </summary>
    /// <param name="_endResult"></param>
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
    /// <summary>
    /// if player is dead then checkWinner
    /// </summary>
    void ReadDamage()
    {
        if (Check_PlayerDead())
            CheckWinner();
    }
    #endregion

    #region //Coroutine
    IEnumerator PHASE_Awake()
    {
        ResetValues();

        yield return new WaitUntil(
            () => UIManager.Instance != null); //wait for UI

        //Play Beginning TimeLine
        if (!GameManager.Instance.m_Init.m_SkipIntro) //when dont SkipIntrol
            if (GameManager.Instance.m_Init.m_GameMode != EGameModes.TRAINING) //when not training mode
                TimelineManager.Instance.Play(
                    TimelineManager.Instance.m_TimeLineInfo.m_TL_Beginning[
                        Random.Range(
                            0,
                            TimelineManager.Instance.m_TimeLineInfo.m_TL_Beginning.Length)]);

        //Deactivate Timer UI-Elemet
        UIManager.Instance.DeativateTimer();
        //Setup Playername
        UIManager.Instance.SetPlayer_Name();
        //Make UIManager wait for TimeLine Manager
        StartCoroutine(UIManager.Instance.WaitForTimeLine());

        yield return new WaitUntil(
            () => TimelineManager.Instance.m_IsPlaying == false); //wait for TimeLine

        StartCoroutine(PHASE_Start());


        yield return null;
    }
    IEnumerator PHASE_Start()
    {
        GameManager.Instance.DeactivatePlayers(); //Deactivates Players RB
        UIManager.Instance.ResetPlayer_Health(); //Reset Healthbar
        AudioManager.Instance.LowerMainMusicVolume(0.7f); //lower MainMusic Volume

        //Update UI for Round Manager
        if (GameManager.Instance.m_Init.m_GameMode != EGameModes.TRAINING) //when not training
        {
            UIManager.Instance.Setup();
            UIManager.Instance.SetRound(m_currentRound);

            yield return new WaitForSeconds(1.5f);
        }

        //Plays the countDown Corouting from UIManager
        StartCoroutine(UIManager.Instance.CountDown(3, "Goo"));

        yield return new WaitUntil(
            () => UIManager.Instance.LOCKED == false); //Wait for UI

        AudioManager.Instance.ResetMainMusicVolume(); //reset MainMusic Volume

        GameManager.Instance.STARTED = true; //Game Starts
        GameManager.Instance.ActivatePlayers(); //Activates Player back

        yield return new WaitForSeconds(1);

        UIManager.Instance.DeativateComment(); //Dactivate Comment UI-Elemet

        StartCoroutine(PHASE_Loop()); //Start the loop for Rounds


        yield return null;
    }
    IEnumerator PHASE_Loop()
    {
        switch (GameManager.Instance.m_Init.m_GameMode)
        {
            case EGameModes.TRAINING:
                {
                    UIManager.Instance.DeativateTimer(); //training no loop
                    break;
                }
            case EGameModes.MULTIPLAYER:
                break;
            case EGameModes.LOCAL:
                {
                    yield return new WaitForSeconds(1); //wait a second
                    --m_timer; //substract a second from timer

                    UIManager.Instance.SetTimer(m_timer); //set UI-Timer with local Variable timer

                    if (m_timer != 0) //when timer is zero, the round ends
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
            () => TimelineManager.Instance.m_IsPlaying == false); //Wait for TimeLine

        m_currentRound++; //add up currentRound

        UIManager.Instance.SetTimer(0); //SetRound in UI 0 while waiting for new begin of round

        //when roundResult
        //true      => leftPlayer Won or 
        //false     => rightPlayer Won, then
        //null      => Tie
        if (m_roundResult == null)
            UIManager.Instance.SetComment_Tie();
        else
            UIManager.Instance.SetComment_PlayerWon((bool)m_roundResult);


        DOTween.Clear(); //stop any DoTween Animations
        GameManager.Instance.STARTED = false; //Game not started, meaning it is paused
        GameManager.Instance.DeactivatePlayers(); //Deactivate RBs

        yield return new WaitForSeconds(3);

        GameManager.Instance.ResetPlayers();

        UIManager.Instance.SetTimer(
            m_timer = GameManager.Instance.m_Init.m_Timer); //Set timer to the given Value in Main_Init

        StartCoroutine(PHASE_Start()); //Start a new Round


        yield return null;
    }
    IEnumerator PHASE_End_Game()
    {
        yield return new WaitUntil(
            () => TimelineManager.Instance.m_IsPlaying == false); //Wait for TimeLine

        UIManager.Instance.SetTimer(0); //SetRound in UI 0 while waiting for new begin of round

        //endResult_TIE => true then both won gamea
        if (m_endResult_Tie)
            UIManager.Instance.SetComment_Tie();
        else
            //when endResult
            //true      => leftPlayer Won or 
            //false     => rightPlayer Won, then
            //null      => noone Won
            UIManager.Instance.SetComment_PlayerWon((bool)m_endResult);

        DOTween.Clear(); //stop any DoTween Animations
        GameManager.Instance.STARTED = false; //Game not started, meaning it is paused
        GameManager.Instance.DeactivatePlayers(); //Deactivate RBs
        //GameManager.Instance.ResetPlayers(); //Reset Playerpositions

        yield return new WaitForSeconds(6);

        SceneHandler.ChangeSceneByName("EndScreen_Overlay");//Starts EndScreen_Overlay for revange or backToMenu


        yield return null;
    }
    #endregion

    #region //Helper
    /// <summary>
    /// Adds a roundWon to the playerInformation
    /// </summary>
    /// <param name="_toLeft"></param>
    void AddRoundWon(bool _toLeft)
    {
        if (_toLeft)
            ++GameManager.Instance.m_Player_L.RoundsWon;
        else
            ++GameManager.Instance.m_Player_R.RoundsWon;
    }
    /// <summary>
    /// Reads the damage of the two players and if one is dead returns the dead player
    /// </summary>
    /// <returns></returns>
    bool Check_PlayerDead()
    {
        return GameManager.Instance.m_Player_L.Health <= 0
               || GameManager.Instance.m_Player_R.Health <= 0;
    }
    #endregion
}
