using System;
using UnityEngine;

[Serializable]
public struct SFrameBasedAtackSettings
{
    public EAttackStates State;
    public AnimationClip AnimationClip;
    public EDamageStates DamageType;
    public float Damage_Amount;
    public float Damage_Range;
    public bool Dash;
    public bool FreezeTime;
    public float Activation_FrameTime;
    public float Damage_FrameTime;
    public float Recovery_FrameTime;
}

[CreateAssetMenu(menuName = "Settings/Attack Settings", fileName = "Attack Settings", order = 2)]
public class Attack_Settings : ScriptableObject
{
    public SFrameBasedAtackSettings[] Attacks;
}
