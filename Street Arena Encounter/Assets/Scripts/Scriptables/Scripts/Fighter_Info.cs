using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Char/Fighter Info", fileName = "Fighter Init", order = 0)]
public class Fighter_Info : ScriptableObject
{
    public string Name;
    public Sprite Image;
    public GameObject Model;
    public AnimatorOverrideController AnimatorOC;
    public float GroundOffset;
}
