﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static void ChangeScene(int _sceneNumber)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(_sceneNumber);
    }

    public static void ChangeSceneByName(string _sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(_sceneName);
    }

    public static void AddSceneByName(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName, LoadSceneMode.Additive);
    }

    public static void UnloadSceneByName(string _sceneName)
    {
        SceneManager.UnloadSceneAsync(_sceneName);

        if (GameManager.Instance)
            if (SceneManager.GetSceneByName("Settings_Overlay").isSubScene)
                GameManager.Instance.LOCKED = false;
    }

    public static void Pause(bool _pause)
    {
        GameManager.Instance.LOCKED = (_pause) ? true : false;
    }

    public static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
