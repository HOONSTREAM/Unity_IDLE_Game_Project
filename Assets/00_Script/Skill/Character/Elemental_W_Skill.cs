using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Elemental_W_Skill : Skill_Base
{

    [SerializeField]
    private GameObject Magic_Circle;


    public override void Set_Skill()
    {

        base.Set_Skill();
        StartCoroutine(Set_Skill_Coroutine());
        
    }

    IEnumerator Set_Skill_Coroutine()
    {

        Skill_Effect.gameObject.SetActive(true);
        bool skillEnded = false;

        try
        {
            var localMonsters = (monsters != null)
                ? System.Array.FindAll(monsters, m => m != null && m.gameObject != null && m.gameObject.activeInHierarchy)
                : null;

            if (localMonsters == null || localMonsters.Length == 0)
            {
                Debug.LogError("Monsters array is null or empty!");
                yield break;
            }

            int Value = Skill_Effect.transform.childCount;

            for (int i = 0; i < Value; i++)
            {
                if (localMonsters.Length == 0)
                {
                    Debug.LogWarning("No monsters left to target.");
                    break;
                }

                var Meteor = Skill_Effect.transform.GetChild(0).GetComponent<Meteor>();
                Base_Manager.SOUND.Play(Sound.BGS, "Meteor");
                Magic_Circle.SetActive(true);
                Magic_Circle.GetComponent<ParticleSystem>().Play();

                Meteor.gameObject.SetActive(true);
                Camera_Manager.instance.Camera_Shake();

                Monster target = localMonsters[Random.Range(0, localMonsters.Length)];
                if (target == null || target.gameObject == null || !target.gameObject.activeInHierarchy)
                {
                    Debug.LogWarning("Target monster is null or inactive, skipping.");
                    continue;
                }

                Vector3 Attack_pos = target.transform.position + new Vector3(Random.insideUnitSphere.x * 3.0f, 0.0f, Random.insideUnitSphere.z * 3.0f);
                Meteor.transform.position = Attack_pos;
                Meteor.Init(Skill_Damage(400));
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSecondsRealtime(5.0f);
            skillEnded = true;
        }
        finally
        {
            // 반드시 실행됨
            this.GetComponent<Player>().Use_Skill = false;
            Magic_Circle.SetActive(false);
            ReturnSkill();

            Debug.Log($"[Elemental_W_Skill] Skill finished. Use_Skill set to false. Normal end: {skillEnded}");
        }
    }
}

