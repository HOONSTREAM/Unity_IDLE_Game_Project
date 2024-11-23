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


    public void Init(Item item)
    {
        Rarity_Image.sprite = Utils.Get_Atlas(item.data.rarity.ToString());
        IconImage.sprite = Utils.Get_Atlas(item.data.name);
        Count_Text.text = item.Count.ToString();


        GetComponent<ToolTip_Controller>().Init(item.data);
    }

}
