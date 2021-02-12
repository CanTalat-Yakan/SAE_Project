using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance { get; private set; }
    #region //Fields
    [SerializeField] PlayableDirector m_TL_Directory;
    public Timeline_Info m_TimeLineInfo;

    [HideInInspector] public PlayableAsset m_TL_CurrentAsset;
    Vector3[] m_tmpStartPos = new Vector3[2];
    #endregion
    #region //Properties
    public bool m_IsPlaying { get => m_TL_Directory.state == PlayState.Playing; }
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

    public void Play(PlayableAsset _tl_asset)
    {
        if (!_tl_asset)
            return;

        StartCoroutine(ActivateTL(_tl_asset));

        m_TL_Directory.Play();

        StartCoroutine(DeactivateTL(Time.time));
    }

    IEnumerator ActivateTL(PlayableAsset _tl_asset)
    {
        m_TL_Directory.gameObject.SetActive(true);
        GameManager.Instance.m_CMVCamera.gameObject.SetActive(false);
        GameManager.Instance.m_MainCamera.gameObject.SetActive(true);

        GameManager.Instance.DeactivatePlayers();
        m_tmpStartPos[0] = GameManager.Instance.m_Player_L.Player.gameObject.transform.localPosition;
        m_tmpStartPos[1] = GameManager.Instance.m_Player_R.Player.gameObject.transform.localPosition;

        m_TL_CurrentAsset = _tl_asset;
        m_TL_Directory.playableAsset = _tl_asset;

        m_TL_Directory.initialTime = 0;

        yield return null;
    }

    IEnumerator DeactivateTL(float _timeStamp)
    {
        yield return new WaitUntil(() => Time.time - _timeStamp >= m_TL_Directory.playableAsset.duration);

        GameManager.Instance.m_CMVCamera.gameObject.SetActive(true);
        m_TL_Directory.gameObject.SetActive(false);

        GameManager.Instance.ActivatePlayers();
        GameManager.Instance.m_Player_L.Player.gameObject.transform.localPosition = m_tmpStartPos[0];
        GameManager.Instance.m_Player_R.Player.gameObject.transform.localPosition = m_tmpStartPos[1];
    }
}
