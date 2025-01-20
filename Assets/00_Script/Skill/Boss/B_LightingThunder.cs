using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_LightingThunder : Skill_Base
{
    IEnumerator B_Skill_Coroutine()
    {
       for(int i = 0; i< players.Length; i++)
        {
            Player player = players[Random.Range(0, players.Length)];
            Instantiate(Resources.Load<GameObject>("Boss_Electric"), player.transform.position, Quaternion.identity);
           
            player.GetDamage(10);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public override void Set_Skill()
    {
        base.Set_Skill();

        for (int i = 0; i < Spawner.m_players.Count; i++)
        {
            if (Spawner.m_players[i] == null || !Spawner.m_players[i].gameObject.activeInHierarchy)
            {
                Debug.LogWarning($"Player at index {i} is invalid or destroyed. Removing from m_players.");
                Spawner.m_players.RemoveAt(i);
                i--; // ÀÎµ¦½º Á¶Á¤
            }
        }

        StartCoroutine(B_Skill_Coroutine());
    }
}
