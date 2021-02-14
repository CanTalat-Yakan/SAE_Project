using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance { get; private set; }
    #region //Fields
    [SerializeField] PlayableDirector m_director;
    public Timeline_Info m_TimeLineInfo;

    Vector3[] m_tmpStartPos = new Vector3[2];
    #endregion

    #region //Properties
    public bool m_IsPlaying { get => m_director.state == PlayState.Playing; }
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

        RebindAniTracks();

        m_director.Play();

        StartCoroutine(DeactivateTL(m_director.playableAsset.duration));
    }

    public void RebindAniTracks()
    {
        TimelineAsset timelineAsset = (TimelineAsset)m_director.playableAsset;

        // map the objects appropriately
        TrackAsset track = (TrackAsset)timelineAsset.GetOutputTrack(1);
        TrackAsset track2 = (TrackAsset)timelineAsset.GetOutputTrack(2);
        m_director.SetGenericBinding(track, GameManager.Instance.m_Player_L.Ani);
        m_director.SetGenericBinding(track2, GameManager.Instance.m_Player_R.Ani);
    }


    IEnumerator ActivateTL(PlayableAsset _tl_asset)
    {
        m_director.gameObject.SetActive(true);
        GameManager.Instance.m_CMVCamera.gameObject.SetActive(false);

        GameManager.Instance.DeactivatePlayers();
        m_tmpStartPos[0] = GameManager.Instance.m_Player_L.Player.gameObject.transform.localPosition;
        m_tmpStartPos[1] = GameManager.Instance.m_Player_R.Player.gameObject.transform.localPosition;

        m_director.playableAsset = _tl_asset;

        m_director.initialTime = 0;

        yield return null;
    }

    IEnumerator DeactivateTL(double _offset)
    {
        float timeStamp = Time.time;
        yield return new WaitUntil(
            () => Time.time - timeStamp > _offset);

        GameManager.Instance.m_CMVCamera.gameObject.SetActive(true);
        m_director.gameObject.SetActive(false);

        GameManager.Instance.ActivatePlayers();
        GameManager.Instance.m_Player_L.Player.gameObject.transform.localPosition = m_tmpStartPos[0];
        GameManager.Instance.m_Player_R.Player.gameObject.transform.localPosition = m_tmpStartPos[1];
    }
}
