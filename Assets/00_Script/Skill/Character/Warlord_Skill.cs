using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Warlord_Skill : Skill_Base
{

    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT_MIN = 1.5f;
    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT_MAX = 3.0f;

    private float LifeTime = 5.0f;
    private GameObject Warlord_Skill_Effect;

    public override void Set_Skill()
    {
        gameObject.GetComponent<Speech_Character>().Init();
        Warlord_Skill_Effect = Instantiate(Resources.Load<GameObject>("Prefabs/Warlord_Skill_Effect"));
        Destroy(Warlord_Skill_Effect, LifeTime);
        StartCoroutine(Set_Skill_Coroutine());
        base.Set_Skill();
    }

    public override void ReturnSkill()
    {       
        base.ReturnSkill();
    }


    IEnumerator Set_Skill_Coroutine()
    {
        var Damage_Multiple = Random.Range(SKILL_DAMAGE_MULTIPLE_CONSTATNT_MIN, SKILL_DAMAGE_MULTIPLE_CONSTATNT_MAX);

        Base_Manager.SOUND.Play(Sound.BGS, "Warlord");

        Warlord_Skill_Effect.transform.position = Vector3.zero;

        for (int i = 0; i<5; i++)
        {
            for(int j = 0; j < monsters.Count(); j++)
            {
               if(Distance(transform.position, monsters[j].transform.position, 4.0f))
                {
                    monsters[j].GetDamage(gameObject.GetComponent<Player>().ATK * Damage_Multiple); 
                }
            }

            yield return new WaitForSeconds(0.5f);
            
        }

       
        ReturnSkill();
        
    }
}
