using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Winter_Bringer_Skill : Skill_Base
{
    private const float SKILL_DAMAGE_MIN = 1.5f;
    private const float SKILL_DAMAGE_MAX = 3.5f;

    private float LifeTime = 3.0f;

    private GameObject WinterBringer_Skill_Effect;

    public override void Set_Skill()
    {
       
        gameObject.GetComponent<Speech_Character>().Init();
        base.Set_Skill();

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            WinterBringer_Skill_Effect = Instantiate(Resources.Load<GameObject>("Prefabs/Winter_Bringer_Skill_Effect"));
            Destroy(WinterBringer_Skill_Effect, LifeTime);
        }
       
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {
        try
        {
            Base_Manager.SOUND.Play(Sound.BGS, "Winter_Bringer");

            float damageMultiple = Random.Range(SKILL_DAMAGE_MIN, SKILL_DAMAGE_MAX);

            var monsterSnapshot = Spawner.m_monsters?.Where(m => m != null).ToList();

            foreach (var monster in monsterSnapshot)
            {
                if (Vector3.Distance(monster.transform.position, Vector3.zero) <= 4.0f)
                {
                    monster.GetDamage(gameObject.GetComponent<Player>().ATK * damageMultiple);
                }
            }

            if (!Utils.is_Skill_Effect_Save_Mode && Skill_Effect != null)
            {
                Skill_Effect.transform.position = Vector3.zero;
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

            Debug.Log("[Winter_Bringer_Skill] ReturnSkill ½ÇÇàµÊ");
            ReturnSkill();
        }
    }
}
