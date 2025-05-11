using System.Collections;
using System.Collections.Generic;
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
        Light_Wizard_Skill_Effect = Instantiate(Resources.Load<GameObject>("Prefabs/Light_Wizard_Skill_Effect"));
        Destroy(Light_Wizard_Skill_Effect, LifeTime);
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {
      
        
        Base_Manager.SOUND.Play(Sound.BGS, "Light_Wizard");
        var localMonsters = (monsters != null) ? (Monster[])monsters.Clone() : null;

        if (localMonsters == null || localMonsters.Length == 0)
        {
            Debug.LogError("Monsters array is null or empty!");
            ReturnSkill();
            yield break;
        }

        var Damage_Multiple = Random.Range(SKILL_DAMAGE_MULTIPLE_CONSTATNT_MIN, SKILL_DAMAGE_MULTIPLE_CONSTATNT_MAX);

        yield return new WaitForSecondsRealtime(2.0f);
        for (int i = 0; i < Spawner.m_monsters.Count; i++)
        {
            if (Vector3.Distance(Spawner.m_monsters[i].transform.position, Light_Wizard_Skill_Effect.transform.position) <= 4.0f)
            {
                Spawner.m_monsters[i].GetDamage(gameObject.GetComponent<Player>().ATK * Damage_Multiple);
            }
        }

        Light_Wizard_Skill_Effect.transform.position = Vector3.zero;

        yield return new WaitForSecondsRealtime(2.0f);
        this.gameObject.GetComponent<Player>().Use_Skill = false;
        

        ReturnSkill();
    }
}
