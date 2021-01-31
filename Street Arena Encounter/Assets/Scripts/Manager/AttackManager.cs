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
    public AnimationClip[] clips = new AnimationClip[6];

    [SerializeField] ParticleSystem m_ps_L;
    [SerializeField] ParticleSystem m_ps_R;
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

    public void Dash(PlayerInformation _playerInfo, float _distance, float _time, bool _map = true)
    {
        if (_distance >= 0 && !GameManager.Instance.BoolDistance(_playerInfo.GP.MinDistance))
            return;

        float targetX = _playerInfo.Char.gameObject.transform.localPosition.x + _playerInfo.Forward * _distance;
        if (_map)
            targetX += _playerInfo.Forward * GameManager.Instance.MapDistance(_playerInfo.GP.MinDistance);

        _playerInfo.Char.gameObject.transform.DOLocalMoveX(targetX, _time).SetEase(Ease.OutCubic);
    }

    public void SetSpecial(bool _toLeft, bool _active = true)
    {
        if (_toLeft)
        {
            if (_active)
            {
                m_ps_L.Play();
                GameManager.Instance.m_Player_L.getMaterial.SetColor("_EmissionColor", Color.red);
            }
            else
            {
                GameManager.Instance.m_Player_L.getMaterial.SetColor("_EmissionColor", Color.black);
                m_ps_L.Stop();
            }
        }
        else
        {
            if (_active)
            {
                m_ps_R.Play();
                GameManager.Instance.m_Player_R.getMaterial.SetColor("_EmissionColor", Color.red);
            }
            else
            {
                GameManager.Instance.m_Player_R.getMaterial.SetColor("_EmissionColor", Color.black);
                m_ps_R.Stop();
            }
        }
    }
    public bool GetSpecial(bool _fromLeft)
    {
        return (_fromLeft) ? m_ps_L.isPlaying: m_ps_R.isPlaying;
    }
}