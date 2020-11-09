using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelectedBG : MonoBehaviour, EventInterface
{
    [SerializeField] Image m_bg;
    [SerializeField] Sprite m_img;

    public void Action()
    {
        m_bg.sprite = m_img;
    }
}
