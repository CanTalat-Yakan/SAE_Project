using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Gameplay Settings", fileName = "GP Settings", order = 1)]
public class GP_Settings : ScriptableObject
{
    public float MinDistance = 2;
    public float MaxDistance = 25;
    public float JumpForce = 10;
    public float JumpDashDistance = 15;
    public float DashDistance = 30;
    public float GravityForce = -40;
    public float Speed = 1;
    public float PlayerHeight = 3.1f;
    public float PlayerStartPos = 3;
    [Range(0.1f, 1)] public float CrouchHeightRatio = 0.74f;
    [HideInInspector] public float CrouchHeight { get => PlayerHeight * CrouchHeightRatio; set => CrouchHeight = value; }
    public float Health = 100f;
}
