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

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            Warlord_Skill_Effect = Instantiate(Resources.Load<GameObject>("Prefabs/Warlord_Skill_Effect"));
            Destroy(Warlord_Skill_Effect, LifeTime);
        }
        
        StartCoroutine(Set_Skill_Coroutine());
        base.Set_Skill();
    }

    public override void ReturnSkill()
    {       
        base.ReturnSkill();
    }


    IEnumerator Set_Skill_Coroutine()
    {
        try
        {
            float damageMultiple = Random.Range(SKILL_DAMAGE_MULTIPLE_CONSTATNT_MIN, SKILL_DAMAGE_MULTIPLE_CONSTATNT_MAX);

            Base_Manager.SOUND.Play(Sound.BGS, "Warlord");

            if (!Utils.is_Skill_Effect_Save_Mode && Warlord_Skill_Effect != null)
            {
                Warlord_Skill_Effect.transform.position = Vector3.zero;
            }

            for (int i = 0; i < 5; i++)
            {
                var monsterSnapshot = monsters?.Where(m => m != null).ToList();

                foreach (var monster in monsterSnapshot)
                {
                    if (Distance(transform.position, monster.transform.position, 4.0f))
                    {
                        monster.GetDamage(gameObject.GetComponent<Player>().ATK * damageMultiple);
                    }
                }

                yield return new WaitForSeconds(0.5f);
            }
        }
        finally
        {
            Debug.Log("[Warlord_Skill] ReturnSkill ½ÇÇàµÊ");
            ReturnSkill();
        }
    }
}
