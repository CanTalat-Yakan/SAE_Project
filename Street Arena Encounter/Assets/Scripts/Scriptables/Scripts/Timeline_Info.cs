using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(menuName = "Info/TimeLine Info", fileName = "TimeLine Info", order = 3)]
public class Timeline_Info : ScriptableObject
{
    public PlayableAsset[] m_TL_Beginning;
    public PlayableAsset[] m_TL_Special_L;
    public PlayableAsset[] m_TL_Special_R;
}
