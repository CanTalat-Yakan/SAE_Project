using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Main_Init m_Init;
    public Gamepad_Input m_input;
    public InputSystemUIInputModule m_uiInput;
    #region ----- Variables -----

    //[Header("Player 1")]
    //[HideInInspector] public PlayerInfo m_p1;
    //[SerializeField] public GameObject m_p1_GO;
    //[SerializeField] public CharacterController m_p1_Ctrlr;
    //[SerializeField] public EStates m_p1_State;

    //[Header("Player 2")]
    //[HideInInspector] public PlayerInfo m_p2;
    //[SerializeField] public GameObject m_p2_GO;
    //[SerializeField] public CharacterController m_p2_Ctrlr;
    //[SerializeField] public GameplaySettings GameplaySettings;
    //[SerializeField] public EStates m_p2_State;

    float m_timer;
    [SerializeField] float m_loadTimer;

    [HideInInspector] public bool LOCKED;
    [HideInInspector] public Camera MainCamera;
    #endregion ----- Variables -----

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        LOCKED = false;
        m_input = new Gamepad_Input();
        //m_p1 = m_p1_GO.AddComponent<PlayerInfo>().Init(100);
        //m_p2 = m_p2_GO.AddComponent<PlayerInfo>().Init(100);
    }

    void Update()
    {
        MenuOverlay();
        LoadingOverlay();

        if (LOCKED)
            return;
    }

    #region ----- Utilities -----
    void LoadingOverlay()
    {
        if (!SceneManager.GetSceneByName(m_Init.m_Level.ToString()).isLoaded)
        {
            SceneManager.LoadScene(m_Init.m_Level.ToString(), LoadSceneMode.Additive);
            return;
        }

        m_timer += Time.deltaTime;
        if (m_timer >= m_loadTimer)
        {
            if (SceneManager.GetSceneByName("Menu").isLoaded)
                SceneManager.UnloadSceneAsync("Menu");

            if (!SceneManager.GetSceneByName("GUI_Overlay").isLoaded)
                SceneManager.LoadScene("GUI_Overlay", LoadSceneMode.Additive);
        }
    }

    void MenuOverlay()
    {
        if (m_timer < m_loadTimer)
            return;

        if (InputSystem.GetDevice<Gamepad>().startButton.wasPressedThisFrame)
        {
            LOCKED = true;
            Pause();
        }
        if(InputSystem.GetDevice<Gamepad>().buttonEast.wasPressedThisFrame)
            LOCKED = false;

        if (!LOCKED)
            Continue();
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
            //SceneManager.LoadScene("Menu_Overlay", LoadSceneMode.Additive);
            LOCKED = true;
            return;
        }

        if (SceneManager.GetSceneByName("Menu_Overlay").isLoaded)
            SceneManager.UnloadSceneAsync("Menu_Overlay");

        Time.timeScale = 1;
        LOCKED = false;
    }
    #endregion ----- Utilities -----

    #region ----- Helper -----
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
    #endregion ----- Helper -----
}
