using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Gameplay Settings", fileName = "GP Settings", order = 1)]
public class GP_Settings : ScriptableObject
{
    public float PlayerRadius = 2;
    public float MaxPlayerDistance = 25;
    public float JumpForce = 10;
    public float JumpDashForce = 15;
    public float DashForce = 30;
    public float GravityForce = -40;
    public float MovementSpeed = 1;
    public float PlayerHeight = 3.1f;
    public float PlayerStartPos = 3;
    [Range(0.1f, 1)] public float CrouchHeight = 0.74f;
    public float Health = 100;
}
