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
    private TextMeshProUGUI GaCha_ReSummon_Text; // 소환결과창에서, 재 소환을 할때 버튼에 1회소환인지, 11회소환인지 수정합니다.
    [SerializeField]
    private TextMeshProUGUI GaCha_ReSummon_Price; //소환 양에따른 소환결과창 재소환버튼 다이아 양을 수정합니다.
    [SerializeField]
    private Button ReGacha_Button;

    private int Hero_Amount_Value_Count;
    private List<GameObject> Reset_Gacha_Hero_Card_List = new List<GameObject>();


    private const int GACHA_RESUMMON_PRICE_11 = 3000;
    public override bool Init()
    {
        return base.Init();
    }

    public void Initialize()
    {
        for(int i = 0; i<Reset_Gacha_Hero_Card_List.Count; i++)
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
                GaCha_ReSummon_Text.text = "11회 소환";
                GaCha_ReSummon_Price.text = GACHA_RESUMMON_PRICE_11.ToString();
                ReGacha_Button.onClick.AddListener(() => OnClick_ReGaCha(Hero_Amount_Value));
                break;
            case 1:
                GaCha_ReSummon_Text.text = "1회 소환";
                GaCha_ReSummon_Price.text = (GACHA_RESUMMON_PRICE_11 / 10).ToString();
                ReGacha_Button.onClick.AddListener(() => OnClick_ReGaCha(Hero_Amount_Value));
                break;
        }
        StartCoroutine(GaCha_Coroutine(Hero_Amount_Value));
    }

    public void OnClick_ReGaCha(int value)
    {
        Initialize();
        Get_Gacha_Hero(value);
    }

    IEnumerator GaCha_Coroutine(int Hero_Amount_Value)
    {
        for (int i = 0; i < Hero_Amount_Value; i++)
        {
            Rarity rarity = Rarity.Common;
            float R_Percentage = 0.0f;
            float Percentage = Random.Range(0.0f, 100.0f);
            var go = Instantiate(Gacha_Hero_Parts, Content);
            Reset_Gacha_Hero_Card_List.Add(go.gameObject);
            go.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.15f);

            for(int j = 0; j < 5; j++)
            {
                R_Percentage += Utils.Gacha_Percentage[j];
                if (Percentage <= R_Percentage)
                {
                    rarity = (Rarity)j;                  
                    break;
                }
            }

            Debug.Log(rarity);
            Character_Scriptable Ch_Scriptable_Data = Base_Manager.Data.Get_Rarity_Character(rarity);
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

        }
    }

}
