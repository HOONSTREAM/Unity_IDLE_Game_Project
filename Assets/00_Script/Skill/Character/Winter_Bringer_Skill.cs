using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Winter_Bringer_Skill : Skill_Base
{
    private const float SKILL_DAMAGE_MIN = 1.5f;
    private const float SKILL_DAMAGE_MAX = 4.0f;

    public override void Set_Skill()
    {
        gameObject.GetComponent<Speech_Character>().Init();
        base.Set_Skill();
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {
      
        
        Base_Manager.SOUND.Play(Sound.BGS, "Winter_Bringer");
        var localMonsters = (monsters != null) ? (Monster[])monsters.Clone() : null;

        if (localMonsters == null || localMonsters.Length == 0)
        {
            Debug.LogError("Monsters array is null or empty!");
            ReturnSkill();
            yield break;
        }

        Camera_Manager.instance.Camera_Shake();


        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Winter_Bringer_Skill_Effect"));
        go.transform.position = Vector3.zero;

        yield return new WaitForSecondsRealtime(0.8f);

        var Damage_Multiple = Random.Range(SKILL_DAMAGE_MIN, SKILL_DAMAGE_MAX);

        for (int i = 0; i < Spawner.m_monsters.Count; i++)
        {
            if (Vector3.Distance(Spawner.m_monsters[i].transform.position, go.transform.position) <= 4.0f)
            {
                Spawner.m_monsters[i].GetDamage(gameObject.GetComponent<Player>().ATK * Damage_Multiple);
            }
        }


        yield return new WaitForSecondsRealtime(2.0f);
        this.gameObject.GetComponent<Player>().Use_Skill = false;
        

        Destroy(go, 2.0f);

        ReturnSkill();
    }
}
