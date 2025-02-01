using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dual_Blader_Skill : Skill_Base
{

    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT = 1.5f;

    public override void Set_Skill()
    {
        gameObject.GetComponent<Speech_Character>().Init();
        m_Player.AnimatorChange("isSKILL");
        Skill_Effect.gameObject.SetActive(true);
        StartCoroutine(Set_Skill_Coroutine());
        base.Set_Skill();

    }

    public override void ReturnSkill()
    {
        Skill_Effect.gameObject.SetActive(false);
        base.ReturnSkill();
    }

    IEnumerator Set_Skill_Coroutine()
    {
        for(int i = 0; i<5; i++)
        {
            for(int j = 0; j < monsters.Count(); j++)
            {
               if(Distance(transform.position, monsters[j].transform.position, 1.5f))
                {
                    monsters[j].GetDamage(gameObject.GetComponent<Player>().ATK * SKILL_DAMAGE_MULTIPLE_CONSTATNT); // 150% 의 데미지를 가한다.
                }
            }

            yield return new WaitForSeconds(0.5f);
            
        }

       
        ReturnSkill();
        
    }
}
