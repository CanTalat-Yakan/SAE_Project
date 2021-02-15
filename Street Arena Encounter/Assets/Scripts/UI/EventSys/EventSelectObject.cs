using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSelectObject : MonoBehaviour
{
    [SerializeField] GameObject _target;
    void Awake()
    {
        EventSystem.current.SetSelectedGameObject(_target);
    }
}
