using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Combination : UI_Base
{
    [SerializeField]
    private UI_Inventory_Parts First_Comb_Parts;
    [SerializeField]
    private UI_Inventory_Parts Second_Comb_Parts;
    [SerializeField]
    private UI_Inventory_Parts Third_Comb_Parts;
    [SerializeField]
    private UI_Inventory_Parts Four_Comb_Parts;
    [SerializeField]
    private UI_Inventory_Parts Selected_Item_Parts;

    [SerializeField]
    private TextMeshProUGUI First_Parts_Count_Text;
    [SerializeField]
    private TextMeshProUGUI Second_Parts_Count_Text;
    [SerializeField]
    private TextMeshProUGUI Third_Parts_Count_Text;
    [SerializeField]
    private TextMeshProUGUI Four_Parts_Count_Text;


    private bool is_Comb_Scroll = false;
    private bool is_Chaos_DarkHero_Book = false;
    private bool is_Summon_DarkHero = false;
    private bool is_Meat_To_Dia = false;
    private bool is_Potion_To_Ball = false;

    public override bool Init()
    {
        First_Comb_Parts.gameObject.SetActive(false);
        Second_Comb_Parts.gameObject.SetActive(false);
        Third_Comb_Parts.gameObject.SetActive(false);
        Four_Comb_Parts.gameObject.SetActive(false);
        Selected_Item_Parts.gameObject.SetActive(false);
        is_Comb_Scroll = false;
        is_Chaos_DarkHero_Book = false;
        is_Summon_DarkHero = false;
        is_Meat_To_Dia = false;
        is_Potion_To_Ball = false;

        _ = Base_Manager.BACKEND.WriteData();

        return base.Init();
    }
    /// <summary>
    /// 조합스크롤 제작식을 노출시킵니다.
    /// </summary>
    public void Set_Comb_Scroll()
    {
        Init();

        is_Comb_Scroll = true;
        Selected_Item_Parts.gameObject.SetActive(true);
        Holder Select_holder = new Holder();
        Select_holder.Hero_Card_Amount = 1;
        Selected_Item_Parts.Init("Scroll_Comb", Select_holder);
        
        First_Comb_Parts.gameObject.SetActive(true);
        Second_Comb_Parts.gameObject.SetActive(true);
        Holder holder = new Holder();
        holder.Hero_Card_Amount = 1000;      
        First_Comb_Parts.Init("Potion", holder);
        First_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Potion"].Hero_Card_Amount, holder.Hero_Card_Amount);
        First_Parts_Count_Text.color = Utils.Item_Count("Potion", 1000) ? Color.green : Color.red;
        Holder holder_2 = new Holder();
        holder_2.Hero_Card_Amount = 1500;
        Second_Comb_Parts.Init("scroll", holder_2);
        Second_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["scroll"].Hero_Card_Amount, holder_2.Hero_Card_Amount);
        Second_Parts_Count_Text.color = Utils.Item_Count("scroll", 1500) ? Color.green : Color.red;
    }
    /// <summary>
    /// 다이아몬드 수급 제작식을 노출시킵니다.
    /// </summary>
    public void Set_Meat_To_Dia()
    {
        Init();

        is_Meat_To_Dia = true;
        Selected_Item_Parts.gameObject.SetActive(true);
        Holder Select_holder = new Holder();
        Select_holder.Hero_Card_Amount = 50;
        Selected_Item_Parts.Init("Combination_Dia", Select_holder);

        First_Comb_Parts.gameObject.SetActive(true);
        
        Holder holder = new Holder();
        holder.Hero_Card_Amount = 1000;
        First_Comb_Parts.Init("Meat", holder);
        First_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Meat"].Hero_Card_Amount, holder.Hero_Card_Amount);
        First_Parts_Count_Text.color = Utils.Item_Count("Meat", 1000) ? Color.green : Color.red;
        
    }
    /// <summary>
    /// 혼돈의 구슬 제작식을 노출시킵니다.
    /// </summary>
    public void Set_Hondon_Potion_To_Hondon_Ball()
    {
        Init();

        is_Potion_To_Ball = true;
        Selected_Item_Parts.gameObject.SetActive(true);
        Holder Select_holder = new Holder();
        Select_holder.Hero_Card_Amount = 500;
        Selected_Item_Parts.Init("Hondon_Ball", Select_holder);

        First_Comb_Parts.gameObject.SetActive(true);

        Holder holder = new Holder();
        holder.Hero_Card_Amount = 500;
        First_Comb_Parts.Init("Hondon_Potion", holder);
        First_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Hondon_Potion"].Hero_Card_Amount, holder.Hero_Card_Amount);
        First_Parts_Count_Text.color = Utils.Item_Count("Hondon_Potion", 500) ? Color.green : Color.red;

    }
    /// <summary>
    /// 다크히어로 소환 제작식
    /// </summary>
    public void Set_Summon_DarkHero_Summon()
    {
        Init();

        is_Summon_DarkHero = true;
        Selected_Item_Parts.gameObject.SetActive(true);
        Holder Select_holder = new Holder();
        Select_holder.Hero_Card_Amount = 3;
        Selected_Item_Parts.Init("Summon_DarkHero", Select_holder);

        First_Comb_Parts.gameObject.SetActive(true);     
        Holder holder = new Holder();
        holder.Hero_Card_Amount = 1;
        First_Comb_Parts.Init("Comb_Book_Summon_Hero", holder);
        First_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Comb_Book_Summon_Hero"].Hero_Card_Amount, holder.Hero_Card_Amount);
        First_Parts_Count_Text.color = Utils.Item_Count("Comb_Book_Summon_Hero", 1) ? Color.green : Color.red;
    }
    /// <summary>
    /// 혼돈등급 다크히어로 영웅 소환서 제작식
    /// </summary>
    public void Set_Chaos_DarkHero_Book()
    {
        Init();
        is_Chaos_DarkHero_Book = true;

        Selected_Item_Parts.gameObject.SetActive(true);
        Holder Select_holder = new Holder();
        Select_holder.Hero_Card_Amount = 1;
        Selected_Item_Parts.Init("Comb_Book_Summon_Hero", Select_holder);

        First_Comb_Parts.gameObject.SetActive(true);
        Second_Comb_Parts.gameObject.SetActive(true);
        Third_Comb_Parts.gameObject.SetActive(true);
        Four_Comb_Parts.gameObject.SetActive(true);
        Holder holder = new Holder();
        holder.Hero_Card_Amount = 5000;
        First_Comb_Parts.Init("Book", holder);
        First_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Book"].Hero_Card_Amount, holder.Hero_Card_Amount);
        First_Parts_Count_Text.color = Utils.Item_Count("Book", 5000) ? Color.green : Color.red;
        Holder holder_2 = new Holder();
        holder_2.Hero_Card_Amount = 3500;
        Second_Comb_Parts.Init("Hondon_Potion", holder_2);
        Second_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Hondon_Potion"].Hero_Card_Amount, holder_2.Hero_Card_Amount);
        Second_Parts_Count_Text.color = Utils.Item_Count("Hondon_Potion", 3500) ? Color.green : Color.red;
        Holder holder_3 = new Holder();
        holder_3.Hero_Card_Amount = 1000;
        Third_Comb_Parts.Init("Steel", holder_3);
        Third_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount, holder_3.Hero_Card_Amount);
        Third_Parts_Count_Text.color = Utils.Item_Count("Steel", 1000) ? Color.green : Color.red;
        Holder holder_4 = new Holder();
        holder_4.Hero_Card_Amount = 1000;
        Four_Comb_Parts.Init("Scroll_Comb", holder_4);
        Four_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Scroll_Comb"].Hero_Card_Amount, holder_4.Hero_Card_Amount);
        Four_Parts_Count_Text.color = Utils.Item_Count("Scroll_Comb", 1000) ? Color.green : Color.red;
    }


    public void Combination()
    {
        _ = Base_Manager.BACKEND.WriteData();

        if (is_Comb_Scroll)
        {
            if(Base_Manager.Data.Item_Holder["Potion"].Hero_Card_Amount >= 1000 && Base_Manager.Data.Item_Holder["scroll"].Hero_Card_Amount >= 1500)
            {
                Base_Manager.Data.Item_Holder["Potion"].Hero_Card_Amount -= 1000;
                Base_Manager.Data.Item_Holder["scroll"].Hero_Card_Amount -= 1500;
                var item = Base_Manager.Data.Data_Item_Dictionary["Scroll_Comb"];
                Base_Manager.Inventory.Get_Item(item);
                Base_Canvas.instance.Get_Toast_Popup().Initialize("조합 스크롤 제작 성공");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");
                Set_Comb_Scroll();
               
            }
            else
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("조합 스크롤 제작에 필요한 재료가 부족합니다.");
                return;
            }
           
        }

        else if (is_Chaos_DarkHero_Book)
        {
            if (Base_Manager.Data.Item_Holder["Book"].Hero_Card_Amount >= 5000 && 
                Base_Manager.Data.Item_Holder["Hondon_Potion"].Hero_Card_Amount >= 3500 &&
                Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount >= 1000 &&
                Base_Manager.Data.Item_Holder["Scroll_Comb"].Hero_Card_Amount >= 1000)
            {
                Base_Manager.Data.Item_Holder["Book"].Hero_Card_Amount -= 5000;
                Base_Manager.Data.Item_Holder["Hondon_Potion"].Hero_Card_Amount -= 3500;
                Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount -= 1000;
                Base_Manager.Data.Item_Holder["Scroll_Comb"].Hero_Card_Amount -= 1000;
                var item = Base_Manager.Data.Data_Item_Dictionary["Comb_Book_Summon_Hero"];
                Base_Manager.Inventory.Get_Item(item);
                Base_Canvas.instance.Get_Toast_Popup().Initialize("소환서 - 다크히어로 제작 성공");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");
                Set_Chaos_DarkHero_Book();
            }
            else
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("소환서 - 다크히어로 제작에 필요한 재료가 부족합니다.");
                return;
            }

            
        }

        else if (is_Summon_DarkHero)
        {
            if (Base_Manager.Data.Item_Holder["Comb_Book_Summon_Hero"].Hero_Card_Amount >= 1)
            {
                Base_Manager.Data.Item_Holder["Comb_Book_Summon_Hero"].Hero_Card_Amount -= 1;

                Base_Manager.Data.character_Holder["DarkHero"].Hero_Card_Amount++;
                Base_Manager.Data.character_Holder["DarkHero"].Hero_Card_Amount++;
                Base_Manager.Data.character_Holder["DarkHero"].Hero_Card_Amount++;

                Base_Canvas.instance.Get_Toast_Popup().Initialize("다크히어로 제작 성공");
                Base_Manager.BACKEND.Log_Try_Combination_DarkHero();
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

                Set_Summon_DarkHero_Summon();
            }
            else
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("다크히어로 제작에 필요한 재료가 부족합니다.");
                return;
            }
           
        }

        else if (is_Meat_To_Dia)
        {
            if (Base_Manager.Data.Item_Holder["Meat"].Hero_Card_Amount >= 1000)
            {
                Base_Manager.Data.Item_Holder["Meat"].Hero_Card_Amount -= 1000;

                Data_Manager.Main_Players_Data.DiaMond += 50;
                Base_Manager.BACKEND.Log_Get_Dia("Combination_Dia");
                
                Base_Canvas.instance.Get_Toast_Popup().Initialize("다이아몬드 제작 성공");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

                Set_Meat_To_Dia();
            }
            else
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("다이아몬드 제작에 필요한 재료가 부족합니다.");
                return;
            }

        }

        else if (is_Potion_To_Ball)
        {
            if (Base_Manager.Data.Item_Holder["Hondon_Potion"].Hero_Card_Amount >= 500)
            {
                Base_Manager.Data.Item_Holder["Hondon_Potion"].Hero_Card_Amount -= 500;

                Base_Manager.Data.Item_Holder["Hondon_Ball"].Hero_Card_Amount += 500;

                Base_Manager.BACKEND.Log_Get_Combination_Hondon_Ball("Combination_Hondon_Ball");

                Base_Canvas.instance.Get_Toast_Popup().Initialize("혼돈의 구슬 제작 성공");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

                Set_Hondon_Potion_To_Hondon_Ball();
            }
            else
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("혼돈의 구슬 제작에 필요한 재료가 부족합니다.");
                return;
            }

        }

        _ =Base_Manager.BACKEND.WriteData(); // _= 는 Task 비동기메서드나 일반 메서드의 반환값이 필요없을 때,
                                            // 반환값 무시. 결과를 변수에 저장하지 않고 실행만 하는 것을 의미합니다.
    }


    public override void DisableOBJ()
    {
        base.DisableOBJ();
    }
}
