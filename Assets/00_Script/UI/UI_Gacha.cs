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


    private const int GACHA_RESUMMON_PRICE_11 = 500;
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

    public void Get_Gacha_Hero(int Hero_Amount_Value)
    {
        Hero_Amount_Value_Count = Hero_Amount_Value;

        ReGacha_Button.onClick.RemoveAllListeners();

        switch (Hero_Amount_Value)
        {
            case 11:
                GaCha_ReSummon_Text.text = "11ȸ ��ȯ";
                GaCha_ReSummon_Price.text = GACHA_RESUMMON_PRICE_11.ToString();
                ReGacha_Button.onClick.AddListener(() => OnClick_ReGaCha(Hero_Amount_Value));
                break;
            case 1:
                GaCha_ReSummon_Text.text = "1ȸ ��ȯ";
                GaCha_ReSummon_Price.text = (GACHA_RESUMMON_PRICE_11 / 10).ToString();
                ReGacha_Button.onClick.AddListener(() => OnClick_ReGaCha(Hero_Amount_Value));
                break;
        }
        StartCoroutine(GaCha_Coroutine(Hero_Amount_Value));
    }

    public void OnClick_ReGaCha(int value)
    {
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
            Data_Manager.Main_Players_Data.Hero_Pickup_Count++;
            Rarity rarity = Rarity.Common;

            if (Data_Manager.Main_Players_Data.Hero_Pickup_Count >= 110)
            {
                Data_Manager.Main_Players_Data.Hero_Pickup_Count = 0;
                rarity = Rarity.Legendary;
            }

            
            float R_Percentage = 0.0f;
            float Percentage = Random.Range(0.0f, 100.0f);
            var go = Instantiate(Gacha_Hero_Parts, Content); // ĳ���� ī�带 �����մϴ�.
            Reset_Gacha_Hero_Card_List.Add(go.gameObject);
            go.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(0.15f);

            if(rarity != Rarity.Legendary)
            {
                for (int j = 0; j < 5; j++)
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




            GameObject.Find("@BackEnd_Manager").gameObject.GetComponent<BackEnd_Manager>().WriteData();

        }

        StartCoroutine(Block_Button_Coroutine());
    }

    IEnumerator Block_Button_Coroutine()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        Blocking_Close_Button.gameObject.SetActive(false);
        Blocking_ReGaCha_Button.gameObject.SetActive(false);
    }

}
