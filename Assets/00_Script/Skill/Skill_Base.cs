using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    protected Monster[] monsters { get { return Spawner.m_monsters.ToArray(); } }   
    protected Player[] players { get { return Spawner.m_players.ToArray(); } }

    public virtual void Set_Skill()
    {

    }
}
