using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Reward : UI_Base
{
    public Image ItemImage;
    public TextMeshProUGUI CountText;

    public void GetIAPReward(IAP_Holder iapName)
    {
        switch (iapName)
        {
            case IAP_Holder.remove_ads: GetRewardInit("PACKAGE_ADS", 1); break;
            case IAP_Holder.package_1: GetRewardInit("PACKAGE_TODAY", 1); break;
            case IAP_Holder.package_2: GetRewardInit("PACKAGE_STRONG", 1); break;
            case IAP_Holder.steel_1000: GetRewardInit("Steel", 1000); break;
            case IAP_Holder.dia_19000: GetRewardInit("Dia", 19000); break;
            case IAP_Holder.dia_1400: GetRewardInit("Dia", 1400); break;
            case IAP_Holder.dia_68000: GetRewardInit("Dia", 68000);break;
            case IAP_Holder.dia_4900: GetRewardInit("Dia", 4900); break;
            case IAP_Holder.dungeon_dia_20: GetRewardInit("Dungeon_Dia", 5); break;
            case IAP_Holder.gold_30: GetRewardInit("Dungeon_Gold", 8); break;
        }
    }

    public void GetRewardInit(string ItemName, int Count)
    {
        Base_Manager.SOUND.Play(Sound.BGS, "OFFLINE");
        ItemImage.sprite = Utils.Get_Atlas(ItemName);
        CountText.text = Count <= 1 ? "" : "x" + Count.ToString();

        switch (ItemName)
        {
            case "Dia": Data_Manager.Main_Players_Data.DiaMond += Count;
                Base_Manager.BACKEND.Log_Get_Dia($"{ItemName}Dia_{Count}");
                break;

            case "PACKAGE_ADS": 
                Data_Manager.Main_Players_Data.isBuyADPackage = true;
                Data_Manager.Main_Players_Data.DiaMond += 3000;
                Base_Manager.BACKEND.Log_Get_Dia("Get_Package_ADS");
                Base_Manager.Data.Item_Holder["SWORD"].Hero_Card_Amount += 15;
                Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount += 1000;                              

                break;

            case "PACKAGE_STRONG":
                Data_Manager.Main_Players_Data.isBuySTRONGPackage = true;
                Data_Manager.Main_Players_Data.DiaMond += 2500;
                Base_Manager.BACKEND.Log_Get_Dia("Get_Package_Strong");
                Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount += 600;
                Base_Manager.Data.Item_Holder["Hondon_Potion"].Hero_Card_Amount += 2000;
                break;

            case "PACKAGE_TODAY":
                Data_Manager.Main_Players_Data.isBuyTodayPackage = true;
                Data_Manager.Main_Players_Data.DiaMond += 8000;
                Base_Manager.BACKEND.Log_Get_Dia("Get_Package_Today");
                Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount += 600;
                Data_Manager.Main_Players_Data.User_Key_Assets[0] += 2;
                Data_Manager.Main_Players_Data.User_Key_Assets[1] += 2;

                break;

            case "Steel": Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount += Count; break;

            case "Dungeon_Dia":
                Data_Manager.Main_Players_Data.User_Key_Assets[0] += Count;
                break;
            case "Dungeon_Gold":
                Data_Manager.Main_Players_Data.User_Key_Assets[1] += Count;
                break;
        }

    }
}
