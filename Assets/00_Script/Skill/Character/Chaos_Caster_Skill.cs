using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chaos_Caster_Skill : Skill_Base
{
    private const float SKILL_DAMAGE_MIN = 5.35f;
    private const float SKILL_DAMAGE_MAX = 8.00f;

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
                Base_Manager.SOUND.Play(Sound.BGS, "Chaos_Caster");

                var ps = Skill_Effect.GetComponent<ParticleSystem>();
                if (ps != null) ps.Play();
            }

            float damageMultiple = Random.Range(SKILL_DAMAGE_MIN, SKILL_DAMAGE_MAX);

            for (int i = 0; i < 3; i++)
            {
                // monsters가 null이거나 변경 도중 예외 방지
                var localMonsters = monsters?.Where(m => m != null).ToList();

                foreach (var monster in localMonsters)
                {
                    if (Distance(transform.position, monster.transform.position, 5.0f))
                    {
                        monster.GetDamage(gameObject.GetComponent<Player>().ATK * damageMultiple);
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

            Debug.Log("[Chaos_Caster_Skill] ReturnSkill 실행됨");
            ReturnSkill();
        }
    }
}
