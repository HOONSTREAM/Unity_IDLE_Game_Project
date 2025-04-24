using Google.Protobuf.WellKnownTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// SHIELD : ���� ��� �� �̺�Ʈ
// SWORD : ���� ���� �̺�Ʈ
// MANA :  �ǰ� �� �̺�Ʈ

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
        if (Base_Manager.Item.Set_Item_Check("DICE")) Delegate_Holder.Monster_Dead_Event -= DICE;
        if (Base_Manager.Item.Set_Item_Check("SWORD")) Delegate_Holder.Player_attack_Event -= SWORD;
        if (Base_Manager.Item.Set_Item_Check("MANA")) Delegate_Holder.player_hit_Event -= MANA;
        if (Base_Manager.Item.Set_Item_Check("DICE")) Delegate_Holder.Monster_Dead_Event += DICE;
        if (Base_Manager.Item.Set_Item_Check("SWORD")) Delegate_Holder.Player_attack_Event += SWORD;
        if (Base_Manager.Item.Set_Item_Check("MANA")) Delegate_Holder.player_hit_Event += MANA;
    }

    /// <summary>
    /// �������� ��ȭ
    /// </summary>
    /// <param name="player"></param>
    /// <param name="monster"></param>
    public void SWORD(Player player, Monster monster)
    {
       
        string value = "SWORD";
     
        if (!RandomCount(float.Parse(CSV_Importer.RELIC_SWORD_Design[Base_Manager.Data.Item_Holder[value].Hero_Level]["start_percent"].ToString())))
        {
            return;
        }

        Vector3 RealPos = monster.transform.position;
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Burst"));
        go.transform.position = RealPos;

        var effect_value = float.Parse(CSV_Importer.RELIC_SWORD_Design[Base_Manager.Data.Item_Holder[value].Hero_Level]["effect_percent"].ToString());

        for (int i = 0; i < Spawner.m_monsters.Count; i++)
        {
            if (Vector3.Distance(Spawner.m_monsters[i].transform.position, RealPos) <= 3.0f)
            {
                Spawner.m_monsters[i].GetDamage(player.ATK * effect_value);
            }
        }

        Destroy(go , 2.0f);
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="player"></param>
    public void MANA(Player player)
    {
        string value = "MANA";
        float percent = float.Parse(CSV_Importer.RELIC_MANA_Design[Base_Manager.Data.Item_Holder[value].Hero_Level]["start_percent"].ToString());

        if (!RandomCount(percent))
        {
            return;
        }
        Debug.Log("MANA ����");
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/MP"));
        go.transform.position = player.transform.position;
        player.Get_MP(int.Parse(CSV_Importer.RELIC_MANA_Design[Base_Manager.Data.Item_Holder[value].Hero_Level]["effect_percent"].ToString()));
    }

    /// <summary>
    /// �ֻ��� ��� ȹ�� , TODO : ī�� �������� Ȯ���� ȹ���� ���� �ʿ�
    /// </summary>
    /// <param name="monster"></param>
    public void DICE(Monster monster)
    {
        string value = "DICE";

        if (!RandomCount(float.Parse(CSV_Importer.RELIC_DICE_Design[Base_Manager.Data.Item_Holder[value].Hero_Level]["start_percent"].ToString())))
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
