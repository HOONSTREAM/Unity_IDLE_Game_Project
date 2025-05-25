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
        Base_Manager.SOUND.Play(Sound.BGS, "Hammer_Knight");
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
            Hammer_Knight_Skill_Effect.transform.position = Vector3.zero;
        }
        

        yield return new WaitForSecondsRealtime(2.0f);

        this.gameObject.GetComponent<Player>().Use_Skill = false;


        ReturnSkill();
    }
}
