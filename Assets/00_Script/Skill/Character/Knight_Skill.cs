using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight_Skill : Skill_Base
{
    [SerializeField]
    private const float KNIGHT_SKILL_DURATION_TIME = 10.0f;
    private const string KNIGHT_NAME = "Knight";

    public override void Set_Skill()
    {
        base.Set_Skill();
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {
        float originalAtkSpeed = m_Player.ATK_Speed;
        double originalHP = Base_Manager.Player.Get_HP(Rarity.Common, Base_Manager.Data.character_Holder[KNIGHT_NAME], KNIGHT_NAME);

        try
        {
            m_Player.ATK_Speed *= 1.5f;

            var player = gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.HP *= 1.5d;
            }

            if (!Utils.is_Skill_Effect_Save_Mode && Skill_Effect != null)
            {
                Skill_Effect.gameObject.SetActive(true);

                var ps = Skill_Effect.GetComponent<ParticleSystem>();
                if (ps != null) ps.Play();
            }

            yield return new WaitForSeconds(KNIGHT_SKILL_DURATION_TIME);
        }
        finally
        {
            m_Player.ATK_Speed = originalAtkSpeed;

            var player = gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.HP = originalHP;
            }

            if (Skill_Effect != null)
            {
                Skill_Effect.gameObject.SetActive(false);
            }

            Debug.Log("[Knight_Skill] ATK_Speed 및 HP 복구, ReturnSkill 실행됨");
            ReturnSkill();
        }
    }
}
