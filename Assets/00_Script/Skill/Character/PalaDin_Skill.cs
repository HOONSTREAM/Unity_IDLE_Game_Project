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
        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            Skill_Effect.gameObject.SetActive(true);
        }
        
        Base_Manager.SOUND.Play(Sound.BGS, PALADIN_NAME);
        double temp = Base_Manager.Player.Get_HP(Rarity.Common, Base_Manager.Data.character_Holder[PALADIN_NAME]);
        gameObject.GetComponent<Player>().HP *= 2.0d;
        yield return new WaitForSeconds(PalaDin_SKILL_DURATION_TIME);       
        Skill_Effect.gameObject.SetActive(false);
        gameObject.GetComponent<Player>().HP = temp;

        ReturnSkill();
    }
}
