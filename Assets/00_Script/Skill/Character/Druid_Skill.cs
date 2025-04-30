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
        Skill_Effect.gameObject.SetActive(true);
        Base_Manager.SOUND.Play(Sound.BGS, "PalaDin");
        double temp = Base_Manager.Player.Get_ATK(Rarity.UnCommon, Base_Manager.Data.character_Holder[DRUID_NAME], DRUID_NAME);
        gameObject.GetComponent<Player>().ATK *= 4.0d;
        yield return new WaitForSeconds(Druid_SKILL_DURATION_TIME);       
        Skill_Effect.gameObject.SetActive(false);
        gameObject.GetComponent<Player>().ATK = temp;
        ReturnSkill();
    }
}
