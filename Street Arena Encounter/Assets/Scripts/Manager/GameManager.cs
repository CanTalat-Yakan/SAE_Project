using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    #region //Fields
    [Header("Level Attributes")]
    public Main_Init m_Init;
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
    /// <summary>
    /// Checks Input and Opens MenuOverlay accordingly
    /// </summary>
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
    /// <summary>
    /// Pauses the game and opens menuOverlay or settingsOverlay
    /// </summary>
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
    /// <summary>
    /// Continues, removes overlay and unlockes game
    /// </summary>
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
    /// <summary>
    /// Resets the players information, round based
    /// </summary>
    public void ResetPlayers()
    {
        m_Player_L.ResetValues();
        m_Player_R.ResetValues();
    }
    #endregion

    #region //Coroutine
    /// <summary>
    /// Initializes the Game with setup of mouse, player and scenes
    /// </summary>
    /// <returns></returns>
    IEnumerator Init()
    {
        StartCoroutine(SetupPlayer()); //Setup PlayerInformation

        Cursor.lockState = CursorLockMode.Locked; //Lockes the Cursor and makes it Invisble
        Cursor.visible = false;
        //Locked and Started reseted
        LOCKED = false;
        STARTED = false;

        DeactivatePlayers(); //Deactivated Players RBs

        if (!SceneManager.GetSceneByName(m_Init.m_Level.ToString()).isLoaded)
            SceneManager.LoadScene(m_Init.m_Level.ToString(), LoadSceneMode.Additive);//Loads Level Additive

        StartCoroutine(UnloadScenes()); //Unloads the obsolete Scenes


        yield return null;
    }
    /// <summary>
    /// Sets up player according to the fighter Information and updates the playerInformation of Left and right
    /// </summary>
    /// <returns></returns>
    IEnumerator SetupPlayer()
    {
        //Set Model
        m_PlayerModel_L = Instantiate(m_Init.m_Player_L.Model, m_PlayerGO_L.transform);
        m_PlayerModel_L.transform.localPosition = new Vector3(0, m_Player_L.GroundOffset = m_Init.m_Player_L.GroundOffset, 0);
        m_PlayerModel_L.transform.rotation = Quaternion.Euler(0, 90, 0);
        m_PlayerModel_L.transform.localScale = new Vector3(1, 1, 1);

        m_PlayerModel_R = Instantiate(m_Init.m_Player_R.Model, m_PlayerGO_R.transform);
        m_PlayerModel_R.transform.localPosition = new Vector3(0, m_Player_R.GroundOffset = m_Init.m_Player_R.GroundOffset, 0);
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
            m_Player_L.Input = InputManager.Instance.GetDefaultInput();
            m_Player_R.Input = null;
        }

        //Get Components
        m_Player_L.GatherComponents(m_PlayerGO_L);
        m_Player_R.GatherComponents(m_PlayerGO_R);


        yield return null;
    }
    /// <summary>
    /// Unloads the scene menu, endscreen or overlay
    /// </summary>
    /// <returns></returns>
    IEnumerator UnloadScenes()
    {
        if (SceneManager.GetSceneByName("EndScreen_Overlay").isLoaded)
            SceneManager.UnloadSceneAsync("EndScreen_Overlay");

        if (SceneManager.GetSceneByName("Menu").isLoaded)
        {
            float timeStamp = Time.time;
            yield return new WaitUntil(
                () => Time.time - timeStamp > m_Init.m_LoadingScreenTime);

            SceneManager.UnloadSceneAsync("Menu");
        }

        if (!SceneManager.GetSceneByName("GUI_Overlay").isLoaded)
            SceneManager.LoadScene("GUI_Overlay", LoadSceneMode.Additive);


        yield return null;
    }
    #endregion

    #region //Helper
    /// <summary>
    /// Casts a raycast and returns the hit
    /// </summary>
    /// <param name="_origin"></param>
    /// <param name="_direction"></param>
    /// <param name="_maxDistance"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Casts a rayscast and returns the result if hit
    /// </summary>
    /// <param name="_origin"></param>
    /// <param name="_direction"></param>
    /// <param name="_maxDistance"></param>
    /// <returns></returns>
    public bool BoolRayCast(Vector3 _origin, Vector3 _direction, float _maxDistance)
    {
        Ray ray = new Ray(
            _origin,
            _direction);

        return Physics.Raycast(
            ray,
            _maxDistance);
    }
    /// <summary>
    /// Activates the players rigidbody
    /// </summary>
    public void ActivatePlayers()
    {
        m_Player_L.RB.WakeUp();
        m_Player_R.RB.WakeUp();
    }
    /// <summary>
    /// Deactivates the playsers rigidbody
    /// </summary>
    public void DeactivatePlayers()
    {
        m_Player_L.RB.Sleep();
        m_Player_R.RB.Sleep();
    }
    /// <summary>
    /// returns the distance of both players with true if the length is greater than the treshold
    /// </summary>
    /// <param name="_threshold">the length of the distance between players</param>
    /// <returns></returns>
    public bool BoolDistance(float _threshold)
    {
        float length = Vector3.Distance(
            m_Player_L.Player.transform.localPosition,
            m_Player_R.Player.transform.localPosition);

        return length > _threshold;
    }
    /// <summary>
    /// returns the distance of the two players
    /// </summary>
    /// <returns></returns>
    public float GetDistance()
    {
        return Vector3.Distance(
            m_Player_L.Player.transform.localPosition,
            m_Player_R.Player.transform.localPosition);
    }
    #endregion
}
