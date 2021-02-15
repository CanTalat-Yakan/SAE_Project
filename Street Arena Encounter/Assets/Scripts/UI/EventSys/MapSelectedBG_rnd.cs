using UnityEngine;
using UnityEngine.UI;

public class MapSelectedBG_rnd : MonoBehaviour, EventInterface
{
    #region //Fields
    [SerializeField] Image m_bg;
    [SerializeField] float m_waitDuration;
    [SerializeField] Sprite[] m_img;
    float tmpTimer;
    #endregion


    public void Action()
    {
        tmpTimer += Time.deltaTime;

        if (tmpTimer < m_waitDuration)
            return;

        Change();
        tmpTimer = 0;
    }

    void Change()
    {
        if (m_img.Length < 2)
            return;
        Sprite img;
        do
        {
            img = m_img[Random.Range(0, m_img.Length)];
        }
        while (img == m_bg.sprite);

        m_bg.sprite = img;
    }
}
