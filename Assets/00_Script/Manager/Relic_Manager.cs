using Google.Protobuf.WellKnownTypes;
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

    #region 유물 이펙트 효과
    [SerializeField]
    private GameObject SWORD_Burst_Prefab;
    [SerializeField]
    private GameObject STAFF_Burst_Prefab;
    [SerializeField]
    private GameObject HP_Burst_Prefab;
    [SerializeField]
    private GameObject MANA_Burst_Prefab;
    #endregion

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

    private void Start()
    {
        SWORD_Burst_Prefab = Instantiate(Resources.Load<GameObject>("PreFabs/Burst"));
        SWORD_Burst_Prefab.gameObject.SetActive(false);

        STAFF_Burst_Prefab = Instantiate(Resources.Load<GameObject>("PreFabs/STAFF"));
        STAFF_Burst_Prefab.gameObject.SetActive(false);

        HP_Burst_Prefab = Instantiate(Resources.Load<GameObject>("PreFabs/HP"));
        HP_Burst_Prefab.gameObject.SetActive(false);

        MANA_Burst_Prefab = Instantiate(Resources.Load<GameObject>("PreFabs/MP"));
        MANA_Burst_Prefab.gameObject.SetActive(false);

    }
    public void Initalize()
    {
        if (Base_Manager.Item.Set_Item_Check("DICE")) Delegate_Holder.Monster_Dead_Event -= DICE;
        if (Base_Manager.Item.Set_Item_Check("SWORD")) Delegate_Holder.Player_attack_Event -= SWORD;
        if (Base_Manager.Item.Set_Item_Check("STAFF")) Delegate_Holder.Player_attack_Event -= STAFF;
        if (Base_Manager.Item.Set_Item_Check("MANA")) Delegate_Holder.player_hit_Event -= MANA;
        if (Base_Manager.Item.Set_Item_Check("HP")) Delegate_Holder.player_hit_Event -= HP;        
        if (Base_Manager.Item.Set_Item_Check("DICE")) Delegate_Holder.Monster_Dead_Event += DICE;
        if (Base_Manager.Item.Set_Item_Check("SWORD")) Delegate_Holder.Player_attack_Event += SWORD;
        if (Base_Manager.Item.Set_Item_Check("MANA")) Delegate_Holder.player_hit_Event += MANA;       
        if (Base_Manager.Item.Set_Item_Check("HP")) Delegate_Holder.player_hit_Event += HP;
        if (Base_Manager.Item.Set_Item_Check("STAFF")) Delegate_Holder.Player_attack_Event += STAFF;
    }

    /// <summary>
    /// 근접공격 강화
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

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            GameObject go = SWORD_Burst_Prefab;
            SWORD_Burst_Prefab.gameObject.SetActive(true);
            SWORD_Burst_Prefab.gameObject.GetComponent<ParticleSystem>().Play();
            go.transform.position = Vector3.zero;
            StartCoroutine(DisableAfter(go, 3.0f));
            
        }


        var effect_value = float.Parse(CSV_Importer.RELIC_SWORD_Design[Base_Manager.Data.Item_Holder[value].Hero_Level]["effect_percent"].ToString());

        for (int i = 0; i < Spawner.m_monsters.Count; i++)
        {
            if (Vector3.Distance(Spawner.m_monsters[i].transform.position, RealPos) <= 3.0f)
            {
                Spawner.m_monsters[i].GetDamage(player.ATK * effect_value);
            }
        }
             
    }

    public void STAFF(Player player, Monster monster)
    {

        string value = "STAFF";

        if (!RandomCount(float.Parse(CSV_Importer.RELIC_STAFF_Design[Base_Manager.Data.Item_Holder[value].Hero_Level]["start_percent"].ToString())))
        {
            return;
        }

        Vector3 RealPos = monster.transform.position;

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            GameObject go = STAFF_Burst_Prefab;
            STAFF_Burst_Prefab.gameObject.SetActive(true);
            STAFF_Burst_Prefab.gameObject.GetComponent<ParticleSystem>().Play();
            go.transform.position = Vector3.zero;
            StartCoroutine(DisableAfter(go, 3.0f));
        }
              
        var effect_value = float.Parse(CSV_Importer.RELIC_STAFF_Design[Base_Manager.Data.Item_Holder[value].Hero_Level]["effect_percent"].ToString());

        for (int i = 0; i < Spawner.m_monsters.Count; i++)
        {
            if (Vector3.Distance(Spawner.m_monsters[i].transform.position, RealPos) <= 3.0f)
            {
                Spawner.m_monsters[i].GetDamage(player.ATK * effect_value);
            }
        }

        
    }


    /// <summary>
    /// 마나 충전
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

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            GameObject go = MANA_Burst_Prefab;
            go.transform.position = player.transform.position;
            go.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DisableAfter(go, 3.0f));
        }
        
        player.Get_MP(int.Parse(CSV_Importer.RELIC_MANA_Design[Base_Manager.Data.Item_Holder[value].Hero_Level]["effect_percent"].ToString()));


       
    }

    public void HP(Player player)
    {
        string value = "HP";
        float percent = float.Parse(CSV_Importer.RELIC_HP_Design[Base_Manager.Data.Item_Holder[value].Hero_Level]["start_percent"].ToString());

        if (!RandomCount(percent))
        {
            return;
        }

        if (!Utils.is_Skill_Effect_Save_Mode)
        {
            GameObject go = HP_Burst_Prefab;
            go.transform.position = player.transform.position;
            go.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DisableAfter(go, 3.0f));
        }
        

        float effect = float.Parse(CSV_Importer.RELIC_HP_Design[Base_Manager.Data.Item_Holder[value].Hero_Level]["effect_percent"].ToString());
        
        player.GetComponent<Player>().HP += (double)effect;
        Main_UI.Instance.Main_UI_PlayerInfo_Text_Check();
        


       
    }

    /// <summary>
    /// 주사위 골드 획득
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


        Destroy(go, 2.0f);
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

    IEnumerator DisableAfter(GameObject obj, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        obj.SetActive(false);
    }

}
