using System;
using UnityEngine;

[Serializable]
public struct SThrowingObject
{
    public GameObject Cap;
    public GameObject Cigarette;
}

public class AttackManager : MonoBehaviour
{
    #region //Properties
    public static AttackManager Instance { get; private set; }
    #endregion

    #region //Fields
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

    public void ActivateSpecialVFX(bool _fromLeft)
    {
        if (_fromLeft)
            SetSpecialVFX(
                true,
                m_ps_L,
                GameManager.Instance.m_Player_L.GetMaterial);
        else
            SetSpecialVFX(
                true,
                m_ps_R,
                GameManager.Instance.m_Player_R.GetMaterial);
    }
    public void DeactivateSpecialVFX(bool _fromLeft)
    {
        if (_fromLeft)
            SetSpecialVFX(
                false,
                m_ps_L,
                GameManager.Instance.m_Player_L.GetMaterial);
        else
            SetSpecialVFX(
                false,
                m_ps_R,
                GameManager.Instance.m_Player_R.GetMaterial);
    }
    #endregion

    #region //Helper
    void SetSpecialVFX(bool _activate, ParticleSystem _ps, Material _material)
    {
        if (_activate)
        {
            _ps.Play();
            _material.SetColor("_EmissionColor", Color.red);
        }
        else
        {
            _ps.Stop();
            _material.SetColor("_EmissionColor", Color.black);
        }
    }
    #endregion
}