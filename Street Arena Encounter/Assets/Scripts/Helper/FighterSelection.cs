using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSelection : MonoBehaviour
{
    [SerializeField] Main_Init m_init;
    [SerializeField] List<Fighter_Info> m_fighterInfos = new List<Fighter_Info>();

    public void SetFighterInfo_L(Fighter_Info _Player)
    {
        m_init.m_Player_L = _Player;
    }

    public void SetFighterInfo_R(Fighter_Info _Player)
    {
        m_init.m_Player_R = _Player;
        InfoBoxManager.Instance.UpdateList();
    }
    public void SetRandomFighters()
    {
        SetFighterInfo_L(
            m_fighterInfos[
                Random.Range(
                    0, 
                    m_fighterInfos.Count)]);
        SetFighterInfo_R(
            m_fighterInfos[
                Random.Range(
                    0,
                    m_fighterInfos.Count)]);
    }
}
