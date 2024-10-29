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
            Camera_Manager.instance.Camera_Shake();
            player.GetDamage(10);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public override void Set_Skill()
    {
        base.Set_Skill();
        StartCoroutine(B_Skill_Coroutine());
    }
}
