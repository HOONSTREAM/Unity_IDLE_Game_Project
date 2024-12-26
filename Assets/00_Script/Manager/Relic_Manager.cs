using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// SHIELD : 몬스터 사망 시 이벤트
// SWORD : 근접 공격 이벤트
// MANA :  피격 시 이벤트

public class Relic_Manager : MonoBehaviour
{
    public static Relic_Manager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Initalize()
    {
        if (Base_Manager.Item.Set_Item_Check("DICE")) Delegate_Holder.Monster_Dead_Event += DICE;
        if (Base_Manager.Item.Set_Item_Check("SWORD")) Delegate_Holder.Player_attack_Event += SWORD;
        if (Base_Manager.Item.Set_Item_Check("MANA")) Delegate_Holder.player_hit_Event += MANA;
    }

    /// <summary>
    /// 근접공격 강화
    /// </summary>
    /// <param name="player"></param>
    /// <param name="monster"></param>
    public void SWORD(Player player, Monster monster)
    {
        Debug.Log("SWORD 실행됨");
        if (!RandomCount(30))
        {
            return;
        }

        Vector3 RealPos = monster.transform.position;
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Burst"));
        go.transform.position = RealPos;

        for (int i = 0; i < Spawner.m_monsters.Count; i++)
        {
            if (Vector3.Distance(Spawner.m_monsters[i].transform.position, RealPos) <= 3.0f)
            {
                Spawner.m_monsters[i].GetDamage(player.ATK * 1.7f);
            }
        }
    }

    /// <summary>
    /// 마나 충전
    /// </summary>
    /// <param name="player"></param>
    public void MANA(Player player)
    {
        if (!RandomCount(50))
        {
            return;
        }
        player.Get_MP(2);
    }

    /// <summary>
    /// 주사위 골드 획득 , TODO : 카드 레벨별로 확률과 획득골드 수정 필요
    /// </summary>
    /// <param name="monster"></param>
    public void DICE(Monster monster)
    {
        Debug.Log("DICE 실행됨");

        if (!RandomCount(50))
        {
            return;
        }
        Vector3 RealPos = monster.transform.position;
        GameObject go = Instantiate(Resources.Load<GameObject>("PreFabs/Dice"));
        go.transform.position = RealPos;
    }

    private bool RandomCount(float RandomValue)
    {
        float randomCount = Random.Range(0.0f, 100.0f);
        if (randomCount <= RandomValue)
        {
            return true;
        }
        return false;
    }

}
