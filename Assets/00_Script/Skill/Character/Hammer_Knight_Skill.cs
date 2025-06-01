using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hammer_Knight_Skill : Skill_Base
{

    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT_MIN = 1.85f;
    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT_MAX = 5.35f;

    private float LifeTime = 4.0f;
    private GameObject Hammer_Knight_Skill_Effect;

    public override void Set_Skill()
    {
        gameObject.GetComponent<Speech_Character>().Init();
        base.Set_Skill();

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            Hammer_Knight_Skill_Effect = Instantiate(Resources.Load<GameObject>("Prefabs/Hammer_Knight_Skill_Effect"));
            Destroy(Hammer_Knight_Skill_Effect, LifeTime);
        }
        
        StartCoroutine(Set_Skill_Coroutine());
        
    }

    public override void ReturnSkill()
    {       
        base.ReturnSkill();
    }


    IEnumerator Set_Skill_Coroutine()
    {
        try
        {
            Base_Manager.SOUND.Play(Sound.BGS, "Hammer_Knight");
            var Damage_Multiple = Random.Range(SKILL_DAMAGE_MULTIPLE_CONSTATNT_MIN, SKILL_DAMAGE_MULTIPLE_CONSTATNT_MAX);

            // 안전한 복사본 생성
            var currentMonsters = Spawner.m_monsters
                .Where(mon => mon != null)
                .ToList();

            foreach (var monster in currentMonsters)
            {
                if (Vector3.Distance(monster.transform.position, Vector3.zero) <= 4.0f)
                {
                    monster.GetDamage(gameObject.GetComponent<Player>().ATK * Damage_Multiple);
                }
            }

            if (!Utils.is_Skill_Effect_Save_Mode && Hammer_Knight_Skill_Effect != null)
            {
                Hammer_Knight_Skill_Effect.transform.position = Vector3.zero;
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

            Debug.Log("[Hammer_Knight_Skill] ReturnSkill 실행됨");
            ReturnSkill();
        }
    }
}
