using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    #region ----- Variables -----
    public Main_Init m_Init;
    public Gamepad_Input m_input;
    [SerializeField] PlayableDirector m_timeline;
    public CharacterController m_playerLEFT;
    public CharacterController m_playerRIGHT;
    public GameObject m_loadingScreen;

    float m_timer = 0;
    float m_timeStamp = 0;
    [SerializeField] float m_loadTimer;

    [HideInInspector] public bool LOCKED;
    [HideInInspector] public bool STARTED;
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

        StartCoroutine(Init());
    }

    void Update()
    {
        MenuOverlay();

        if (LOCKED)
            return;
    }

    #region ----- Utilities -----


    void MenuOverlay()
    {
        if (!STARTED)
            return;

        if (InputSystem.GetDevice<Gamepad>().startButton.wasPressedThisFrame)
        {
            LOCKED = true;
            Pause();
        }
        if (InputSystem.GetDevice<Gamepad>().buttonEast.wasPressedThisFrame)
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
    public void ActivateChars()
    {
        m_playerLEFT.enabled = true;
        m_playerRIGHT.enabled = true;
    }
    public void DeactivateChars()
    {
        m_playerLEFT.enabled = false;
        m_playerRIGHT.enabled = false;
    }
    #endregion ----- Helper -----

    #region ----- Coroutine -----
    IEnumerator Init()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        LOCKED = false;
        STARTED = false;
        m_input = new Gamepad_Input();

        DeactivateChars();

        if (!SceneManager.GetSceneByName(m_Init.m_Level.ToString()).isLoaded)
            SceneManager.LoadScene(m_Init.m_Level.ToString(), LoadSceneMode.Additive);

        m_timeStamp = Time.time;
        StartCoroutine(UnloadScenes());

        yield return null;
    }

    IEnumerator UnloadScenes()
    {
        yield return new WaitForSeconds(1);

        while (Time.time - m_timeStamp < m_loadTimer)
        {
            m_loadingScreen.SetActive(!SceneManager.GetSceneByName("Menu").isLoaded);
            yield return new WaitForSeconds(1);
        }

        if (SceneManager.GetSceneByName("Menu").isLoaded)
            SceneManager.UnloadSceneAsync("Menu");

        if (!SceneManager.GetSceneByName("GUI_Overlay").isLoaded)
            SceneManager.LoadScene("GUI_Overlay", LoadSceneMode.Additive);



        m_timeline.Play();

        m_loadingScreen.SetActive(false);

        yield return null;
    }
    #endregion
}
