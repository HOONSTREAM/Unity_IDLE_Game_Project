using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Druid_Skill : Skill_Base
{
    [SerializeField]
    private const float Druid_SKILL_DURATION_TIME = 15.0f;
    private const string DRUID_NAME = "Druid";
    public override void Set_Skill()
    {
        gameObject.GetComponent<Speech_Character>().Init();
        base.Set_Skill();
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {
        double originalATK = Base_Manager.Player.Get_ATK(Rarity.UnCommon, Base_Manager.Data.character_Holder[DRUID_NAME], DRUID_NAME);

        try
        {
            if (!Utils.is_Skill_Effect_Save_Mode && Skill_Effect != null)
            {
                Skill_Effect.gameObject.SetActive(true);
                Base_Manager.SOUND.Play(Sound.BGS, "PalaDin");
            }

            var player = gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.ATK *= 4.0d;
            }

            yield return new WaitForSeconds(Druid_SKILL_DURATION_TIME);
        }
        finally
        {
            if (Skill_Effect != null)
            {
                Skill_Effect.gameObject.SetActive(false);
            }

            var player = gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.ATK = originalATK;
                player.Use_Skill = false;
            }

            Debug.Log("[Druid_Skill] ATK 복구 및 ReturnSkill 실행됨");
            ReturnSkill();
        }
    }
}
