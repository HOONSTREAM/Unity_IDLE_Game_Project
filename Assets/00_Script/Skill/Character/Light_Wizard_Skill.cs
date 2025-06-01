using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Light_Wizard_Skill : Skill_Base
{
    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT_MIN = 1.15f;
    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT_MAX = 4.35f;

    private float LifeTime = 6.0f;
    private GameObject Light_Wizard_Skill_Effect;

    public override void Set_Skill()
    {
        gameObject.GetComponent<Speech_Character>().Init();
        base.Set_Skill();

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            Light_Wizard_Skill_Effect = Instantiate(Resources.Load<GameObject>("Prefabs/Light_Wizard_Skill_Effect"));
            Destroy(Light_Wizard_Skill_Effect, LifeTime);
        }

        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {
        try
        {
            Base_Manager.SOUND.Play(Sound.BGS, "Light_Wizard");

            float damageMultiple = Random.Range(SKILL_DAMAGE_MULTIPLE_CONSTATNT_MIN, SKILL_DAMAGE_MULTIPLE_CONSTATNT_MAX);

            yield return new WaitForSecondsRealtime(2.0f);

            var monsterSnapshot = Spawner.m_monsters?.Where(m => m != null).ToList();

            foreach (var monster in monsterSnapshot)
            {
                if (Vector3.Distance(monster.transform.position, Vector3.zero) <= 4.0f)
                {
                    monster.GetDamage(gameObject.GetComponent<Player>().ATK * damageMultiple);
                }
            }

            if (!Utils.is_Skill_Effect_Save_Mode && Light_Wizard_Skill_Effect != null)
            {
                Light_Wizard_Skill_Effect.transform.position = Vector3.zero;
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

            Debug.Log("[Light_Wizard_Skill] ReturnSkill ½ÇÇàµÊ");
            ReturnSkill();
        }
    }
}
