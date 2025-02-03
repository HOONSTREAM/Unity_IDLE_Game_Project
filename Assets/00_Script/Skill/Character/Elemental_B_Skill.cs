using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Elemental_B_Skill : Skill_Base
{
    private const float SKILL_DAMAGE_MULTIPLE_CONSTATNT = 1.2f;

    public override void Set_Skill()
    {
        gameObject.GetComponent<Speech_Character>().Init();
        base.Set_Skill();
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {
      
        Skill_Effect.gameObject.SetActive(true);

        var localMonsters = (monsters != null) ? (Monster[])monsters.Clone() : null;

        if (localMonsters == null || localMonsters.Length == 0)
        {
            Debug.LogError("Monsters array is null or empty!");
            ReturnSkill();
            yield break;
        }

        Camera_Manager.instance.Camera_Shake();


        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Elemental_B_Skill_Effect"));
        go.transform.position = Vector3.zero;

        yield return new WaitForSecondsRealtime(0.8f);

        for (int i = 0; i < Spawner.m_monsters.Count; i++)
        {
            if (Vector3.Distance(Spawner.m_monsters[i].transform.position, go.transform.position) <= 4.0f)
            {
                Spawner.m_monsters[i].GetDamage(gameObject.GetComponent<Player>().ATK * SKILL_DAMAGE_MULTIPLE_CONSTATNT);
            }
        }


        yield return new WaitForSecondsRealtime(2.0f);
        this.gameObject.GetComponent<Player>().Use_Skill = false;
        Skill_Effect.gameObject.SetActive(false);

        ReturnSkill();
    }
}
