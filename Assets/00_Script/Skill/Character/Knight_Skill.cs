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
        var data = m_Player.ATK_Speed;
        m_Player.ATK_Speed *= 1.5f;
        double temp = Base_Manager.Player.Get_HP(Rarity.Common, Base_Manager.Data.character_Holder[KNIGHT_NAME]);
        gameObject.GetComponent<Player>().HP *= 1.5d;
        Skill_Effect.gameObject.SetActive(true);
        Skill_Effect.gameObject.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(KNIGHT_SKILL_DURATION_TIME);
        m_Player.ATK_Speed = data;
        gameObject.GetComponent<Player>().HP = temp;
        Skill_Effect.gameObject.SetActive(false);
        ReturnSkill();
    }
}
