using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance { get; private set; }
    
    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void DealDamage(float _amount, bool _toLeftSide)
    {
        if (_toLeftSide)
            GameManager.Instance.m_Player_L.Health -= _amount;
        else
            GameManager.Instance.m_Player_R.Health -= _amount;
    }
}
