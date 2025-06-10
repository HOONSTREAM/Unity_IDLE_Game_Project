using AssetKits.ParticleImage;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_Status_Parts : MonoBehaviour
{
    [SerializeField]
    private Image Rarity_Image, Item_Icon;
    [SerializeField]
    private TextMeshProUGUI Enhancement_Text;
   

    public void Init(string name, Status_Item_Holder holder)
    {        
        Status_Item_Scriptable scriptable = Base_Manager.Data.Status_Item_Dictionary[name];
        Rarity_Image.sprite = Utils.Get_Atlas(scriptable.rarity.ToString());
        Item_Icon.sprite = Utils.Get_Atlas(scriptable.name);
        Enhancement_Text.text = $"+ {holder.Enhancement.ToString()}";

        GetComponent<Status_ToolTip_Controller>().Init(scriptable);
    }
  
}
