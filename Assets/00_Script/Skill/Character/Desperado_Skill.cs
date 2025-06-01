using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Desperado_Skill : Skill_Base
{
    [SerializeField]
    private GameObject Player_Effect;

    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT = 4.0f;
    private float LifeTime = 3.0f;
    private GameObject Desperado_Skill_Effect;
    public override void Set_Skill()
    {
        gameObject.GetComponent<Speech_Character>().Init();
        base.Set_Skill();

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            Desperado_Skill_Effect = Instantiate(Resources.Load<GameObject>("Prefabs/Desperado_Skill_Effect"));
            Destroy(Desperado_Skill_Effect, LifeTime);
        }
        
        
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {
        try
        {
            if (Player_Effect != null)
            {
                Player_Effect.gameObject.SetActive(true);

                var ps = Player_Effect.GetComponent<ParticleSystem>();
                if (ps != null) ps.Play();

                Player_Effect.transform.position = transform.position;
            }

            double skillATK = gameObject.GetComponent<Player>().ATK * SKILL_DAMAGE_MULTIPLE_CONSTATNT;
            var monstersSnapshot = Spawner.m_monsters?.Where(m => m != null).ToList();

            foreach (var monster in monstersSnapshot)
            {
                if (Vector3.Distance(monster.transform.position, Vector3.zero) <= 4.0f)
                {
                    monster.GetDamage(skillATK);
                }
            }

            Base_Manager.SOUND.Play(Sound.BGS, "Desperado_1");
            Base_Manager.SOUND.Play(Sound.BGS, "Desperado_2");

            if (!Utils.is_Skill_Effect_Save_Mode && Desperado_Skill_Effect != null)
            {
                Desperado_Skill_Effect.transform.position = Vector3.zero;
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

            if (Player_Effect != null)
            {
                Player_Effect.gameObject.SetActive(false);
            }

            Debug.Log("[Desperado_Skill] ReturnSkill ½ÇÇàµÊ");
            ReturnSkill();
        }
    }
}

