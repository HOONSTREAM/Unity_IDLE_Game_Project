using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Upgrade_Parts : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Now_Level_Text;
    [SerializeField]
    private TextMeshProUGUI Upgrade_Level;
    [SerializeField]
    private Image Hero_Icon, Rarity_Image;
    [SerializeField]
    private Animator anim;

    public void Init(Character_Holder holder,int Now_Level, int Level)
    {
        Now_Level_Text.text = Now_Level.ToString();
        Upgrade_Level.text = Level.ToString();
        Hero_Icon.sprite = Utils.Get_Atlas(holder.Data.name);        
    }

   
}
