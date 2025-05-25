using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter_Skill : Skill_Base
{
    [SerializeField]
    private const float HUNTER_SKILL_DURATION_TIME = 15.0f;

    public override void Set_Skill()
    {
        base.Set_Skill();
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {
        var data = m_Player.ATK_Speed;
        m_Player.ATK_Speed *= 2.0f;

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            Skill_Effect.gameObject.SetActive(true);
        }
       

        yield return new WaitForSeconds(HUNTER_SKILL_DURATION_TIME);
        m_Player.ATK_Speed = data;
        Skill_Effect.gameObject.SetActive(false);
        ReturnSkill();
    }
}
