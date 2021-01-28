﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[Serializable]
public struct SThrowingObject
{
    public GameObject rhoeObjKind;
}
public class AttackManager : MonoBehaviour
{
    #region -values
    public static AttackManager Instance { get; private set; }

    [SerializeField] SThrowingObject m_object;
    public AnimationClip[] clips = new AnimationClip[6];
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

    public void InitThrowObj(Vector3 _origin, float _dir, float _speed)
    {
        StartCoroutine(ThrowObj(_origin, _dir, _speed));
    }
    IEnumerator ThrowObj(Vector3 _origin, float _dir, float _speed)
    {
        if (!GameManager.Instance.STARTED)
            yield return null;

        GameObject gObj = GameObject.Instantiate(m_object.rhoeObjKind, _origin, Quaternion.identity, this.gameObject.transform);
        yield return new WaitForSeconds(1);
        gObj.transform.DOMoveX(_dir * 25, _speed);
        Destroy(gObj, _speed);
        yield return null;
    }

    public void Throwback(PlayerInformation _PlayerInfo, float _distance, float _time, bool _map = true)
    {
        if (!GameManager.Instance.BoolDistance(_PlayerInfo.GP.MinDistance))
            return;

        float targetX = _PlayerInfo.Char.gameObject.transform.localPosition.x + _PlayerInfo.Forward * _distance;
        //if (_map)
        //    targetX *= GameManager.Instance.MapDistance(_PlayerInfo.GP.MinDistance);

        _PlayerInfo.Char.gameObject.transform.DOLocalMoveX(targetX, _time).SetEase(Ease.OutCubic);
    }
}