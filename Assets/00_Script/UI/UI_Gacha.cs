using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Gacha : UI_Base
{

    [SerializeField]
    private Image Gacha_Hero_Parts;
    public Transform Content;
    [SerializeField]
    private GameObject Rare_Particle;
    [SerializeField]
    private TextMeshProUGUI GaCha_ReSummon_Text; // ��ȯ���â����, �� ��ȯ�� �Ҷ� ��ư�� 1ȸ��ȯ����, 11ȸ��ȯ���� �����մϴ�.
    [SerializeField]
    private TextMeshProUGUI GaCha_ReSummon_Price; //��ȯ �翡���� ��ȯ���â ���ȯ��ư ���̾� ���� �����մϴ�.
    [SerializeField]
    private Button ReGacha_Button;
    [SerializeField]
    private GameObject Blocking_Close_Button;
    [SerializeField]
    private GameObject Blocking_ReGaCha_Button;


    private int Hero_Amount_Value_Count;
    private List<GameObject> Reset_Gacha_Hero_Card_List = new List<GameObject>();

    public static Action Pressed_Tutorial_Gacha_Close_Button;

    private const int GACHA_RESUMMON_PRICE_11 = 500;
    private const int GACHA_RESUMMON_PRICE_55 = 2500;

    public override bool Init()
    {
        return base.Init();
    }

    /// <summary>
    /// ���ȯ�� �ǽ��� ��, ĳ���� ī�� ������Ʈ�� ���� �����ϰ�, ���ġ�մϴ�.
    /// </summary>
    public void ReGaCha_Initialize()
    {
   
        for (int i = 0; i<Reset_Gacha_Hero_Card_List.Count; i++)
        {
            Destroy(Reset_Gacha_Hero_Card_List[i]);
        }

        Reset_Gacha_Hero_Card_List.Clear();
    }

    public void Get_Gacha_Hero(int Hero_Amount_Value, bool ADS = false)
    {
        Hero_Amount_Value_Count = Hero_Amount_Value;

        ReGacha_Button.onClick.RemoveAllListeners();

        switch (Hero_Amount_Value)
        {
            case 11:
                GaCha_ReSummon_Text.text = "11ȸ ��ȯ";
                GaCha_ReSummon_Price.text = GACHA_RESUMMON_PRICE_11.ToString();
                if (ADS == false)
                {
                    Data_Manager.Main_Players_Data.DiaMond -= GACHA_RESUMMON_PRICE_11;
                    Base_Manager.BACKEND.Log_Get_Dia("Gacha_11_Hero");
                }
                ReGacha_Button.onClick.AddListener(() => OnClick_ReGaCha(Hero_Amount_Value));
                break;
            case 55:
                GaCha_ReSummon_Text.text = "55ȸ ��ȯ";
                GaCha_ReSummon_Price.text = (GACHA_RESUMMON_PRICE_55).ToString();
                Data_Manager.Main_Players_Data.DiaMond -= (GACHA_RESUMMON_PRICE_55);
                Base_Manager.BACKEND.Log_Get_Dia("Gacha_55_Hero");
                ReGacha_Button.onClick.AddListener(() => OnClick_ReGaCha(Hero_Amount_Value));
                break;
        }
        StartCoroutine(GaCha_Coroutine(Hero_Amount_Value));
    }

    public void OnClick_ReGaCha(int value)
    {
        switch (value)
        {
            case 11:
                if(Data_Manager.Main_Players_Data.DiaMond < GACHA_RESUMMON_PRICE_11)
                {
                    Base_Canvas.instance.Get_Toast_Popup().Initialize("���̾Ƹ�尡 �����մϴ�.");
                    return;
                }
                break;
            case 55:
                if (Data_Manager.Main_Players_Data.DiaMond < (GACHA_RESUMMON_PRICE_55))
                {
                    Base_Canvas.instance.Get_Toast_Popup().Initialize("���̾Ƹ�尡 �����մϴ�.");
                    return;
                }
                break;
        }
        ReGaCha_Initialize();
        Get_Gacha_Hero(value);     
    }

    IEnumerator GaCha_Coroutine(int Hero_Amount_Value)
    {
        Blocking_Close_Button.gameObject.SetActive(true);
        Blocking_ReGaCha_Button.gameObject.SetActive(true);

        

        for (int i = 0; i < Hero_Amount_Value; i++)
        {
            
            Data_Manager.Main_Players_Data.Hero_Summon_Count++;
            Data_Manager.Main_Players_Data.Summon++; //��������Ʈ ���� ��� (������ȯ)
            Data_Manager.Main_Players_Data.Hero_Pickup_Count++;
            Rarity rarity = Rarity.Common;

            if (Data_Manager.Main_Players_Data.Hero_Pickup_Count >= 110)
            {
                Data_Manager.Main_Players_Data.Hero_Pickup_Count = 0;
                rarity = Rarity.Legendary;
            }

            
            float R_Percentage = 0.0f;
            float Percentage = UnityEngine.Random.Range(0.0f, 100.0f);
            var go = Instantiate(Gacha_Hero_Parts, Content); // ĳ���� ī�带 �����մϴ�.
            Reset_Gacha_Hero_Card_List.Add(go.gameObject);
            go.gameObject.SetActive(true);

            yield return new WaitForSecondsRealtime(0.02f);
            
            if (rarity != Rarity.Legendary)
            {
                for (int j = 0; j < 6; j++)
                {
                    R_Percentage += Utils.Gacha_Percentage()[j];
                    if (Percentage <= R_Percentage)
                    {
                        rarity = (Rarity)j;
                        break;
                    }
                }
            }
            

            Character_Scriptable Ch_Scriptable_Data = Base_Manager.Data.Get_Rarity_Character(rarity); // ��ȯ �Ϸ�� ĳ������ ������ ���� �Ϸ�         
            Base_Manager.Data.character_Holder[Ch_Scriptable_Data.name].Hero_Card_Amount++; // ī�� ���� ����

            var tooltip = go.GetComponent<Hero_ToolTip_Controller>();
            if (tooltip != null)
            {
                tooltip.Init(Ch_Scriptable_Data);
            }

            go.sprite = Utils.Get_Atlas(rarity.ToString());
            go.transform.GetChild(1).GetComponent<Image>().sprite = Utils.Get_Atlas(Ch_Scriptable_Data.name);

            if ((int)rarity >= (int)Rarity.Epic)
            {
                go.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                go.transform.GetChild(0).gameObject.SetActive(false);
            }

            
            

            ScrollRect scrollRect = Content.GetComponentInParent<ScrollRect>();
            if (scrollRect != null)
            {
                Canvas.ForceUpdateCanvases(); // ���̾ƿ� ���� ������Ʈ
                scrollRect.verticalNormalizedPosition = 0.0f; // ���� �Ʒ��� ��ũ��
                                
            }


        }


        
        StartCoroutine(Block_Button_Coroutine());
    }

    /// <summary>
    /// ��ȯ �ڷ�ƾ ������ WriteData ȣ����, ���� ���� �ʷ��ϹǷ� ��ư ��� �� 1ȸ ����
    /// </summary>
    /// <returns></returns>
    IEnumerator Block_Button_Coroutine()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        _ = Base_Manager.BACKEND.WriteData();
        Blocking_Close_Button.gameObject.SetActive(false);
        Blocking_ReGaCha_Button.gameObject.SetActive(false);
        GameObject.Find("Shop").gameObject.GetComponent<UI_Shop>().Init();
    }

    public override void DisableOBJ()
    {
        Pressed_Tutorial_Gacha_Close_Button?.Invoke();
        base.DisableOBJ();
    }

}
