using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Elemental_W_Skill : Skill_Base
{

   

    public override void Set_Skill()
    {

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

        int Value = Skill_Effect.transform.childCount;

        for (int i = 0; i < Value; i++)
        {
            if (localMonsters.Length == 0) // 로컬 배열이 비어 있으면 종료
            {
                Debug.LogWarning("No monsters left to target.");
                break;
            }

            var Meteor = Skill_Effect.transform.GetChild(0).GetComponent<Meteor>();
            Meteor.gameObject.SetActive(true);
            Camera_Manager.instance.Camera_Shake();
            Vector3 Attack_pos = localMonsters[Random.Range(0, localMonsters.Length)].transform.position +
                new Vector3(Random.insideUnitSphere.x * 3.0f, 0.0f, Random.insideUnitSphere.z * 3.0f);

            Meteor.transform.position = Attack_pos;
            Meteor.Init(Skill_Damage(200));
            yield return new WaitForSeconds(0.1f);
            
        }

        yield return new WaitForSecondsRealtime(5.0f);
        this.gameObject.GetComponent<Player>().Use_Skill = false;
        ReturnSkill();
      
    }
}

