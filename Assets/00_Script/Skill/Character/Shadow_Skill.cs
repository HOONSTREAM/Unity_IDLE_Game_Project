using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Shadow_Skill : Skill_Base
{
    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT_MIN = 5.25f;
    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT_MAX = 8.0f;

    private float LifeTime = 6.0f;
    private GameObject Shadow_Skill_Effect;

    public override void Set_Skill()
    {
        gameObject.GetComponent<Speech_Character>().Init();
        base.Set_Skill();

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            Shadow_Skill_Effect = Instantiate(Resources.Load<GameObject>("Prefabs/Shadow_Skill_Effect"));
            Destroy(Shadow_Skill_Effect, LifeTime);
        }
       
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {

        try
        {
            float damageMultiple = Random.Range(SKILL_DAMAGE_MULTIPLE_CONSTATNT_MIN, SKILL_DAMAGE_MULTIPLE_CONSTATNT_MAX);



            if (!Utils.is_Skill_Effect_Save_Mode && Shadow_Skill_Effect != null)
            {
                Shadow_Skill_Effect.transform.position = Vector3.zero;
            }

            for (int i = 0; i < 5; i++)
            {
                Base_Manager.SOUND.Play(Sound.BGS, "DarkHero");
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
            Debug.Log("[Shadow] ReturnSkill ½ÇÇàµÊ");
            ReturnSkill();
        }
    }
}


