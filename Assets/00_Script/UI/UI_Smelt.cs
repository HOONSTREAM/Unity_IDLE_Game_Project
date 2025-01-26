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
            Base_Canvas.instance.Get_Toast_Popup().Initialize("���ο� �ʿ��� ��ᰡ �����մϴ�.");
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
            Smelt_Status status = (Smelt_Status)Random.Range(0, 8); // � ������ �ΰ��ɷ�ġ�� �ο����� �����մϴ�.
            int value = Calculate_Rarity_Percentage(); // Ư���� ������ �ΰ��ɷ�ġ�� ����� �����մϴ�.
            float valueCount = Random.Range(StatusHolder(status)[value].Min, StatusHolder(status)[value].Max); // ������ �ΰ��ɷ�ġ ����� �ּڰ��� �ִ��� �������� �����մϴ�.

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
            case Smelt_Status.ATK: return "���ݷ� ����";
            case Smelt_Status.HP: return "HP ����";
            case Smelt_Status.MONEY: return "��� ����� ����";
            case Smelt_Status.ITEM: return "������ ����� ����";
            case Smelt_Status.SKILL_COOL: return "��ų ��Ÿ�� ����";
            case Smelt_Status.ATK_SPEED: return "���� �ӵ� ����";
            case Smelt_Status.CRITICAL_PERCENTAGE: return "ġ��Ÿ Ȯ�� ����";
            case Smelt_Status.CRITICAL_DAMAGE: return "ġ��Ÿ ������ ����";
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
