using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Poison_Skill : Skill_Base
{
    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT_MIN = 1.05f;
    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT_MAX = 4.8f;

    private float LifeTime = 6.0f;
    private GameObject Poison_Skill_Effect;

    public override void Set_Skill()
    {
        gameObject.GetComponent<Speech_Character>().Init();
        base.Set_Skill();

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            Poison_Skill_Effect = Instantiate(Resources.Load<GameObject>("Prefabs/Poison_Skill_Effect"));
            Destroy(Poison_Skill_Effect, LifeTime);
        }
       
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {

        try
        {
            float damageMultiple = Random.Range(SKILL_DAMAGE_MULTIPLE_CONSTATNT_MIN, SKILL_DAMAGE_MULTIPLE_CONSTATNT_MAX);



            if (!Utils.is_Skill_Effect_Save_Mode && Poison_Skill_Effect != null)
            {
                Poison_Skill_Effect.transform.position = Vector3.zero;
            }
            
            for (int i = 0; i < 5; i++)
            {               
                yield return new WaitForSecondsRealtime(0.5f);

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
            Debug.Log("[Poison] ReturnSkill ½ÇÇàµÊ");
            ReturnSkill();
        }
    }
}


