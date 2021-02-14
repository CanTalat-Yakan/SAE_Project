﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoBoxManager : MonoBehaviour
{
    public static InfoBoxManager Instance { get; private set; }

    [SerializeField] GameObject m_userList;
    [SerializeField] GameObject m_playerInfo_Prefab;
    [SerializeField] Main_Init m_init;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void UpdateList()
    {
        switch (m_init.m_GameMode)
        {
            case EGameModes.SOLO:
                break;
            case EGameModes.MULTIPLAYER:
            case EGameModes.LOCAL:
                {
                    GameObject iconL = Instantiate(m_playerInfo_Prefab, m_userList.transform);
                    InputManager.Instance.m_DestroyGObjCollection.Add(iconL);
                    iconL.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_init.m_Player_L.Name;
                    iconL.transform.GetChild(1).GetComponent<Image>().color = new Color(Random.value, Random.value, Random.value, 1);
                    iconL.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = m_init.m_Player_L.Image;

                    GameObject iconR = Instantiate(m_playerInfo_Prefab, m_userList.transform);
                    InputManager.Instance.m_DestroyGObjCollection.Add(iconR);
                    iconR.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_init.m_Player_R.Name;
                    iconR.transform.GetChild(1).GetComponent<Image>().color = new Color(Random.value, Random.value, Random.value, 1);
                    iconR.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = m_init.m_Player_R.Image;


                    switch (InputManager.Instance.m_PlayerL_Input.currentControlScheme)
                    {
                        case "Keyboard":
                            InputManager.Instance.CreateIcon(EPIIconType.KEYBOARD, iconL.transform.GetChild(2).GetChild(1));
                            break;
                        case "Gamepad":
                            InputManager.Instance.CreateIcon(EPIIconType.GAMEPAD, iconL.transform.GetChild(2).GetChild(1));
                            break;
                        default:
                            break;
                    }
                    switch (InputManager.Instance.m_PlayerR_Input.currentControlScheme)
                    {
                        case "Keyboard":
                            InputManager.Instance.CreateIcon(EPIIconType.KEYBOARD, iconR.transform.GetChild(2).GetChild(1));
                            break;
                        case "Gamepad":
                            InputManager.Instance.CreateIcon(EPIIconType.GAMEPAD, iconR.transform.GetChild(2).GetChild(1));
                            break;
                        default:
                            break;
                    }
                }
                break;
            case EGameModes.TRAINING:
                {
                    GameObject iconL = Instantiate(m_playerInfo_Prefab, m_userList.transform);
                    InputManager.Instance.m_DestroyGObjCollection.Add(iconL);
                    iconL.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_init.m_Player_L.Name;
                    iconL.transform.GetChild(1).GetComponent<Image>().color = new Color(Random.value, Random.value, Random.value, 1);
                }
                break;
            default:
                break;
        }
    }
}
