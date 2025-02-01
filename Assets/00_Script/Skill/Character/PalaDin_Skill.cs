using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalaDin_Skill : Skill_Base
{
    [SerializeField]
    private const float PalaDin_SKILL_DURATION_TIME = 10.0f;

    public override void Set_Skill()
    {
        gameObject.GetComponent<Speech_Character>().Init();
        base.Set_Skill();
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {        
        Skill_Effect.gameObject.SetActive(true);
        gameObject.GetComponent<Player>().HP *= 2.0d;
        yield return new WaitForSeconds(PalaDin_SKILL_DURATION_TIME);       
        Skill_Effect.gameObject.SetActive(false);
        gameObject.GetComponent<Player>().HP *= 1.0d;
        ReturnSkill();
    }
}
