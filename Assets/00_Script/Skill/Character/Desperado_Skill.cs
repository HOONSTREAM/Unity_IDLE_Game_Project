using System.Collections;
using System.Collections.Generic;
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
      
        
        Player_Effect.gameObject.SetActive(true);
        Player_Effect.gameObject.GetComponent<ParticleSystem>().Play();
        Player_Effect.gameObject.transform.position = this.transform.position;
        
        var localMonsters = (monsters != null) ? (Monster[])monsters.Clone() : null;

        if (localMonsters == null || localMonsters.Length == 0)
        {
            Debug.LogError("Monsters array is null or empty!");
            ReturnSkill();
            yield break;
        }
        for (int i = 0; i < Spawner.m_monsters.Count; i++)
        {
            if (Vector3.Distance(Spawner.m_monsters[i].transform.position, Vector3.zero) <= 4.0f)
            {
                Spawner.m_monsters[i].GetDamage(gameObject.GetComponent<Player>().ATK * SKILL_DAMAGE_MULTIPLE_CONSTATNT);
            }
        }

        Base_Manager.SOUND.Play(Sound.BGS, "Desperado_1");        
        Base_Manager.SOUND.Play(Sound.BGS, "Desperado_2");

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            Desperado_Skill_Effect.transform.position = Vector3.zero;
        }
        
 
        yield return new WaitForSecondsRealtime(2.0f);
        this.gameObject.GetComponent<Player>().Use_Skill = false;      
        Player_Effect.gameObject.SetActive(false);
   
        ReturnSkill();
    }
}
