using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    #region //Values
    [Header("Level Attributes")]
    public Main_Init m_Init;
    public GameObject m_Default_Input;
    public CinemachineVirtualCamera m_CMVCamera;
    public GP_Settings m_GP;
    public Attack_Settings m_ATK;

    [Header("Player Attributes")]
    [Space(15)]
    public GameObject m_PlayerGO_L;
    public GameObject m_PlayerModel_L;
    public PlayerInformation m_Player_L;
    [Space(15)]
    public GameObject m_PlayerGO_R;
    public GameObject m_PlayerModel_R;
    public PlayerInformation m_Player_R;

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
        m_Init.m_SkipIntro = false;
#endif

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        StartCoroutine(Init());
    }

    void Update()
    {
        MenuOverlay();
    }

    #region //Utilities
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
        m_Player_L.ResetValues();
        m_Player_R.ResetValues();
    }
    #endregion

    #region //Helper
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
        m_Player_L.RB.WakeUp();
        m_Player_R.RB.WakeUp();
    }
    public void DeactivatePlayers()
    {
        m_Player_L.RB.Sleep();
        m_Player_R.RB.Sleep();
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
        return Map(
            Mathf.Abs(GetDistance()),
            _threshold, 2.3f,
            0, 1);
    }
    #endregion

    #region //Coroutine
    IEnumerator Init()
    {
        StartCoroutine(SetupPlayer());

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
    IEnumerator SetupPlayer()
    {
        //Set Model
        m_PlayerModel_L = Instantiate(m_Init.m_Player_L.Model, m_PlayerGO_L.transform);
        m_PlayerModel_L.transform.localPosition = new Vector3(0, m_Init.m_Player_L.GroundOffset, 0);
        m_PlayerModel_L.transform.rotation = Quaternion.Euler(0, 90, 0);
        m_PlayerModel_L.transform.localScale = new Vector3(1, 1, 1);

        m_PlayerModel_R = Instantiate(m_Init.m_Player_R.Model, m_PlayerGO_R.transform);
        m_PlayerModel_R.transform.localPosition = new Vector3(0, m_Init.m_Player_R.GroundOffset, 0);
        m_PlayerModel_R.transform.rotation = Quaternion.Euler(0, -90, 0);
        m_PlayerModel_R.transform.localScale = new Vector3(-1, 1, 1);


        //Reset Values; Health, Roundswon, Special
        m_Player_L.ResetPlayerInformation();
        m_Player_R.ResetPlayerInformation();

        //Get Input
        if (m_Init.m_GameMode == EGameModes.LOCAL)
        {
            if (!InputManager.Instance.m_PlayerL_Input)
                SceneManager.LoadScene("Menu");

            m_Player_L.Input = InputManager.Instance.m_PlayerL_Input.GetComponent<InputMaster>();
            m_Player_R.Input = InputManager.Instance.m_PlayerR_Input.GetComponent<InputMaster>();
        }
        if (m_Init.m_GameMode == EGameModes.TRAINING)
        {
            m_Default_Input.SetActive(true);
            m_Player_L.Input = m_Default_Input.GetComponent<InputMaster>();
            m_Player_R.Input = null;
        }

        //Get Components
        m_Player_L.GatherComponents(m_PlayerGO_L);
        m_Player_R.GatherComponents(m_PlayerGO_R);


        yield return null;
    }
    IEnumerator UnloadScenes()
    {
        if (SceneManager.GetSceneByName("Menu").isLoaded)
        {
            float timeStamp = Time.time;
            yield return new WaitUntil(
                () => Time.time - timeStamp > m_Init.m_LoadingScreenTime);

            SceneManager.UnloadSceneAsync("Menu");
        }

        if (SceneManager.GetSceneByName("EndScreen_Overlay").isLoaded)
            SceneManager.UnloadSceneAsync("EndScreen_Overlay");

        if (!SceneManager.GetSceneByName("GUI_Overlay").isLoaded)
            SceneManager.LoadScene("GUI_Overlay", LoadSceneMode.Additive);


        yield return null;
    }
    #endregion
}
