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
        Skill_Effect.gameObject.SetActive(true);
        Base_Manager.SOUND.Play(Sound.BGS, "Dragon_Knight");
        Skill_Effect.GetComponent<ParticleSystem>().Play();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < monsters.Count(); j++)
            {
                if (Distance(transform.position, monsters[j].transform.position, 2.0f))
                {
                    monsters[j].GetDamage(gameObject.GetComponent<Player>().ATK * SKILL_DAMAGE_MULTIPLE_CONSTATNT); // 120% 의 데미지를 가한다.
                }
            }

            yield return new WaitForSeconds(0.5f);

        }

        ReturnSkill();
        Skill_Effect.gameObject.SetActive(false);

    }
}
