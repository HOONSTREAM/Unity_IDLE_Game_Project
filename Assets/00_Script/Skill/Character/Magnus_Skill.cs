using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Magnus_Skill : Skill_Base
{
    private const float SKILL_DAMAGE_MIN = 1.2f;
    private const float SKILL_DAMAGE_MAX = 4.5f;

    
    public override void Set_Skill()
    {
        gameObject.GetComponent<Speech_Character>().Init();
        base.Set_Skill();
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {
        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            Skill_Effect.gameObject.SetActive(true);
            Base_Manager.SOUND.Play(Sound.BGS, "Magnus");
            Skill_Effect.GetComponent<ParticleSystem>().Play();
        }
        

        var Damage_Multiple = Random.Range(SKILL_DAMAGE_MIN, SKILL_DAMAGE_MAX);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < monsters.Count(); j++)
            {
                if (Distance(transform.position, monsters[j].transform.position, 2.0f))
                {
                    monsters[j].GetDamage(gameObject.GetComponent<Player>().ATK * Damage_Multiple); 
                }
            }

            yield return new WaitForSeconds(0.5f);

        }

        ReturnSkill();
    }
}
