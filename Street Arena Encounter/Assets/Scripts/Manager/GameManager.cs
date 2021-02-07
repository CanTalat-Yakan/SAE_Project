using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    #region -Values
    [Header("Level Attributes")]
    public Main_Init m_Init;
    public GameObject m_Default_Input;
    public CinemachineVirtualCamera m_CMVCamera;
    public Camera m_MainCamera;
    public GP_Settings m_GP;
    public Attack_Settings m_ATK;

    [Header("Player Attributes")]
    [Space(15)]
    [SerializeField] GameObject m_PlayerGO_L;
    [HideInInspector] public PlayerInformation m_Player_L;
    [Space(15)]
    [SerializeField] GameObject m_PlayerGO_R;
    [HideInInspector] public PlayerInformation m_Player_R;

    [Header("Start Attributes")]
    [SerializeField] float m_loadingscreenTimer;
    public bool m_SkipIntro;

    [HideInInspector] public bool LOCKED;
    [HideInInspector] public bool STARTED;
    #endregion

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

#if !UNITY_EDITOR
        m_SkipIntro = false;
#endif

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        StartCoroutine(Init());
    }

    void Update()
    {
        MenuOverlay();

        if (LOCKED)
            return;
    }

    void LateUpdate()
    {
        //Lock Target in Movable Space
        m_Player_L.Constraint();
        m_Player_R.Constraint();
    }

    #region -Utilities
    void MenuOverlay()
    {
        if (!STARTED)
            return;

        if (InputSystem.GetDevice<Gamepad>() != null)
        {
            if (InputSystem.GetDevice<Gamepad>().startButton.wasPressedThisFrame)
            {
                LOCKED = true;
                Pause();
            }
            if (InputSystem.GetDevice<Gamepad>().buttonEast.wasPressedThisFrame)
            {
                LOCKED = false;
                Continue();
            }
        }
        if (InputSystem.GetDevice<Keyboard>() != null)
        {
            if (InputSystem.GetDevice<Keyboard>().escapeKey.wasPressedThisFrame)
            {
                LOCKED = true;
                Pause();
            }
            if (InputSystem.GetDevice<Keyboard>().enterKey.wasPressedThisFrame)
            {
                LOCKED = false;
                Continue();
            }
        }
    }
    public void Pause()
    {
        if (!SceneManager.GetSceneByName("Settings_Overlay").isLoaded)
        {
            if (!SceneManager.GetSceneByName("Menu_Overlay").isLoaded)
                SceneManager.LoadScene("Menu_Overlay", LoadSceneMode.Additive);
        }
        else if (SceneManager.GetSceneByName("Menu_Overlay").isLoaded)
            SceneManager.UnloadSceneAsync("Menu_Overlay");

        Time.timeScale = 0;
    }
    public void Continue()
    {
        if (SceneManager.GetSceneByName("Settings_Overlay").isLoaded)
        {
            SceneManager.UnloadSceneAsync("Settings_Overlay");
            LOCKED = true;
            return;
        }

        if (SceneManager.GetSceneByName("Menu_Overlay").isLoaded)
            SceneManager.UnloadSceneAsync("Menu_Overlay");

        Time.timeScale = 1;
        LOCKED = false;
    }
    public void ResetPlayers()
    {
        m_Player_L.Player.ResetValues();
        if (m_Player_R.Player.isActiveAndEnabled)
            m_Player_R.Player.ResetValues();
    }
    #endregion

    #region -Helper
    public RaycastHit HitRayCast(Vector3 _origin, Vector3 _direction, float _maxDistance)
    {
        RaycastHit hit;

        Ray ray = new Ray(
            _origin,
            _direction);

        Physics.Raycast(
            ray,
            out hit,
            _maxDistance);

        return hit;
    }
    public bool BoolRayCast(Vector3 _origin, Vector3 _direction, float _maxDistance)
    {
        Ray ray = new Ray(
            _origin,
            _direction);

        return Physics.Raycast(
            ray,
            _maxDistance);
    }
    public float Map(float _oldValue, float _oldMin, float _oldMax, float _newMin, float _newMax)
    {
        float oldRange = _oldMax - _oldMin;
        float newRange = _newMax - _newMin;
        float newValue = ((_oldValue - _oldMin) * newRange / oldRange) + _newMin;

        return Mathf.Clamp(newValue, _newMin, _newMax);
    }
    public void ActivatePlayers()
    {
        if (m_Player_L.Player.m_MovementController)
            m_Player_L.Player.m_MovementController.enabled = true;
        if (m_Player_R.Player.m_MovementController)
            m_Player_R.Player.m_MovementController.enabled = true;
    }
    public void DeactivatePlayers()
    {
        if (m_Player_L.Player.m_MovementController)
            m_Player_L.Player.m_MovementController.enabled = false;
        if (m_Player_R.Player.m_MovementController)
            m_Player_R.Player.m_MovementController.enabled = false;
    }
    public bool BoolDistance(float _threshold)
    {
        float length = Vector3.Distance(
            m_Player_L.Player.transform.localPosition,
            m_Player_R.Player.transform.localPosition);

        return length > _threshold;
    }
    public float GetDistance()
    {
        return Vector3.Distance(
            m_Player_L.Player.transform.localPosition,
            m_Player_R.Player.transform.localPosition);
    }
    public float MapDistance(float _threshold)
    {
        return Map(Mathf.Abs(GetDistance()),
            _threshold, 2.3f,
            0, 1);
    }
    #endregion

    #region -Coroutine
    IEnumerator Init()
    {
        StartCoroutine(Setup_Playerinformation());

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        LOCKED = false;
        STARTED = false;

        DeactivatePlayers();

        if (!SceneManager.GetSceneByName(m_Init.m_Level.ToString()).isLoaded)
            SceneManager.LoadScene(m_Init.m_Level.ToString(), LoadSceneMode.Additive);

        StartCoroutine(UnloadScenes());

        yield return null;
    }
    IEnumerator Setup_Playerinformation()
    {
        m_Player_L.ResetValues();
        m_Player_R.ResetValues();
        m_Player_L.Name = m_Init.m_Player_L.Name;
        m_Player_R.Name = m_Init.m_Player_R.Name;
        m_Player_L.GatherComponents(m_PlayerGO_L);
        m_Player_R.GatherComponents(m_PlayerGO_R);

        if (m_Init.m_GameMode == EGameModes.LOCAL)
        {
            m_Player_L.Input = GameObject.Find("P_Input(Clone)0").GetComponent<InputMaster>();
            m_Player_R.Input = GameObject.Find("P_Input(Clone)1").GetComponent<InputMaster>();
        }
        if (m_Init.m_GameMode == EGameModes.TRAINING)
        {
            m_Default_Input.SetActive(true);
            m_Player_L.Input = m_Default_Input.GetComponent<InputMaster>();
        }

        yield return null;
    }
    IEnumerator UnloadScenes()
    {
        float timeStamp = Time.time;

        yield return new WaitForSeconds(1);

        if (!m_SkipIntro)
            while (Time.time - timeStamp < m_loadingscreenTimer)
            {
                yield return new WaitForSeconds(1);
            }

        if (SceneManager.GetSceneByName("Menu").isLoaded)
            SceneManager.UnloadSceneAsync("Menu");

        if (SceneManager.GetSceneByName("EndScreen_Overlay").isLoaded)
            SceneManager.UnloadSceneAsync("EndScreen_Overlay");

        if (!SceneManager.GetSceneByName("GUI_Overlay").isLoaded)
            SceneManager.LoadScene("GUI_Overlay", LoadSceneMode.Additive);


        if (!m_SkipIntro)
            TimelineManager.Instance.Play(TimelineManager.Instance.m_TL_Beginning[Random.Range(0, TimelineManager.Instance.m_TL_Beginning.Length)]);
        else
            m_MainCamera.gameObject.SetActive(true);

        yield return null;
    }
    #endregion
}
