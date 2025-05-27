using System.Collections;
using System.Collections.Generic;
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
              
        Base_Manager.SOUND.Play(Sound.BGS, "DarkHero");
        var localMonsters = (monsters != null) ? (Monster[])monsters.Clone() : null;

        if (localMonsters == null || localMonsters.Length == 0)
        {
            Debug.LogError("Monsters array is null or empty!");
            ReturnSkill();
            yield break;
        }


        var Damage_Multiple = Random.Range(SKILL_DAMAGE_MULTIPLE_CONSTATNT_MIN, SKILL_DAMAGE_MULTIPLE_CONSTATNT_MAX);

        for (int i = 0; i < Spawner.m_monsters.Count; i++)
        {
            if (Vector3.Distance(Spawner.m_monsters[i].transform.position, Vector3.zero) <= 4.0f)
            {
                Spawner.m_monsters[i].GetDamage(gameObject.GetComponent<Player>().ATK * Damage_Multiple);
            }
        }

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            DarkHero_Skill_Effect.transform.position = new Vector3(0.0f, 15.0f, 0.0f);
        }
       

        yield return new WaitForSecondsRealtime(2.0f);
        this.gameObject.GetComponent<Player>().Use_Skill = false;
        

        ReturnSkill();
    }
}
