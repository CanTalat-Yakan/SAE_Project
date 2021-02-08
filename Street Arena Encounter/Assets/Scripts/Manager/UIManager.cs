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

    public void SetPlayer_Name(bool _isLeft, string _name)
    {
        if (_isLeft)
            m_name_L.text = _name;
        else
            m_name_R.text = _name;
    }
    public void SetPlayer_Health()
    {
        //Player_L
        if (m_playerHealthBar_L.fillAmount != GameManager.Instance.m_Player_L.Health / GameManager.Instance.m_Player_L.GP.Health)
        {
            m_playerHealthBar_L.fillAmount = GameManager.Instance.m_Player_L.Health / GameManager.Instance.m_Player_L.GP.Health;
            StartCoroutine(DamageShadowAni());
        }

        //Player_R
        if (m_playerHealthBar_R.fillAmount != GameManager.Instance.m_Player_R.Health / GameManager.Instance.m_Player_R.GP.Health)
        {
            m_playerHealthBar_R.fillAmount = GameManager.Instance.m_Player_R.Health / GameManager.Instance.m_Player_R.GP.Health;
            StartCoroutine(DamageShadowAni());
        }
    }
    public void SetTimer(int time)
    {
        m_timerGUI.gameObject.SetActive(true);
        m_timerGUI.text = time.ToString();
    }
    public void SetComment(string _comment)
    {
        m_commentGUI.gameObject.SetActive(true);
        m_commentGUI.text = _comment;
    }
    public void SetComment_PlayerWon(bool? _toLeft)
    {
        m_commentGUI.gameObject.SetActive(true);

        m_commentGUI.text =
            (bool)_toLeft ?
                GameManager.Instance.m_Player_L.Name :
                GameManager.Instance.m_Player_R.Name
            + " Won";

        if (_toLeft == null)
            m_commentGUI.text = "Tie";
    }

    public void DeativateTimer()
    {
        m_timerGUI.gameObject.SetActive(false);
    }
    public void DeativateComment()
    {
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
        if (GameManager.Instance.m_SkipIntro)
            yield return null;

        m_canvas.SetActive(false);

        yield return new WaitUntil(
            () => TimelineManager.Instance.m_IsPlaying == false);

        m_canvas.SetActive(true);


        yield return null;
    }
    public IEnumerator DamageShadowAni()
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

        m_commentGUI.SetText(_startText);

        yield return new WaitForSeconds(1);
        m_commentGUI.gameObject.SetActive(false);

        LOCKED = false;


        yield return null;
    }
    #endregion
}
