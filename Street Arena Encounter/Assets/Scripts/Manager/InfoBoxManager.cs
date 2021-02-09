using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoBoxManager : MonoBehaviour
{
    [SerializeField] GameObject m_userList;
    [SerializeField] GameObject m_playerInfo_Prefab;
    [SerializeField] Main_Init m_init;

    public void UpdateList()
    {
        switch (m_init.m_GameMode)
        {
            case EGameModes.SOLO:
                break;
            case EGameModes.MULTIPLAYER:
            case EGameModes.LOCAL:
                {
                    GameObject gobj = Instantiate(m_playerInfo_Prefab, m_userList.transform);
                    gobj.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_init.m_Player_L.Name;

                    GameObject gobj2 = Instantiate(m_playerInfo_Prefab, m_userList.transform);
                    gobj2.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_init.m_Player_R.Name;

                    switch (InputManager.Instance.m_Player_L_Input.currentControlScheme)
                    {
                        case "Keyboard":
                            InputManager.Instance.CreateIcon(EPIIconType.KEYBOARD, gobj.transform.GetChild(2).GetChild(1));
                            break;
                        case "Gamepad":
                            InputManager.Instance.CreateIcon(EPIIconType.GAMEPAD, gobj.transform.GetChild(2).GetChild(1));
                            break;
                        default:
                            break;
                    }
                    switch (InputManager.Instance.m_Player_R_Input.currentControlScheme)
                    {
                        case "Keyboard":
                            InputManager.Instance.CreateIcon(EPIIconType.KEYBOARD, gobj2.transform.GetChild(2).GetChild(1));
                            break;
                        case "Gamepad":
                            InputManager.Instance.CreateIcon(EPIIconType.GAMEPAD, gobj2.transform.GetChild(2).GetChild(1));
                            break;
                        default:
                            break;
                    }
                }
                break;
            case EGameModes.TRAINING:
                {
                    Instantiate(m_playerInfo_Prefab, m_userList.transform);
                }
                break;
            default:
                break;
        }
    }
}
