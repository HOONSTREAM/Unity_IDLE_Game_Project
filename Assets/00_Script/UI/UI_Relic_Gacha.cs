using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Relic_Gacha : UI_Base
{
    [SerializeField]
    private Image Gacha_Relic_Parts;
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


    private int Relic_Amount_Value_Count;
    private List<GameObject> Reset_Gacha_Relic_Card_List = new List<GameObject>();


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

        for (int i = 0; i < Reset_Gacha_Relic_Card_List.Count; i++)
        {
            Destroy(Reset_Gacha_Relic_Card_List[i]);
        }

        Reset_Gacha_Relic_Card_List.Clear();
    }

    public void Get_Gacha_Relic(int Relic_Amount_Value, bool ADS = false)
    {
        Relic_Amount_Value_Count = Relic_Amount_Value;

        ReGacha_Button.onClick.RemoveAllListeners();

        switch (Relic_Amount_Value)
        {
            case 11:
                GaCha_ReSummon_Text.text = "11ȸ ��ȯ";
                GaCha_ReSummon_Price.text = GACHA_RESUMMON_PRICE_11.ToString();
                if(ADS == false)
                {
                    Data_Manager.Main_Players_Data.DiaMond -= GACHA_RESUMMON_PRICE_11;
                    Base_Manager.BACKEND.Log_Get_Dia("Gacha_Relic_11");
                }            
                ReGacha_Button.onClick.AddListener(() => OnClick_ReGaCha(Relic_Amount_Value));
                break;
            case 55:
                GaCha_ReSummon_Text.text = "55ȸ ��ȯ";
                GaCha_ReSummon_Price.text = (GACHA_RESUMMON_PRICE_55).ToString();
                Data_Manager.Main_Players_Data.DiaMond -= (GACHA_RESUMMON_PRICE_55);
                Base_Manager.BACKEND.Log_Get_Dia("Gacha_Relic_55");
                ReGacha_Button.onClick.AddListener(() => OnClick_ReGaCha(Relic_Amount_Value));
                break;
        }
        StartCoroutine(GaCha_Coroutine(Relic_Amount_Value));
    }

    public void OnClick_ReGaCha(int value)
    {
        switch (value)
        {
            case 11:
                if (Data_Manager.Main_Players_Data.DiaMond < GACHA_RESUMMON_PRICE_11)
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
        Get_Gacha_Relic(value);

    }

    IEnumerator GaCha_Coroutine(int Relic_Amount_Value)
    {
        Blocking_Close_Button.gameObject.SetActive(true);
        Blocking_ReGaCha_Button.gameObject.SetActive(true);

        

        for (int i = 0; i < Relic_Amount_Value; i++)
        {
            Data_Manager.Main_Players_Data.Relic_Summon_Count++;
            Data_Manager.Main_Players_Data.Relic++; //��������Ʈ ���� ��� (������ȯ)
            Data_Manager.Main_Players_Data.Relic_Pickup_Count++;

            Rarity rarity = Rarity.Common;

            if (Data_Manager.Main_Players_Data.Relic_Pickup_Count >= 110)
            {
                Data_Manager.Main_Players_Data.Relic_Pickup_Count = 0;
                rarity = Rarity.Legendary;
            }


            float R_Percentage = 0.0f;
            float Percentage = Random.Range(0.0f, 100.0f);
            var go = Instantiate(Gacha_Relic_Parts, Content); // ĳ���� ī�带 �����մϴ�.
            Reset_Gacha_Relic_Card_List.Add(go.gameObject);
            go.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(0.02f);

            if (rarity != Rarity.Legendary)
            {
                for (int j = 0; j < 5; j++)
                {
                    R_Percentage += Utils.Gacha_Percentage_Relic()[j];
                    if (Percentage <= R_Percentage)
                    {
                        rarity = (Rarity)j;
                        break;
                    }
                }
            }


            Item_Scriptable item_scriptable_Data = Base_Manager.Data.Get_Rarity_Relic(rarity); // ��ȯ �Ϸ�� ĳ������ ������ ���� �Ϸ�         
            Base_Manager.Data.Item_Holder[item_scriptable_Data.name].Hero_Card_Amount++; // ī�� ���� ����

            var tooltip = go.GetComponent<Relic_ToolTip_Controller>();
            if (tooltip != null)
            {
                tooltip.Init(item_scriptable_Data);
            }

            //Base_Manager.BACKEND.Log_RelicSummon(item_scriptable_Data, Relic_Amount_Value);

            go.sprite = Utils.Get_Atlas(rarity.ToString());
            go.transform.GetChild(1).GetComponent<Image>().sprite = Utils.Get_Atlas(item_scriptable_Data.name);

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
    /// ������ ���� ��ȯ�Ǳ� ���� ���ȯ�� ��Ű��, ������ �� ȹ��Ǵ� ���װ� �߻��ϹǷ�
    /// ��ư���� ��Ų �Ŀ�, 1�� �ڿ� ���������ְ�, ȹ���� ������ �����մϴ�.
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
}
