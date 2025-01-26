using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Smelt : UI_Base
{
    Smelt_Scriptable smelt_Data;
    public GameObject Smelt_Panel;
    public Transform Vertical_Content;
    bool Opening = false;
    public TextMeshProUGUI CountText;
    [HideInInspector] public List<GameObject> Garbage = new List<GameObject>();

    public override bool Init()
    {
        smelt_Data = Resources.Load<Smelt_Scriptable>("Scriptable/Smelt_Data");

        if (Base_Manager.Data.User_Main_Data_Smelt.Count > 0)
        {
            for (int i = 0; i < Base_Manager.Data.User_Main_Data_Smelt.Count; i++)
            {
                Get_Smelt_Panel(
                      (int)Base_Manager.Data.User_Main_Data_Smelt[i].rarity,
                      Base_Manager.Data.User_Main_Data_Smelt[i].smelt_holder,
                      Base_Manager.Data.User_Main_Data_Smelt[i].smelt_value
                      );
            }
        }

        //Smelt_Request_Item_Count_Check();

        return base.Init();
    }
    public void Smelt_Change()
    {
        if (Opening) return;
        if (!Utils.Item_Count("Steel", 100))
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("각인에 필요한 재료가 부족합니다.");
            return;
        }

        Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount -= 100;
        //Smelt_Request_Item_Count_Check();
        Opening = true;

        if (Garbage.Count > 0)
        {
            for (int i = 0; i < Garbage.Count; i++) Destroy(Garbage[i]);
            Garbage.Clear();
        }

        StartCoroutine(OpenCoroutine((Calculate_Rarity_Percentage())));
    }
    IEnumerator OpenCoroutine(int count)
    {
        Base_Manager.Data.User_Main_Data_Smelt.Clear();

        for (int i = 0; i < count; i++)
        {
            Smelt_Status status = (Smelt_Status)Random.Range(0, 8); // 어떤 종류의 부가능력치를 부여할지 결정합니다.
            int value = Calculate_Rarity_Percentage(); // 특정된 종류의 부가능력치의 등급을 결정합니다.
            float valueCount = Random.Range(StatusHolder(status)[value].Min, StatusHolder(status)[value].Max); // 결정된 부가능력치 등급의 최솟값과 최댓값의 랜덤값을 산출합니다.

            Base_Manager.Data.User_Main_Data_Smelt.Add(new Smelt_Holder { rarity = (Rarity)value, smelt_holder = status, smelt_value = valueCount });

            Get_Smelt_Panel(value, status, valueCount);
            
            yield return new WaitForSeconds(0.1f);
        }

        for (int i = 0; i < Spawner.m_players.Count; i++)
        {
            Spawner.m_players[i].Set_ATK_HP_Sub_Hero();
        }

        Opening = false;
    }
    private void Smelt_Request_Item_Count_Check()
    {
        CountText.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount, 100);
        CountText.color = Utils.Item_Count("Steel", 100) ? Color.green : Color.red;
    }
    private void Get_Smelt_Panel(int rarityValue, Smelt_Status status, float valueCount)
    {
        var go = Instantiate(Smelt_Panel, Vertical_Content);
        Garbage.Add(go);
        go.SetActive(true);
        go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Utils.String_Color_Rarity((Rarity)rarityValue) + StatusString(status);
        go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Utils.String_Color_Rarity((Rarity)rarityValue) + string.Format("{0:0.00}%", valueCount);
        go.GetComponent<Animator>().SetTrigger("Open");
    }
    private int Calculate_Rarity_Percentage()
    {
        int RandomCount = Random.Range(0, 100);

        float RandomValue = 0.0f;

        int count = 0;

        for (int i = 0; i < smelt_Data.Smelt_Count_Value.Length; i++)
        {
            RandomValue += smelt_Data.Smelt_Count_Value[i];

            if (RandomValue >= RandomCount)
            {
                count = i + 1;
                return count;
            }
        }

        return -1;
    }
    private string StatusString(Smelt_Status holder)
    {
        switch (holder)
        {
            case Smelt_Status.ATK: return "공격력 증가";
            case Smelt_Status.HP: return "HP 증가";
            case Smelt_Status.MONEY: return "골드 드랍률 증가";
            case Smelt_Status.ITEM: return "아이템 드랍률 증가";
            case Smelt_Status.SKILL_COOL: return "스킬 쿨타임 감소";
            case Smelt_Status.ATK_SPEED: return "공격 속도 증가";
            case Smelt_Status.CRITICAL_PERCENTAGE: return "치명타 확률 증가";
            case Smelt_Status.CRITICAL_DAMAGE: return "치명타 데미지 증가";
        }
        return "";
    }

    private Percentage_Smelt[] StatusHolder(Smelt_Status holder)
    {
        switch (holder)
        {
            case Smelt_Status.ATK: return smelt_Data.ATK_percentage;
            case Smelt_Status.HP: return smelt_Data.HP_percentage;
            case Smelt_Status.MONEY: return smelt_Data.MONEY_percentage;
            case Smelt_Status.ITEM: return smelt_Data.ITEM_percentage;
            case Smelt_Status.SKILL_COOL: return smelt_Data.SKILL_COOL_percntage;
            case Smelt_Status.ATK_SPEED: return smelt_Data.ATK_SPEED_percentage;
            case Smelt_Status.CRITICAL_PERCENTAGE: return smelt_Data.CRITICAL_PER_percentage;
            case Smelt_Status.CRITICAL_DAMAGE: return smelt_Data.CRITICAL_DMG_percentage;
        }
        return null;
    }

    public override void DisableOBJ()
    {
        Main_UI.Instance.Layer_Check(-1);
        base.DisableOBJ();
    }
}
