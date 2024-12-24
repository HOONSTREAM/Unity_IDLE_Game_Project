using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class UI_Relic : UI_Base
{
    public Transform Content;
    public GameObject Parts;
    public List<UI_Relic_Parts> relic_parts = new List<UI_Relic_Parts>();
    private Dictionary<string, Item_Scriptable> _dict = new Dictionary<string, Item_Scriptable>(); // 유물장비 저장 딕셔너리
    private Item_Scriptable Item;
    private const int RELIC_SLOT_NUMBER = 9;
    public GameObject[] Relic_Panel_Objects;
   
    public override bool Init()
    { 
        var Data = Base_Manager.Data.Data_Item_Dictionary; //모든 아이템 딕셔너리

        GetItemcheck();

        foreach (var data in Data)
        {
            if(data.Value.ItemType == ItemType.Equipment)
            {
                _dict.Add(data.Value.name, data.Value);
            }          
        }


        var sort_dict = _dict.OrderByDescending(x => x.Value.rarity);


        int value = 0;


        foreach (var data in sort_dict)
        {
            var go = Instantiate(Parts, Content).GetComponent<UI_Relic_Parts>();
            value++;
            relic_parts.Add(go);
            int index = value;
            go.Init(data.Value, this);
        }

        for (int i = 0; i< Relic_Panel_Objects.Length; i++)
        {
            if(i == 0)
            {
                Relic_Panel_Objects[i].transform.GetChild(0).gameObject.SetActive(false);
                Relic_Panel_Objects[i].transform.GetChild(1).gameObject.SetActive(false);
                Relic_Panel_Objects[i].transform.GetChild(2).gameObject.SetActive(false);

                break;
            }

            Relic_Panel_Objects[i].transform.GetChild(0).gameObject.SetActive(true);
            Relic_Panel_Objects[i].transform.GetChild(1).gameObject.SetActive(false);
            Relic_Panel_Objects[i].transform.GetChild(2).gameObject.SetActive(false);
        }

        return base.Init();
    }

    public void Initialize()
    {
        Set_Click(null);   

        for (int i = 0; i < relic_parts.Count; i++)
        {
            relic_parts[i].Get_Item_Check();
        }

        GetItemcheck();
        //Main_UI.Instance.Set_Character_Data();
    }

    public void Set_Item_Button(int value)
    {
        Debug.Log("Set_Item_Button 실행");
        Base_Manager.Item.Get_Item(value, Item.name);
        Initialize();
    }

    /// <summary>
    /// 장착 유물을 검사하여, 스프라이트 및 컬러를 수정합니다.
    /// </summary>
    public void GetItemcheck()
    {
        for(int i = 0; i<Relic_Panel_Objects.Length; i++)
        {
            if (Base_Manager.Data.Main_Set_Item[i] != null)
            {
                Debug.Log("getitemcheck if 구문 진입");
               
                Relic_Panel_Objects[i].transform.GetChild(0).gameObject.SetActive(false);
                Relic_Panel_Objects[i].transform.GetChild(1).gameObject.SetActive(true);
                Relic_Panel_Objects[i].transform.GetChild(2).gameObject.SetActive(true);
                Relic_Panel_Objects[i].transform.GetChild(2).GetComponent<Image>().sprite = Utils.Get_Atlas(Base_Manager.Data.Main_Set_Item[i].name);
                Relic_Panel_Objects[i].transform.GetChild(1).GetComponent<Image>().sprite = Utils.Get_Atlas(Base_Manager.Data.Main_Set_Item[i].rarity.ToString());
            }

            else
            {
                Debug.Log("getitemcheck else 구문 진입");
                Relic_Panel_Objects[i].transform.GetChild(1).gameObject.SetActive(false);
                Relic_Panel_Objects[i].transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }

    public void Set_Click(UI_Relic_Parts parts)
    {

        if (parts == null)
        {
            for (int i = 0; i < relic_parts.Count; i++)
            {
                relic_parts[i].Lock_OBJ.SetActive(false);
                relic_parts[i].GetComponent<Outline>().enabled = false;
            }
        }

        else
        {
            for (int i = 0; i < Base_Manager.Data.Main_Set_Item.Length; i++)
            {
                var Data = Base_Manager.Data.Main_Set_Item[i];
                if (Data != null)
                {
                    if (Data == parts.item)
                    {
                        Base_Manager.Item.Disable_Item(i);
                        Initialize();
                        return;
                    }
                }
            }

            Item = parts.item;

            for (int i = 0; i < relic_parts.Count; i++)
            {
                relic_parts[i].Lock_OBJ.SetActive(true);
                relic_parts[i].GetComponent<Outline>().enabled = false;
            }

            parts.Lock_OBJ.SetActive(false);
            parts.GetComponent<Outline>().enabled = true;

        }

    }


    public override void DisableOBJ()
    {
        Main_UI.Instance.Layer_Check(-1);
        base.DisableOBJ();
    }

}
