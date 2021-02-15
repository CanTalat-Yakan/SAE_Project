using UnityEngine;
using DG.Tweening;

public class BarAni : MonoBehaviour
{
    #region //Fields
    [SerializeField] Ease m_ease;
    [SerializeField] float m_duration = 5;
    [SerializeField] bool m_randomOffsetDuration = false;
    [SerializeField] float m_randomOffsetPos = 100;
    float m_initialPos = 0;
    #endregion


    void Start()
    {
        m_initialPos = GetComponent<RectTransform>().position.x;
        Play();
    }

    /// <summary>
    /// Plays an Animation with the transform.
    /// </summary>
    void Play()
    {
        float pos = m_initialPos;
        float dur = m_duration;
        if (m_randomOffsetDuration)
            dur *= Random.Range(0.6f, 1);
        pos += Random.Range(-1, 1) * m_randomOffsetPos;
        transform.DOMoveX(pos, dur).SetEase(m_ease).OnComplete(Play);
    }
}
