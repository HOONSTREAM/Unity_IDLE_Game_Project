using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Smelt : UI_Base
{
    private Smelt_Scriptable smelt_Data;
    public GameObject Smelt_Panel;
    public Transform Vertical_Content;
    private bool Opening = false;
    public TextMeshProUGUI CountText;

    [HideInInspector] 
    public List<GameObject> Garbage = new List<GameObject>();

    public override bool Init()
    {
        smelt_Data = Resources.Load<Smelt_Scriptable>("Scriptable/Smelt_Data");

        for (int i = 0; i < Base_Manager.Data.User_Main_Data_Smelt_Array.Count; i++)
        {
            if (Base_Manager.Data.User_Main_Data_Smelt_Array.Count > 0)
            {
                Get_Smelt_Panel(
                (int)Base_Manager.Data.User_Main_Data_Smelt_Array[i].rarity,
                Base_Manager.Data.User_Main_Data_Smelt_Array[i].smelt_holder,
                Base_Manager.Data.User_Main_Data_Smelt_Array[i].smelt_value
                );
            }
          
        }

        Smelt_Request_Item_Count_Check();

        return base.Init();
    }
    public void Smelt_Change()
    {
        if (Opening) return;

        if (!Utils.Item_Count("Steel", 300))
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("각인에 필요한 재료가 부족합니다.");
            return;
        }

        Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount -= 300;
        Base_Manager.BACKEND.Log_Try_Smelt();
        Smelt_Request_Item_Count_Check();
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
        Base_Manager.Data.User_Main_Data_Smelt_Array.Clear();

        for (int i = 0; i < count; i++)
        {
            Smelt_Status status = (Smelt_Status)Random.Range(0, 7); // 어떤 종류의 부가능력치를 부여할지 결정합니다.
            int value = Calculate_Rarity_Level(); // 특정된 종류의 부가능력치의 등급을 결정합니다.
          
            float valueCount = Random.Range(StatusHolder(status)[value].Min, StatusHolder(status)[value].Max); // 결정된 부가능력치 등급의 최솟값과 최댓값의 랜덤값을 산출합니다.

            Base_Manager.Data.User_Main_Data_Smelt_Array.Add(new Smelt_Holder { rarity = (Rarity)value, smelt_holder = status, smelt_value = valueCount });

            Get_Smelt_Panel(value, status, valueCount);
            
            yield return new WaitForSeconds(0.1f);
        }

        for (int i = 0; i < Spawner.m_players.Count; i++)
        {
            Spawner.m_players[i].Set_ATK_HP_Sub_Hero();
        }

        Opening = false;

        Base_Canvas.instance.Get_Toast_Popup().Initialize("각인이 완료되었습니다.");
        Base_Manager.SOUND.Play(Sound.BGS, "Gacha");
        _ =Base_Manager.BACKEND.WriteData();
    }
    private void Smelt_Request_Item_Count_Check()
    {
        CountText.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount, 300);
        CountText.color = Utils.Item_Count("Steel", 300) ? Color.green : Color.red;
    }
    private void Get_Smelt_Panel(int rarityValue, Smelt_Status status, float valueCount)
    {
        var go = Instantiate(Smelt_Panel, Vertical_Content);
        Garbage.Add(go);
        go.SetActive(true);
        go.transform.GetChild(1).gameObject.SetActive(false);
        go.transform.GetChild(2).gameObject.SetActive(false);
        go.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Utils.String_Color_Rarity((Rarity)rarityValue) + StatusString(status);
        go.transform.GetChild(2).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Utils.String_Color_Rarity((Rarity)rarityValue) + string.Format("{0:0.00}%", valueCount);
        go.GetComponent<Animator>().SetTrigger("Open");
        StartCoroutine(Smelt_Delay_Coroutine());
        go.transform.GetChild(1).gameObject.SetActive(true);
        go.transform.GetChild(2).gameObject.SetActive(true);     
    }

    private IEnumerator Smelt_Delay_Coroutine()
    {
        yield return new WaitForSecondsRealtime(1.5f);
    }

    /// <summary>
    /// 몇개의 각인을 시킬 것인지 확률적으로 계산합니다.
    /// </summary>
    /// <returns></returns>
    private int Calculate_Rarity_Percentage()
    {
        int RandomCount = Random.Range(0, 100);

        float RandomValue = 0.0f;

        int count = -1;

        for (int i = 0; i < smelt_Data.Smelt_Count_Value.Length; i++)
        {
            RandomValue += smelt_Data.Smelt_Count_Value[i];

            if (RandomValue >= RandomCount)
            {
                count = i + 1;
                break;
            }         
        }

        return count >= 0 ? count : (int)smelt_Data.Smelt_Count_Value.Length;
    }

    /// <summary>
    /// 각인의 등급을 결정합니다.
    /// </summary>
    /// <returns></returns>
    private int Calculate_Rarity_Level()
    {
        int RandomCount = Random.Range(0, 100);

        float RandomValue = 0.0f;

        int count = -1;

        for (int i = 0; i < smelt_Data.Smelt_Count_Value.Length; i++)
        {
            RandomValue += smelt_Data.Smelt_Count_Value[i];

            if (RandomValue >= RandomCount)
            {
                count = i + 1;
                break;
            }
        }

        return count >= 0 && count < 5 ? count : ( (int)smelt_Data.Smelt_Count_Value.Length - 1 );
    }

    private string StatusString(Smelt_Status holder)
    {
        switch (holder)
        {
            case Smelt_Status.ATK: return "공격력 증가";
            case Smelt_Status.HP: return "HP 증가";
            case Smelt_Status.MONEY: return "골드 획득량 증가"; 
            case Smelt_Status.ITEM: return "아이템 드랍률 증가"; 
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
