using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_Inventory_Parts : MonoBehaviour
{
    [SerializeField]
    private Image Rarity_Image, IconImage;
    [SerializeField]
    private TextMeshProUGUI Count_Text;


    public void Init(string name, Holder holder)
    {        
        Item_Scriptable scriptable = Base_Manager.Data.Data_Item_Dictionary[name];
        Rarity_Image.sprite = Utils.Get_Atlas(scriptable.rarity.ToString());
        IconImage.sprite = Utils.Get_Atlas(scriptable.name);
        Count_Text.text = holder.Hero_Card_Amount.ToString();


        GetComponent<ToolTip_Controller>().Init(scriptable);
    }

}
