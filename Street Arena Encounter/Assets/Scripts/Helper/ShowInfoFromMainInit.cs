using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

enum EMainInitInfoType
{
    GameMode,
    Map,
    Rounds,
    Timer,
}

public class ShowInfoFromMainInit : MonoBehaviour
{
    [SerializeField] Main_Init m_init;
    [SerializeField] EMainInitInfoType m_type;
    [SerializeField] TextMeshProUGUI m_text;

    void Start()
    {
        m_text.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        switch (m_type)
        {
            case EMainInitInfoType.GameMode:
                m_text.text = m_init.m_GameMode.ToString();
                break;
            case EMainInitInfoType.Map:
                m_text.text = m_init.m_Level.ToString();
                break;
            case EMainInitInfoType.Rounds:
                m_text.text = m_init.m_Rounds.ToString();
                break;
            case EMainInitInfoType.Timer:
                m_text.text = m_init.m_Timer.ToString();
                break;
            default:
                break;
        }
    }
}
