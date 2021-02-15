using UnityEngine;
using UnityEngine.UI;

public class ImageFlicker : MonoBehaviour
{
    #region //Fields
    [SerializeField] Color m_targetColor;
    [SerializeField] float m_speed = 1f;
    Color m_startColor;
    Image m_image;
    float m_intensityOverTime;
    #endregion


    void Start()
    {
        m_image = GetComponent<Image>();
        m_startColor = m_image.color;
    }

    void Update()
    {
        // Change Intensity with Sinus Curve
        m_intensityOverTime = 0.5f - 0.5f * Mathf.Sin(Time.time * m_speed);

        // Apply values
        m_image.color = m_startColor * (1-m_intensityOverTime)+ m_targetColor * m_intensityOverTime;
    }
}
