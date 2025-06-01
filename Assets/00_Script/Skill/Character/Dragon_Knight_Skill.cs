using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dragon_Knight_Skill : Skill_Base
{
    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT = 2.4f;

    public override void Set_Skill()
    {
        gameObject.GetComponent<Speech_Character>().Init();
        base.Set_Skill();
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {
        try
        {
            if (!Utils.is_Skill_Effect_Save_Mode && Skill_Effect != null)
            {
                Skill_Effect.gameObject.SetActive(true);
                Base_Manager.SOUND.Play(Sound.BGS, "Dragon_Knight");

                var ps = Skill_Effect.GetComponent<ParticleSystem>();
                if (ps != null) ps.Play();
            }

            double skillATK = gameObject.GetComponent<Player>().ATK * SKILL_DAMAGE_MULTIPLE_CONSTATNT;

            for (int i = 0; i < 3; i++)
            {
                var monsterSnapshot = monsters?.Where(m => m != null).ToList();

                foreach (var monster in monsterSnapshot)
                {
                    if (Distance(transform.position, monster.transform.position, 2.0f))
                    {
                        monster.GetDamage(skillATK); // 240% 데미지
                    }
                }

                yield return new WaitForSeconds(0.5f);
            }
        }
        finally
        {
            if (!Utils.is_Skill_Effect_Save_Mode && Skill_Effect != null)
            {
                Skill_Effect.gameObject.SetActive(false);
            }

            Debug.Log("[Dragon_Knight_Skill] ReturnSkill 실행됨");
            ReturnSkill();
        }

    }
}
