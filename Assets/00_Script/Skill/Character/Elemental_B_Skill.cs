using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Elemental_B_Skill : Skill_Base
{
    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT = 2.25f;

    private float LifeTime = 3.0f;
    private GameObject Elemental_B_Skill_Effect;

    public override void Set_Skill()
    {
        gameObject.GetComponent<Speech_Character>().Init();
        base.Set_Skill();

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            Elemental_B_Skill_Effect = Instantiate(Resources.Load<GameObject>("Prefabs/Elemental_B_Skill_Effect"));
            Destroy(Elemental_B_Skill_Effect, LifeTime);
        }
        
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {

        try
        {
            if (Skill_Effect != null)
            {
                Skill_Effect.gameObject.SetActive(true);
            }

            Base_Manager.SOUND.Play(Sound.BGS, "Ele_Black");

            Camera_Manager.instance?.Camera_Shake();

            double skillATK = gameObject.GetComponent<Player>().ATK * SKILL_DAMAGE_MULTIPLE_CONSTATNT;

            var monstersSnapshot = Spawner.m_monsters?.Where(m => m != null).ToList();

            foreach (var monster in monstersSnapshot)
            {
                if (Vector3.Distance(monster.transform.position, Vector3.zero) <= 4.0f)
                {
                    monster.GetDamage(skillATK);
                }
            }

            if (!Utils.is_Skill_Effect_Save_Mode && Elemental_B_Skill_Effect != null)
            {
                Elemental_B_Skill_Effect.transform.position = Vector3.zero;
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

            if (Skill_Effect != null)
            {
                Skill_Effect.gameObject.SetActive(false);
            }

            Debug.Log("[Elemental_B_Skill] ReturnSkill ½ÇÇàµÊ");
            ReturnSkill();
        }
    }
}
