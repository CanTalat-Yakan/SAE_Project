using UnityEngine;

public class circleAround : MonoBehaviour
{
    void Update()
    {
        Vector3 newPos = Vector3.zero;
        newPos.y += Mathf.Sin(Time.time) * 0.5f;
        transform.localPosition += newPos * (Time.deltaTime * 8);
    }
}
