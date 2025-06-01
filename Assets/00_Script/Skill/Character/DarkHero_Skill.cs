using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class DarkHero_Skill : Skill_Base
{
    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT_MIN = 4.25f;
    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT_MAX = 10.0f;

    private float LifeTime = 3.0f;
    private GameObject DarkHero_Skill_Effect;

    public override void Set_Skill()
    {
        gameObject.GetComponent<Speech_Character>().Init();
        base.Set_Skill();

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            DarkHero_Skill_Effect = Instantiate(Resources.Load<GameObject>("Prefabs/Dark_Hero_Skill_Effect"));
            Destroy(DarkHero_Skill_Effect, LifeTime);
        }
       
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {

        try
        {
            Base_Manager.SOUND.Play(Sound.BGS, "DarkHero");

            float damageMultiple = Random.Range(SKILL_DAMAGE_MULTIPLE_CONSTATNT_MIN, SKILL_DAMAGE_MULTIPLE_CONSTATNT_MAX);

            var monstersSnapshot = Spawner.m_monsters?.Where(m => m != null).ToList();

            foreach (var monster in monstersSnapshot)
            {
                if (Vector3.Distance(monster.transform.position, Vector3.zero) <= 4.0f)
                {
                    monster.GetDamage(gameObject.GetComponent<Player>().ATK * damageMultiple);
                }
            }

            if (!Utils.is_Skill_Effect_Save_Mode && DarkHero_Skill_Effect != null)
            {
                DarkHero_Skill_Effect.transform.position = new Vector3(0.0f, 15.0f, 0.0f);
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

            Debug.Log("[DarkHero_Skill] ReturnSkill ½ÇÇàµÊ");
            ReturnSkill();
        }
    }
}

