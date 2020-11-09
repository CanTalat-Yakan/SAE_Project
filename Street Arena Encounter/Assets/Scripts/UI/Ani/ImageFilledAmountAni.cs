using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ImageFilledAmountAni : MonoBehaviour
{
    Image m_image;
    [SerializeField] float m_speed = 2;
    [SerializeField] bool m_sin = false;

    void Start()
    {
        m_image = GetComponent<Image>();
    }

    void Update()
    {
        ChangeParams();

        float add = m_speed * Time.deltaTime;
        if (m_sin)
            add *= Mathf.Sin(m_image.fillAmount + 0.2f);

        m_image.fillAmount += add;
    }

    void ChangeParams()
    {
        if (m_image.fillAmount == 1)
        {
            if (m_image.fillMethod == Image.FillMethod.Radial360)
                m_image.fillClockwise = true;
            if (m_image.fillMethod == Image.FillMethod.Horizontal)
                m_image.fillOrigin = 0;
            m_speed *= -1;
        }
        else if (m_image.fillAmount == 0)
        {
            if (m_image.fillMethod == Image.FillMethod.Radial360)
                m_image.fillClockwise = false;
            if (m_image.fillMethod == Image.FillMethod.Horizontal)
                m_image.fillOrigin = 1;
            m_speed *= -1;
        }

    }
}
