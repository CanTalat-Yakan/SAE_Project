using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region //Values
    public static UIManager Instance { get; private set; }

    [HideInInspector] public bool LOCKED;

    [Header("The Canvas")]
    [SerializeField] GameObject m_canvas;

    [Header("Main UI")]
    [SerializeField] TextMeshProUGUI m_timerGUI;
    [SerializeField] TextMeshProUGUI m_commentGUI;

    [Header("Player_L")]
    [SerializeField] TextMeshProUGUI m_name_L;
    [SerializeField] Image m_playerHealthBar_L;
    [SerializeField] Image m_playerHealthBarShadow_L;
    [SerializeField] GameObject m_roundsGO_L;

    [Header("Player_R")]
    [SerializeField] TextMeshProUGUI m_name_R;
    [SerializeField] Image m_playerHealthBar_R;
    [SerializeField] Image m_playerHealthBarShadow_R;
    [SerializeField] GameObject m_roundsGO_R;
    #endregion


    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    #region //Utilities
    public void Setup()
    {
        switch (GameManager.Instance.m_Init.m_GameMode)
        {
            case EGameModes.SOLO:
            case EGameModes.MULTIPLAYER:
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
                break;
            default:
                break;
        }
    }

    public void UpdatePlayer_Health()
    {
        //Player_L
        if (GameManager.Instance.m_Player_L.Health / GameManager.Instance.m_Player_L.GP.Health //the percentage of the players health to his original health amount
            != m_playerHealthBar_L.fillAmount)
        {
            m_playerHealthBar_L.fillAmount = GameManager.Instance.m_Player_L.Health / GameManager.Instance.m_Player_L.GP.Health;
            StartCoroutine(DamageShadowAni(true));
        }
        //Player_R
        if (GameManager.Instance.m_Player_R.Health / GameManager.Instance.m_Player_R.GP.Health //the percentage of the players health to his original health amount
            != m_playerHealthBar_L.fillAmount)
        {
            m_playerHealthBar_R.fillAmount = GameManager.Instance.m_Player_R.Health / GameManager.Instance.m_Player_R.GP.Health;
            StartCoroutine(DamageShadowAni(false));
        }
    }
    public void SetPlayer_Name()
    {
        m_name_L.text = GameManager.Instance.m_Player_L.Name;
        m_name_R.text = GameManager.Instance.m_Player_R.Name;
    }
    public void SetTimer(int time)
    {
        m_timerGUI.gameObject.SetActive(true);
        m_timerGUI.text = time.ToString();
    }
    public void SetRound(int _currentRound)
    {
        m_commentGUI.gameObject.SetActive(true);

        if (_currentRound == GameManager.Instance.m_Init.m_Rounds * 2 - 1)
            if (_currentRound != 1)
            {
                m_commentGUI.text = "Final Round ";

                AudioManager.Instance.Play(
                    AudioManager.Instance.m_AudioInfo.m_FinalRound);

                return;
            }

        m_commentGUI.text = "Round " + _currentRound.ToString();

        if (_currentRound <= 5)
            AudioManager.Instance.Play(
                AudioManager.Instance.m_AudioInfo.m_RoundX[_currentRound]);
        else
            AudioManager.Instance.PlaySequence(
                AudioManager.Instance.m_AudioInfo.m_Round,
                AudioManager.Instance.m_AudioInfo.m_Count[_currentRound]);
    }
    public void SetComment(string _comment)
    {
        m_commentGUI.gameObject.SetActive(true);
        m_commentGUI.text = _comment;
    }
    public void SetComment_Tie()
    {
        m_commentGUI.gameObject.SetActive(true);

        m_commentGUI.text = "Tie";

        AudioManager.Instance.Play(
            AudioManager.Instance.m_AudioInfo.m_Tie);
    }
    public void SetComment_PlayerWon(bool _toLeft)
    {
        m_commentGUI.gameObject.SetActive(true);

        m_commentGUI.text =
            (_toLeft ?
                GameManager.Instance.m_Player_L.Name :
                GameManager.Instance.m_Player_R.Name)
            + "\nWon";

        AudioManager.Instance.PlaySequence(
            AudioManager.Instance.m_AudioInfo.m_Player[_toLeft ? 0 : 1],
            AudioManager.Instance.m_AudioInfo.m_Won);
    }

    public void DeativateTimer()
    {
        m_timerGUI.gameObject.SetActive(false);
    }
    public void DeativateComment()
    {
        m_commentGUI.text = "";
        m_commentGUI.gameObject.SetActive(false);
    }
    public void ResetPlayer_Health()
    {
        m_playerHealthBar_L.fillAmount = GameManager.Instance.m_Player_L.Health / GameManager.Instance.m_Player_L.GP.Health;
        m_playerHealthBarShadow_L.fillAmount = GameManager.Instance.m_Player_L.Health / GameManager.Instance.m_Player_L.GP.Health;

        m_playerHealthBar_R.fillAmount = GameManager.Instance.m_Player_R.Health / GameManager.Instance.m_Player_R.GP.Health;
        m_playerHealthBarShadow_R.fillAmount = GameManager.Instance.m_Player_R.Health / GameManager.Instance.m_Player_R.GP.Health;
    }
    #endregion

    #region //Curoutines
    public IEnumerator WaitForTimeLine()
    {
        m_canvas.SetActive(false);

        yield return new WaitUntil(
            () => TimelineManager.Instance.m_IsPlaying == false);

        m_canvas.SetActive(true);


        yield return null;
    }
    public IEnumerator DamageShadowAni(bool _ofLeftSide, float _duration = 0.2f)
    {
        yield return new WaitForSecondsRealtime(1);

        yield return new WaitUntil(
            () => TimelineManager.Instance.m_IsPlaying == false);


        if (_ofLeftSide)
            DOTween.To(() => m_playerHealthBarShadow_L.fillAmount,
                x => m_playerHealthBarShadow_L.fillAmount = x,
                     m_playerHealthBar_L.fillAmount,
                     _duration);
        else
            DOTween.To(() => m_playerHealthBarShadow_R.fillAmount,
                x => m_playerHealthBarShadow_R.fillAmount = x,
                     m_playerHealthBar_R.fillAmount,
                     _duration);


        yield return null;
    }
    public IEnumerator CountDown(int _countStart, string _startText)
    {
        LOCKED = true;

        for (int i = _countStart; i > 0; i--)
        {
            m_commentGUI.SetText("In " + i);

            AudioManager.Instance.Play(
                AudioManager.Instance.m_AudioInfo.m_Count[i]);
            yield return new WaitForSeconds(1);
        }

        LOCKED = false;

        m_commentGUI.SetText(_startText);

        AudioManager.Instance.Play(
            AudioManager.Instance.m_AudioInfo.m_Begin[
                Random.Range(
                    0,
                    AudioManager.Instance.m_AudioInfo.m_Begin.Length)]);


        yield return new WaitForSeconds(1);
        m_commentGUI.gameObject.SetActive(false);


        yield return null;
    }
    #endregion
}
