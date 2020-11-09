using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Gameplay Settings", fileName = "GP Settings", order = 1)]
public class GP_Settings : ScriptableObject
{
    [Header("Movement")]
    public float JumpForce = 5;
    public float GravityForce = 12;
    [Tooltip("0 is sneaking, 1 is walking, 2 is running")]
    public float[] Speed = new float[3] { 1.5f, 3f, 6f };
    [Header("Setter")]
    public float PlayerHeight = 2f;
    [Range(0.1f, 10)] public float CrouchTimeRatio = 5;
    [Range(0.1f, 1)] public float CrouchHeightRatio = 0.75f;
}
