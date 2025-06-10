using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PalaDin_Skill : Skill_Base
{
    [SerializeField]
    private const float PalaDin_SKILL_DURATION_TIME = 10.0f;
    private const string PALADIN_NAME = "PalaDin";
    
    public override void Set_Skill()
    {
        gameObject.GetComponent<Speech_Character>().Init();
        base.Set_Skill();
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {
        double originalHP = Base_Manager.Player.Get_HP(Rarity.Common, Base_Manager.Data.character_Holder[PALADIN_NAME], PALADIN_NAME);

        try
        {
            if (!Utils.is_Skill_Effect_Save_Mode && Skill_Effect != null)
            {
                Skill_Effect.gameObject.SetActive(true);
            }

            Base_Manager.SOUND.Play(Sound.BGS, PALADIN_NAME);

            var player = gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.HP *= 2.0d;
            }

            yield return new WaitForSeconds(PalaDin_SKILL_DURATION_TIME);
        }
        finally
        {
            var player = gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.HP = originalHP;
            }

            if (Skill_Effect != null)
            {
                Skill_Effect.gameObject.SetActive(false);
            }

            Debug.Log("[PalaDin_Skill] HP 복구 및 ReturnSkill 실행됨");
            ReturnSkill();
        }
    }
}
