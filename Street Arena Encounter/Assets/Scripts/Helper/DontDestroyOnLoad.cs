using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    static DontDestroyOnLoad Instance;
    [SerializeField] bool m_Singleton;

    void Awake()
    {
        if (Instance)
            if (m_Singleton)
            {
                Destroy(gameObject);
                return;
            }
        Instance = this;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
