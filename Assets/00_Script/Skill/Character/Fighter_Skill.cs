using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Fighter_Skill : Skill_Base
{
    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT = 1.2f;

    private float LifeTime = 3.0f;
    private GameObject Fighter_Skill_Effect;
    public override void Set_Skill()
    {
        
        gameObject.GetComponent<Speech_Character>().Init();
        base.Set_Skill();
        Fighter_Skill_Effect = Instantiate(Resources.Load<GameObject>("Prefabs/Fighter_Skill_Effect"));
        Destroy(Fighter_Skill_Effect, LifeTime);
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {
      
        Base_Manager.SOUND.Play(Sound.BGS, "Fighter");
        var localMonsters = (monsters != null) ? (Monster[])monsters.Clone() : null;

        if (localMonsters == null || localMonsters.Length == 0)
        {
            Debug.LogError("Monsters array is null or empty!");
            ReturnSkill();
            yield break;
        }

        for (int i = 0; i < Spawner.m_monsters.Count; i++)
        {
            if (Vector3.Distance(Spawner.m_monsters[i].transform.position, Fighter_Skill_Effect.transform.position) <= 4.0f)
            {
                Spawner.m_monsters[i].GetDamage(gameObject.GetComponent<Player>().ATK * SKILL_DAMAGE_MULTIPLE_CONSTATNT);
            }
        }

        Fighter_Skill_Effect.transform.position = Vector3.zero;
   
        yield return new WaitForSecondsRealtime(2.0f);
        this.gameObject.GetComponent<Player>().Use_Skill = false;
    
        ReturnSkill();
    }
}
