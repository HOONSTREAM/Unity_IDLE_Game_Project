using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Reward : UI_Base
{
    public Image ItemImage;
    public TextMeshProUGUI CountText;

    //public void GetIAPReward(IAP_Holder iapName)
    //{
    //    switch (iapName)
    //    {
    //        case IAP_Holder.removeads: GetRewardInit("ADS", 0); break;
    //        case IAP_Holder.dia300: GetRewardInit("Dia", 300); break;
    //    }
    //}

    public void GetRewardInit(string ItemName, int Count)
    {
        ItemImage.sprite = Utils.Get_Atlas(ItemName);
        CountText.text = Count <= 1 ? "" : "x" + Count.ToString();

        switch (ItemName)
        {
            case "Dia": Data_Manager.Main_Players_Data.DiaMond += Count; break;
            case "ADS": Data_Manager.Main_Players_Data.isBuyADPackage = true; break;
        }
    }
}
