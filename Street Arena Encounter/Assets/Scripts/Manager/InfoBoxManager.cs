using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoBoxManager : MonoBehaviour
{
    public static InfoBoxManager Instance { get; private set; }

    #region //Fields
    [SerializeField] GameObject m_userList;
    [SerializeField] GameObject m_playerInfo_Prefab;
    [SerializeField] Main_Init m_init;
    #endregion


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
                    //Set up UserInfo of LeftPlayer
                    GameObject iconL = Instantiate(m_playerInfo_Prefab, m_userList.transform);
                    InputManager.Instance.m_DestroyGObjCollection.Add(iconL);
                    iconL.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_init.m_Player_L.Name; //name
                    iconL.transform.GetChild(1).GetComponent<Image>().color = new Color(Random.value, Random.value, Random.value, 1); //color
                    iconL.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = m_init.m_Player_L.Image; //pb

                    //Set up UserInfo of RightPlayer
                    GameObject iconR = Instantiate(m_playerInfo_Prefab, m_userList.transform);
                    InputManager.Instance.m_DestroyGObjCollection.Add(iconR);
                    iconR.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_init.m_Player_R.Name; //name
                    iconR.transform.GetChild(1).GetComponent<Image>().color = new Color(Random.value, Random.value, Random.value, 1); //color
                    iconR.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = m_init.m_Player_R.Image; //pb


                    //Adds the ControlScheme Icons
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
                    //Set up UserInfo of LeftPlayer
                    GameObject iconL = Instantiate(m_playerInfo_Prefab, m_userList.transform);
                    InputManager.Instance.m_DestroyGObjCollection.Add(iconL);
                    iconL.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_init.m_Player_L.Name;
                    iconL.transform.GetChild(1).GetComponent<Image>().color = new Color(Random.value, Random.value, Random.value, 1);
                    iconL.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = m_init.m_Player_L.Image;
                }
                break;
            default:
                break;
        }
    }
}
