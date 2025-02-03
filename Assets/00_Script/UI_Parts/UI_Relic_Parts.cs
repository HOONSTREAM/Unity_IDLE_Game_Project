using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Relic_Parts : MonoBehaviour
{
    [SerializeField]
    private Image M_Silder, Relic_Image, M_Rarity_Image;
    [SerializeField]
    private TextMeshProUGUI M_Level, M_Count;
    [SerializeField]
    private GameObject Equip_Relic_Image;
    [SerializeField]
    private Button OnClickButton;
    [SerializeField]
    private GameObject Plus_Button, Minus_Button;


    public GameObject Lock_OBJ;
    public Item_Scriptable item;
    private UI_Relic parent;


    public void LockCheck(bool Lock)
    {
        switch (Lock)
        {
            case true:
                Lock_OBJ.SetActive(true);
                Plus_Button.SetActive(false);
                Minus_Button.SetActive(true);
                break;
            case false:
                Lock_OBJ.SetActive(false);
                Plus_Button.SetActive(true);
                Minus_Button.SetActive(false);
                break;

        }
    }

    public void Init(Item_Scriptable data, UI_Relic parentsBASE)
    {
        parent = parentsBASE;
        item = data;

        M_Level.text = "LV." + (Base_Manager.Data.Item_Holder[data.name].Hero_Level + 1).ToString();
        M_Silder.fillAmount = (float)Base_Manager.Data.Item_Holder[data.name].Hero_Card_Amount / Utils.Data.heroCardData.Get_LEVELUP_Relic_Card_Amount(data.name);
        M_Count.text = Base_Manager.Data.Item_Holder[data.name].Hero_Card_Amount.ToString() + "/" + Utils.Data.heroCardData.Get_LEVELUP_Relic_Card_Amount(data.name);

        M_Rarity_Image.sprite = Utils.Get_Atlas(data.rarity.ToString());
        Relic_Image.sprite = Utils.Get_Atlas(data.name);
        
         

        Get_Item_Check();
    }

    /// <summary>
    /// 캐릭터 강화가 이루어 진 뒤에, 바로 영웅창에서 적용 할 수있도록 합니다.
    /// </summary>
    public void Initialize()
    {

        M_Silder.fillAmount = (float)Base_Manager.Data.Item_Holder[item.name].Hero_Card_Amount / Utils.Data.heroCardData.Get_LEVELUP_Relic_Card_Amount(item.name);
        M_Count.text = Base_Manager.Data.Item_Holder[item.name].Hero_Card_Amount.ToString() + "/" + Utils.Data.heroCardData.Get_LEVELUP_Relic_Card_Amount(item.name);
        M_Level.text = "LV." + (Base_Manager.Data.Item_Holder[item.name].Hero_Level + 1).ToString();
    }

    public void Get_Item_Check()
    {
        bool Equip = false;

        for (int i = 0; i < Base_Manager.Data.Main_Set_Item.Length; i++)
        {
            if (Base_Manager.Data.Main_Set_Item[i] != null)
            {
                if (Base_Manager.Data.Main_Set_Item[i] == item)
                {
                    Equip = true;
                }
            }

        }

        Equip_Relic_Image.gameObject.SetActive(Equip);

    }

    /// <summary>
    /// 보유중인 유물을 터치했을 때의 기능을 구현합니다.
    /// </summary>
    public void Click_My_Relic()
    {
        parent.Get_Relic_Information(item, this);
    }


}
