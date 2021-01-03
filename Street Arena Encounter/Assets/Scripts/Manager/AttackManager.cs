using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[Serializable]
public struct SThrowingObject
{
    [SerializeField]
    public GameObject fireBall;
}
public class AttackManager : MonoBehaviour
{
    #region -values
    public static AttackManager Instance { get; private set; }
    [SerializeField] SThrowingObject m_object;
    #endregion

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

        GameObject gObj = GameObject.Instantiate(m_object.fireBall, _origin, Quaternion.identity, this.gameObject.transform);
        yield return new WaitForSeconds(1);
        gObj.transform.DOMoveX(_dir * 25, _speed);
        Destroy(gObj, _speed);
        yield return null;
    }

}