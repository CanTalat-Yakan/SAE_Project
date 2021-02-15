using UnityEngine;

[CreateAssetMenu(menuName = "Char/Fighter Info", fileName = "Fighter Init", order = 0)]
public class Fighter_Info : ScriptableObject
{
    public string Name;
    public Sprite Image;
    public GameObject Model;
    public AnimatorOverrideController AnimatorOC;
    public float GroundOffset;
}
