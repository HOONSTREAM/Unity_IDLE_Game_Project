using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class temp_Skill : Skill_Base
{
    [SerializeField]
    private const float PalaDin_SKILL_DURATION_TIME = 10.0f;
    private const string PALADIN_NAME = "PalaDin";
    List<double> originalAtkList = new List<double>();
    public override void Set_Skill()
    {
        gameObject.GetComponent<Speech_Character>().Init();
        base.Set_Skill();
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {        
        Skill_Effect.gameObject.SetActive(true);
        Base_Manager.SOUND.Play(Sound.BGS, PALADIN_NAME);
        double temp = Base_Manager.Player.Get_HP(Rarity.Common, Base_Manager.Data.character_Holder[PALADIN_NAME]);



        foreach(var players in players)
        {
            originalAtkList.Add(players.ATK);
            players.ATK *= 100.0f;
        }

        gameObject.GetComponent<Player>().HP *= 2.0d;
        yield return new WaitForSeconds(PalaDin_SKILL_DURATION_TIME);       
        Skill_Effect.gameObject.SetActive(false);
        gameObject.GetComponent<Player>().HP = temp;

        for (int i = 0; i < players.Length; i++)
        {
            players[i].ATK = originalAtkList[i];
        }

        ReturnSkill();
    }
}
