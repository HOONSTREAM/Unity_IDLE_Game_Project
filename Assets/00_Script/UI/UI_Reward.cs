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
            case IAP_Holder.start: GetRewardInit("START_Package", 1); break;
            case IAP_Holder.package_3: GetRewardInit("PACKAGE_DIAMOND", 1); break;
            case IAP_Holder.hondon_2000: GetRewardInit("Scroll_Comb", 150000); break;
            case IAP_Holder.dia_gacha: GetRewardInit("Dia", Utils.GetRandomDiamond()); break;
            case IAP_Holder.enhancement: GetRewardInit("Dungeon_Enhancement", 5); break;
            case IAP_Holder.def_enhancement: GetRewardInit("DEF_Enhancement", 1); break;
            case IAP_Holder.dia_pass: GetRewardInit("DIA_PASS", 1); break;
            case IAP_Holder.enhancement_package: GetRewardInit("PACKAGE_ENHANCEMENT", 1); break;
            case IAP_Holder.ticket: GetRewardInit("Research_Item_Ticket", 1); break;
            case IAP_Holder.research_book: GetRewardInit("Research_Levelup_Book", 1); break;
            case IAP_Holder.research_package: GetRewardInit("PACKAGE_RESEARCH", 1); break;
            case IAP_Holder.hondon_comb: GetRewardInit("PACKAGE_HONDON_COMB", 1); break;


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

            case "Scroll_Comb":
                Base_Manager.Data.Item_Holder["Scroll_Comb"].Hero_Card_Amount += 150000;                
                break;

            case "Research_Item_Ticket":
                Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount += 3;
                break;

            case "Research_Levelup_Book":
                Base_Manager.Data.Item_Holder["Research_Levelup_Book"].Hero_Card_Amount += 1;
                break;

            case "PACKAGE_ADS": 
                Data_Manager.Main_Players_Data.isBuyADPackage = true;
                Data_Manager.Main_Players_Data.DiaMond += 3000;
                Base_Manager.BACKEND.Log_Get_Dia("Get_Package_ADS");
                Base_Manager.Data.Item_Holder["SWORD"].Hero_Card_Amount += 15;
                Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount += 1000;
                break;

            case "PACKAGE_RESEARCH":
                
                Data_Manager.Main_Players_Data.DiaMond += 5000;
                Base_Manager.BACKEND.Log_Get_Dia("Get_Package_Research");
                Base_Manager.Data.Item_Holder["Research_Levelup_Book"].Hero_Card_Amount += 2;
                Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount += 5;
                break;

            case "PACKAGE_HONDON_COMB":

                Data_Manager.Main_Players_Data.DiaMond += 2500;
                Base_Manager.BACKEND.Log_Get_Dia("get_Package_Hondon_Comb");
                Base_Manager.Data.Item_Holder["Blood"].Hero_Card_Amount += 4000;
                Base_Manager.Data.Item_Holder["Hondon_Potion"].Hero_Card_Amount += 4000;
                break;

            case "PACKAGE_ENHANCEMENT":
                
                Data_Manager.Main_Players_Data.DiaMond += 2500;
                Base_Manager.BACKEND.Log_Get_Dia("Get_Package_ENHANCEMENT");
                Base_Manager.Data.Item_Holder["DEF_Enhancement"].Hero_Card_Amount += 3;
                Base_Manager.Data.Item_Holder["Bonus_Enhancement"].Hero_Card_Amount += 3;
                break;

            case "DIA_PASS":
                Data_Manager.Main_Players_Data.DiaMond += 30000;
                Data_Manager.Main_Players_Data.isBUY_DIA_PASS = true;
                Base_Manager.BACKEND.Log_Get_Dia($"DIA_PASS_Purchase");
                break;
                
            case "PACKAGE_STRONG":
                Data_Manager.Main_Players_Data.isBuySTRONGPackage = true;
                Data_Manager.Main_Players_Data.DiaMond += 10000;
                Base_Manager.BACKEND.Log_Get_Dia("Get_Package_Strong");
                Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount += 1500;
                Base_Manager.Data.Item_Holder["DEF_Enhancement"].Hero_Card_Amount += 1;
                Base_Manager.Data.Item_Holder["Research_Levelup_Book"].Hero_Card_Amount += 1;
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

            case "DEF_Enhancement": Base_Manager.Data.Item_Holder["DEF_Enhancement"].Hero_Card_Amount += Count; break;

            case "Dungeon_Dia":
                Data_Manager.Main_Players_Data.User_Key_Assets[0] += Count;
                break;
            case "Dungeon_Enhancement":
                Data_Manager.Main_Players_Data.User_Key_Assets[2] += Count;
                break;
            case "Dungeon_Gold":
                Data_Manager.Main_Players_Data.User_Key_Assets[1] += Count;
                break;
            case "START_Package":
                Data_Manager.Main_Players_Data.isBuySTARTPackage = true;
                Base_Manager.Data.character_Holder["DarkHero"].Hero_Card_Amount += 1;
                Base_Manager.Data.Item_Holder["STAFF"].Hero_Card_Amount += 10;
                Data_Manager.Main_Players_Data.DiaMond += 2500;

                break;
            case "PACKAGE_DIAMOND":
                Data_Manager.Main_Players_Data.isBuyDIAMONDPackage = true;
                Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount += 3000;
                Data_Manager.Main_Players_Data.DiaMond += 43000;
                Data_Manager.Main_Players_Data.User_Key_Assets[0] += 5;

                break;
        }

    }
}
