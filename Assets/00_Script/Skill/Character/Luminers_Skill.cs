using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Luminers_Skill : Skill_Base
{
    [SerializeField]
    private const float LUMINERS_SKILL_DURATION_TIME = 6.0f;
    private const string LUMINERS_NAME = "Luminers";
    private List<double> originalAtkList = new List<double>();
    private List<double> originalHpList = new List<double>();
    private float LifeTime = 6.0f;
    private GameObject Luminers_Skill_Effect;
    
    public override void Set_Skill()
    {

        gameObject.GetComponent<Speech_Character>().Init();
        base.Set_Skill();

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            Luminers_Skill_Effect = Instantiate(Resources.Load<GameObject>("Prefabs/Luminers_Skill_Effect"));
            Luminers_Skill_Effect.transform.position = Vector3.zero;
            Destroy(Luminers_Skill_Effect, LifeTime);
        }
       
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {        
        
        foreach(var players in players)
        {
            originalAtkList.Add(players.ATK);
            originalHpList.Add(players.HP);

            players.ATK *= 2.0f;
            players.HP *= 2.0f;
        }
    
        yield return new WaitForSeconds(LUMINERS_SKILL_DURATION_TIME);
             
        for (int i = 0; i < players.Length; i++)
        {
            players[i].ATK = originalAtkList[i];
            players[i].HP = originalHpList[i];
        }

        ReturnSkill();
    }
}
