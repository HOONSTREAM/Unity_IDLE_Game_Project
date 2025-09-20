using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter_Skill : Skill_Base
{
    [SerializeField]
    private const float HUNTER_SKILL_DURATION_TIME = 15.0f;
    private bool isSkillActive = false;

    public override void Set_Skill()
    {
        if (isSkillActive) return; // �̹� �ߵ� ���̸� ����
        base.Set_Skill();
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {
        isSkillActive = true;

        var originalAtkSpeed = m_Player.ATK_Speed;

        try
        {
            m_Player.ATK_Speed *= 2.0f;

            if (!Utils.is_Skill_Effect_Save_Mode && Skill_Effect != null)
            {
                Skill_Effect.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(HUNTER_SKILL_DURATION_TIME);
        }
        finally
        {
            m_Player.ATK_Speed = originalAtkSpeed;

            if (Skill_Effect != null)
            {
                Skill_Effect.gameObject.SetActive(false);
            }

            Debug.Log("[Hunter_Skill] ATK_Speed ���� �� ReturnSkill �����");
            ReturnSkill();
            isSkillActive = false;
        }
    }
}
