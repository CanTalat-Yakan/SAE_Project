using System;
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
    public AnimatorOverrideController Overrider;
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

}