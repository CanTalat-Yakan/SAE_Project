using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class imageRotateAni : MonoBehaviour
{
    [SerializeField] float m_speed;

    void Update()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, m_speed * 100 * Time.time);
    }
}
