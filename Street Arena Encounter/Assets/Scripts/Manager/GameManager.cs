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
    public Gamepad_Input m_Input;
    [SerializeField] PlayableDirector m_timeline;
    public CharacterController m_PlayerLEFT;
    public CharacterController m_PlayerRIGHT;

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

    public void ResetPlayer()
    {
        m_PlayerLEFT.gameObject.transform.position = new Vector3(-4, 0, 0);
        m_PlayerRIGHT.gameObject.transform.position = new Vector3(4, 0, 0);
        m_PlayerLEFT.height = 3.1f;
        m_PlayerRIGHT.height = 3.1f;
        m_PlayerRIGHT.GetComponent<CharController>().m_Ani.SetBool("Jump", false);
        m_PlayerRIGHT.GetComponent<CharController>().m_Ani.SetBool("Crouch", false);
        m_PlayerRIGHT.GetComponent<CharController>().m_Ani.SetFloat("Move", 0);
        m_PlayerLEFT.GetComponent<CharController>().m_Ani.SetBool("Jump", false);
        m_PlayerLEFT.GetComponent<CharController>().m_Ani.SetBool("Crouch", false);
        m_PlayerLEFT.GetComponent<CharController>().m_Ani.SetFloat("Move", 0);
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
        m_PlayerLEFT.enabled = true;
        m_PlayerRIGHT.enabled = true;
    }
    public void DeactivateChars()
    {
        m_PlayerLEFT.enabled = false;
        m_PlayerRIGHT.enabled = false;
    }
    #endregion ----- Helper -----

    #region ----- Coroutine -----
    IEnumerator Init()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        LOCKED = false;
        STARTED = false;
        m_Input = new Gamepad_Input();

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
            yield return new WaitForSeconds(1);
        }

        if (SceneManager.GetSceneByName("Menu").isLoaded)
            SceneManager.UnloadSceneAsync("Menu");

        if (SceneManager.GetSceneByName("EndScreen_Overlay").isLoaded)
            SceneManager.UnloadSceneAsync("EndScreen_Overlay");

        if (!SceneManager.GetSceneByName("GUI_Overlay").isLoaded)
            SceneManager.LoadScene("GUI_Overlay", LoadSceneMode.Additive);



        m_timeline.Play();

        yield return null;
    }
    #endregion
}
