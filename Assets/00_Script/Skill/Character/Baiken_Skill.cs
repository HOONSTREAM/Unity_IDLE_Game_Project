using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Baiken_Skill : Skill_Base
{

    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT_MIN = 3.25f;
    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT_MAX = 7.5f;

    private float LifeTime = 5.0f;
    private GameObject Baiken_Skill_Effect;

    public override void Set_Skill()
    {
        gameObject.GetComponent<Speech_Character>().Init();

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            Baiken_Skill_Effect = Instantiate(Resources.Load<GameObject>("Prefabs/Baiken_Skill_Effect"));
            Destroy(Baiken_Skill_Effect, LifeTime);


            StartCoroutine(Set_Skill_Coroutine());
            base.Set_Skill();
        }
    }

    public override void ReturnSkill()
    {       
        base.ReturnSkill();
    }


    IEnumerator Set_Skill_Coroutine()
    {
        try
        {
            Base_Manager.SOUND.Play(Sound.BGS, "Baiken");
            float damageMultiple = Random.Range(SKILL_DAMAGE_MULTIPLE_CONSTATNT_MIN, SKILL_DAMAGE_MULTIPLE_CONSTATNT_MAX);
           
            var monsterSnapshot = Spawner.m_monsters?.Where(m => m != null).ToList();

            foreach (var monster in monsterSnapshot)
            {
                if (Vector3.Distance(monster.transform.position, Vector3.zero) <= 4.0f)
                {
                    
                    monster.GetDamage(gameObject.GetComponent<Player>().ATK * damageMultiple);
                }
            }

            if (!Utils.is_Skill_Effect_Save_Mode && Baiken_Skill_Effect != null)
            {
                Baiken_Skill_Effect.transform.position = Vector3.zero;
            }

            yield return new WaitForSecondsRealtime(2.0f);
        }
        finally
        {
            var player = gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Use_Skill = false;
            }

            Debug.Log("[Baiken] ReturnSkill ½ÇÇàµÊ");
            ReturnSkill();
        }
    }
}

