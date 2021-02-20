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
    public int Activation_FrameTime;
    public int Damage_FrameTime;
    public int Recovery_FrameTime;
    public int Penalty_FrameTime;
}

[CreateAssetMenu(menuName = "Settings/Attack Settings", fileName = "Attack Settings", order = 2)]
public class Attack_Settings : ScriptableObject
{
    public SFrameBasedAtackSettings[] Attacks;
}
