using OpenCover.Framework.Model;
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

        int Value = Skill_Effect.transform.childCount;

        for(int i = 0; i< Value; i++)
        {
            var Meteor = Skill_Effect.transform.GetChild(0).GetComponent<Meteor>();
            Meteor.gameObject.SetActive(true);


            Vector3 Attack_pos = monsters[Random.Range(0, monsters.Length)].transform.position + 
                new Vector3(Random.insideUnitSphere.x * 3.0f, 0.0f , Random.insideUnitSphere.z * 3.0f);

            Meteor.transform.position = Attack_pos;
            Meteor.Init(Skill_Damage(150));
            yield return new WaitForSeconds(0.2f);
        }

        ReturnSkill();

    }
}
