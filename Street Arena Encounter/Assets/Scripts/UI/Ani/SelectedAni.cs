using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SelectedAni : MonoBehaviour
{
    GameObject m_currentGObj;
    [SerializeField] float m_scaleFactor = 1.1f;

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(m_currentGObj);
            return;
        }

        if (EventSystem.current.currentSelectedGameObject.GetComponent<EventInterface>() != null)
            EventSystem.current.currentSelectedGameObject.GetComponent<EventInterface>().Action();

        Ani();
    }


    void Ani()
    {
        GameObject gobj = EventSystem.current.currentSelectedGameObject;
        if (!gobj)
            return;

        if (gobj != m_currentGObj)
        {
            if (m_currentGObj)
                Return(m_currentGObj);
            Play(gobj);
        }
        m_currentGObj = gobj;
    }
    //selected
    void Play(GameObject _gobj)
    {
        _gobj.transform.DOScale(m_scaleFactor, 0.1f);
    }

    //deselected 
    void Return(GameObject _gobj)
    {
        _gobj.transform.localScale = Vector3.one;
    }
}
