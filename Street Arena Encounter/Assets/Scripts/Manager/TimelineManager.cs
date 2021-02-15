using System.Collections;
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


    #region //Utilites
    /// <summary>
    /// Plays timelineAsset when not null, rebindsTrack Animator references and as long as it plays the timeline the it is active
    /// </summary>
    /// <param name="_tl_asset"></param>
    public void Play(PlayableAsset _tl_asset)
    {
        if (!_tl_asset)
            return;

        StartCoroutine(ActivateTL(_tl_asset));

        RebindAniTracks();

        m_director.Play();

        StartCoroutine(DeactivateTL(m_director.playableAsset.duration));
    }
    /// <summary>
    /// Rebinds the track reference of currently playing timelineAsset. Necessary for player are dynamic
    /// </summary>
    public void RebindAniTracks()
    {
        TimelineAsset timelineAsset = (TimelineAsset)m_director.playableAsset;

        // map the objects appropriately
        TrackAsset track = (TrackAsset)timelineAsset.GetOutputTrack(1);
        TrackAsset track2 = (TrackAsset)timelineAsset.GetOutputTrack(2);
        TrackAsset track3 = (TrackAsset)timelineAsset.GetOutputTrack(3);
        TrackAsset track4 = (TrackAsset)timelineAsset.GetOutputTrack(4);
        m_director.SetGenericBinding(track, GameManager.Instance.m_Player_L.Ani);
        m_director.SetGenericBinding(track2, GameManager.Instance.m_Player_R.Ani);
        m_director.SetGenericBinding(track3, GameManager.Instance.m_PlayerGO_L);
        m_director.SetGenericBinding(track4, GameManager.Instance.m_PlayerGO_R);
    }
    #endregion

    #region //Coroutines
    IEnumerator ActivateTL(PlayableAsset _tl_asset)
    {
        m_director.gameObject.SetActive(true); //activates TimeLine
        GameManager.Instance.m_CMVCamera.gameObject.SetActive(false); //deactivates CinemachineVirtual-Camera

        GameManager.Instance.DeactivatePlayers(); //Deactivates RBs of players
        //remembers the pos of players
        m_tmpStartPos[0] = GameManager.Instance.m_Player_L.Player.gameObject.transform.localPosition;
        m_tmpStartPos[1] = GameManager.Instance.m_Player_R.Player.gameObject.transform.localPosition;

        //set playableAsset from given Parameter
        m_director.playableAsset = _tl_asset;

        //resets timer of Timeline
        m_director.initialTime = 0;

        yield return null;
    }
    IEnumerator DeactivateTL(double _offset)
    {
        float timeStamp = Time.time;
        yield return new WaitUntil(
            () => Time.time - timeStamp > _offset); //waits until timline is finished

        GameManager.Instance.m_CMVCamera.gameObject.SetActive(true); //Activates CinemachineVirtual-Camera again
        m_director.gameObject.SetActive(false); //Deactivates TimeLine again

        GameManager.Instance.ActivatePlayers(); //Activates RBs of players again
        //Sets the pos of players from remembered values
        GameManager.Instance.m_Player_L.Player.gameObject.transform.localPosition = m_tmpStartPos[0];
        GameManager.Instance.m_Player_R.Player.gameObject.transform.localPosition = m_tmpStartPos[1];
    }
    #endregion
}
