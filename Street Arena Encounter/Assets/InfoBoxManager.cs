using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoBoxManager : MonoBehaviour
{
    [SerializeField] GameObject m_userList;
    [SerializeField] GameObject m_playerInfo_Prefab;
    [SerializeField] GameObject m_pi_controller_Prefab;
    [SerializeField] GameObject m_pi_keyboard_Prefab;
    [SerializeField] Main_Init m_init;

    public void UpdateList()
    {
        {
            switch (m_init.m_GameMode)
            {
                case EGameModes.SOLO:
                    break;
                case EGameModes.MULTIPLAYER:
                    break;
                case EGameModes.LOCAL:
                    {
                        GameObject gobj = Instantiate(m_playerInfo_Prefab, m_userList.transform);
                        gobj.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_init.m_Player_L.Name;

                        if (m_init.m_Player_L.Input.currentControlScheme == "Gamepad")
                            Instantiate(m_pi_controller_Prefab, gobj.transform.GetChild(2).GetChild(1).transform);
                        if (m_init.m_Player_L.Input.currentControlScheme == "Keyboard")
                            Instantiate(m_pi_keyboard_Prefab, gobj.transform.GetChild(2).GetChild(1).transform);

                        GameObject gobj2 = Instantiate(m_playerInfo_Prefab, m_userList.transform);
                        gobj2.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_init.m_Player_R.Name;

                        if (m_init.m_Player_R.Input.currentControlScheme == "Gamepad")
                            Instantiate(m_pi_controller_Prefab, gobj2.transform.GetChild(2).GetChild(1).transform);
                        if (m_init.m_Player_R.Input.currentControlScheme == "Keyboard")
                            Instantiate(m_pi_keyboard_Prefab, gobj2.transform.GetChild(2).GetChild(1).transform);
                    }
                    break;
                case EGameModes.TRAINING:
                    {
                        GameObject gobj = Instantiate(m_playerInfo_Prefab, m_userList.transform);
                        gobj.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_init.m_Player_L.Name;
                    }
                    break;
                default:
                    break;
            }
        }
        {
        }
    }
}
