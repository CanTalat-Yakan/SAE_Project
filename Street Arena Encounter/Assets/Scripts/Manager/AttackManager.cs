using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[Serializable]
public struct SThrowingObject
{
    public GameObject Cap;
    public GameObject Cigarette;
}
public class AttackManager : MonoBehaviour
{
    #region //Values
    public static AttackManager Instance { get; private set; }

    public SThrowingObject m_ThrowingObjectType;

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

    #region //Utilities
    public void InitThrowObj(GameObject _objType, Vector3 _origin, float _dir, float _speed, float _destroyTime)
    {
        if (!GameManager.Instance.STARTED)
            return;

        GameObject gObj = Instantiate(_objType, _origin, Quaternion.identity, this.gameObject.transform);

        gObj.tag = _objType.ToString();
        gObj.AddComponent<Rigidbody>().velocity = Vector3.right * _dir * _speed;

        Destroy(gObj, _destroyTime);
    }

    public void Dash(PlayerInformation _playerInfo, float _force = 8, float _drag = 30)
    {
        _playerInfo.Player.m_MovementController.Force(_force * _playerInfo.Forward, _drag);
    }

    public void SetSpecial(bool _toLeft, bool _active)
    {
        if (_toLeft)
        {
            if (_active)
            {
                m_ps_L.Play();
                GameManager.Instance.m_Player_L.GetMaterial.SetColor("_EmissionColor", Color.red);
            }
            else
            {
                GameManager.Instance.m_Player_L.GetMaterial.SetColor("_EmissionColor", Color.black);
                m_ps_L.Stop();
            }
        }
        else
        {
            if (_active)
            {
                m_ps_R.Play();
                GameManager.Instance.m_Player_R.GetMaterial.SetColor("_EmissionColor", Color.red);
            }
            else
            {
                GameManager.Instance.m_Player_R.GetMaterial.SetColor("_EmissionColor", Color.black);
                m_ps_R.Stop();
            }
        }
    }
    public bool GetSpecial(bool _fromLeft)
    {
        return (_fromLeft) ? m_ps_L.isPlaying: m_ps_R.isPlaying;
    }
    #endregion
}