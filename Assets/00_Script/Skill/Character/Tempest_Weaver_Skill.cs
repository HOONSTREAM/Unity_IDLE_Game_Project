using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Tempest_Weaver_Skill : Skill_Base
{
    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT_MIN = 1.35f;
    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT_MAX = 6.0f;

    private float LifeTime = 4.0f;
    private GameObject Tempest_Weaver_Skill_Effect;

    public override void Set_Skill()
    {
        gameObject.GetComponent<Speech_Character>().Init();
        base.Set_Skill();

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            Tempest_Weaver_Skill_Effect = Instantiate(Resources.Load<GameObject>("Prefabs/Tempest_Weaver_Skill_Effect"));
            Destroy(Tempest_Weaver_Skill_Effect, LifeTime);
        }

        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {
        try
        {
            

            float damageMultiple = Random.Range(SKILL_DAMAGE_MULTIPLE_CONSTATNT_MIN, SKILL_DAMAGE_MULTIPLE_CONSTATNT_MAX);

            Base_Manager.SOUND.Play(Sound.BGS, "Ele_Black");

            var monsterSnapshot = Spawner.m_monsters?.Where(m => m != null).ToList();

            foreach (var monster in monsterSnapshot)
            {
                if (Vector3.Distance(monster.transform.position, Vector3.zero) <= 4.0f)
                {
                    monster.GetDamage(gameObject.GetComponent<Player>().ATK * damageMultiple);
                }
            }

            if (!Utils.is_Skill_Effect_Save_Mode && Tempest_Weaver_Skill_Effect != null)
            {
                Tempest_Weaver_Skill_Effect.transform.position = Vector3.zero;
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

            Debug.Log("[Tempest_Weaver_Skill] ReturnSkill ½ÇÇàµÊ");
            ReturnSkill();
        }
    }
}
