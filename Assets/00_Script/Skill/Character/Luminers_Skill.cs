using BackndChat.Message;
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

    private static bool isSkillApplied = false;

    public override void Set_Skill()
    {        
        if (isSkillApplied) return; // 이미 적용 중이면 중복 실행 막기
        isSkillApplied = true;

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
        var cachedPlayers = players
           ?.Where(p => p != null)
           .GroupBy(p => p.GetInstanceID())
           .Select(g => g.First())
           .ToArray();

        if (cachedPlayers == null || cachedPlayers.Length == 0)
        {
            Debug.LogWarning("[Luminers_Skill] 유효한 플레이어가 없습니다.");
            isSkillApplied = false;
            ReturnSkill();
            yield break;
        }

        try
        {
            foreach (var p in cachedPlayers)
            {
                originalAtkList.Add(p.ATK);
                originalHpList.Add(p.HP);

                p.ATK *= 1.45f;
                p.HP *= 1.45f;
            }

            yield return new WaitForSeconds(LUMINERS_SKILL_DURATION_TIME);
        }
        finally
        {
            for (int i = 0; i < cachedPlayers.Length; i++)
            {
                if (cachedPlayers[i] != null)
                {
                    cachedPlayers[i].ATK = originalAtkList[i];
                    cachedPlayers[i].HP = originalHpList[i];
                }
            }

            isSkillApplied = false;

            Debug.Log("[Luminers_Skill] 버프 해제 및 ReturnSkill 호출");
            ReturnSkill();

            originalAtkList.Clear();
            originalHpList.Clear();
        }
    }
}
