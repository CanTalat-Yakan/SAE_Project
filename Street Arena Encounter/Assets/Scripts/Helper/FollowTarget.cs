using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    #region //Fields
    [SerializeField] GameObject m_target;
    [SerializeField] bool m_2D = false;
    Vector3 destiny;
    #endregion

    void Update()
    {
        destiny = m_target.transform.localPosition;

        if (m_2D)
            destiny.y = 0;

        transform.position = destiny;
    }
}
