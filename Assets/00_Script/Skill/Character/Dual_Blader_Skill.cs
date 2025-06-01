using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dual_Blader_Skill : Skill_Base
{

    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT = 4.0f;

    public override void Set_Skill()
    {
        if (this.gameObject.GetComponent<Player>().isDead)
        {           
            return;
        }

        gameObject.GetComponent<Speech_Character>().Init();
        m_Player.AnimatorChange("isSKILL");

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            Skill_Effect.gameObject.SetActive(true);
        }
       
        StartCoroutine(Set_Skill_Coroutine());
        base.Set_Skill();

    }

    public override void ReturnSkill()
    {
        if (Skill_Effect != null)
        {
            Skill_Effect.gameObject.SetActive(false);
        }
        base.ReturnSkill();
    }

    IEnumerator Set_Skill_Coroutine()
    {
        try
        {
            double skillATK = gameObject.GetComponent<Player>().ATK * SKILL_DAMAGE_MULTIPLE_CONSTATNT;

            for (int i = 0; i < 5; i++)
            {
                var monsterSnapshot = monsters?.Where(m => m != null).ToList();

                foreach (var monster in monsterSnapshot)
                {
                    if (Distance(transform.position, monster.transform.position, 1.5f))
                    {
                        monster.GetDamage(skillATK); // 400% 데미지
                    }
                }

                yield return new WaitForSeconds(0.5f);
            }
        }
        finally
        {
            Debug.Log("[Dual_Blader_Skill] ReturnSkill 실행됨");
            ReturnSkill();
        }

    }
}
