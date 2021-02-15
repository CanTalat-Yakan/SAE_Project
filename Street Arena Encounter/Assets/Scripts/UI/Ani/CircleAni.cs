using UnityEngine;
using DG.Tweening;

public class CircleAni : MonoBehaviour
{
    #region //Fields
    [SerializeField] Ease m_ease;
    [SerializeField] float m_duration = 5;
    [SerializeField] bool m_randomOffsetDuration = false;
    [SerializeField] float m_randomOffsetPos = 100;
    Vector2 m_initialPos = new Vector2();
    #endregion


    void Start()
    {
        m_initialPos = GetComponent<RectTransform>().position;
        Play();

    }

    #region //Utilities
    /// <summary>
    /// Plays Animation with transform in both x and y axis.
    /// </summary>
    void Play()
    {
        MoveX();
        MoveY();
    }

    void MoveX()
    {
        Vector2 pos = m_initialPos;
        float dur = m_duration;
        if (m_randomOffsetDuration)
            dur *= Random.Range(0.6f, 1);
        pos.x += Random.Range(-1, 1) * m_randomOffsetPos;
        transform.DOMoveX(pos.x, dur).SetEase(m_ease).OnComplete(MoveX);
    }
    void MoveY()
    {
        Vector2 pos = m_initialPos;
        float dur = m_duration;
        if (m_randomOffsetDuration)
            dur *= Random.Range(0.6f, 1);
        pos.y += Random.Range(-1, 1) * m_randomOffsetPos;
        transform.DOMoveY(pos.y, dur).SetEase(m_ease).OnComplete(MoveY);
    }
    #endregion
}
