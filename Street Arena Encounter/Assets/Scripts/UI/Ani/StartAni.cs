using UnityEngine;
using DG.Tweening;

public class StartAni : MonoBehaviour
{
    [SerializeField] Ease m_ease;
    [SerializeField] float m_scaleFactor = 2;
    [SerializeField] float m_duration = 5;

    void Start()
    {
        In();
    }
    void In()
    {
        transform.DOScale(1 * m_scaleFactor, m_duration).SetEase(m_ease).OnComplete(Out);
    }

    void Out()
    {
        transform.DOScale(1 / m_scaleFactor, m_duration).SetEase(m_ease).OnComplete(In);
    }
}