using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Starlist_Skill : Skill_Base
{
    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT = 1.45f;

    private float LifeTime = 3.0f;
    private GameObject Starlist_Skill_Effect;

    public override void Set_Skill()
    {
        gameObject.GetComponent<Speech_Character>().Init();
        base.Set_Skill();

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            Starlist_Skill_Effect = Instantiate(Resources.Load<GameObject>("Prefabs/Starlist_Skill_Effect"));
            Destroy(Starlist_Skill_Effect, LifeTime);
        }
        
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {
        try
        {
            Base_Manager.SOUND.Play(Sound.BGS, "Starlist");

            Camera_Manager.instance?.Camera_Shake();

            var skillATK = gameObject.GetComponent<Player>().ATK * SKILL_DAMAGE_MULTIPLE_CONSTATNT;
            var monsterSnapshot = Spawner.m_monsters?.Where(m => m != null).ToList();

            foreach (var monster in monsterSnapshot)
            {
                if (Vector3.Distance(monster.transform.position, Vector3.zero) <= 4.0f)
                {
                    monster.GetDamage(skillATK);
                }
            }

            if (!Utils.is_Skill_Effect_Save_Mode && Starlist_Skill_Effect != null)
            {
                Starlist_Skill_Effect.transform.position = Vector3.zero;
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

            Debug.Log("[Starlist_Skill] ReturnSkill ½ÇÇàµÊ");
            ReturnSkill();
        }
    }
}
