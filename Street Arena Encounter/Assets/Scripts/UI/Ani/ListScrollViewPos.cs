using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ListScrollViewPos : MonoBehaviour, EventInterface
{
    [SerializeField] GameObject m_list;
    [SerializeField] GameObject m_bar;
    [SerializeField] float m_listPos;
    [SerializeField] bool m_barVisible;
    [SerializeField] bool m_lerp;
    [SerializeField] float m_lerpDuration;

    public void Action()
    {
        if (!m_lerp)
            m_list.transform.localPosition = new Vector3(
                m_listPos,
                m_list.transform.localPosition.y,
                m_list.transform.localPosition.z);
        else
            m_list.transform.DOLocalMoveX(m_listPos, m_lerpDuration);

        m_bar.SetActive(m_barVisible);
    }
}
