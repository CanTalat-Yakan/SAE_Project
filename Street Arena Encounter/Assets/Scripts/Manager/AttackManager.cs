using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AttackManager : MonoBehaviour
{
    public static AttackManager Instance { get; private set; }
    [SerializeField] GameObject m_fireBall;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void InitFireBall(Vector3 _origin, float _dir, float _speed)
    {
        StartCoroutine(FireBall(_origin, _dir, _speed));
    }
    IEnumerator FireBall(Vector3 _origin, float _dir, float _speed)
    {
        if (!GameManager.Instance.STARTED)
            yield return null;

        GameObject gObj = GameObject.Instantiate(m_fireBall, _origin, Quaternion.identity, this.gameObject.transform);
        yield return new WaitForSeconds(1);
        gObj.transform.DOMoveX(_dir * 25, _speed);
        Destroy(gObj, _speed);
        yield return null;
    }

    public void InitHeadButt()
    {
        StartCoroutine(HeadButt());

    }
    IEnumerator HeadButt()
    {
        if (!GameManager.Instance.STARTED)
            yield return null;

        yield return null;
    }

    public void InitBlock()
    {
        StartCoroutine(Block());
    }
    IEnumerator Block()
    {
        if (!GameManager.Instance.STARTED)
            yield return null;

        yield return null;
    }
}