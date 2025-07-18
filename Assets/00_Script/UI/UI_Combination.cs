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

    #region Combination_Items
    private bool is_Comb_Scroll = false;

    private bool is_Chaos_DarkHero_Book = false;
    private bool is_Summon_DarkHero = false;

    private bool is_Chaos_Caster_Book = false;
    private bool is_Summon_Chaos_Caster = false;

    private bool is_Aqua_Book = false;
    private bool is_Summon_Aqua = false;

    private bool is_Meat_To_Dia = false;
    private bool is_Potion_To_Ball = false;
    #endregion

    #region Research_Items
    private bool is_Slime_Potion = false;
    private bool is_Spider_Potion = false;
    private bool is_Skeleton_Potion = false;
    private bool is_Plant_Potion = false;
    private bool is_Orc_Potion = false;
    private bool is_Mushroom_Potion = false;
    private bool is_Turtle_Potion = false;

    private bool is_Research_Levelup_Book = false;
    #endregion
    public override bool Init()
    {
        First_Comb_Parts.gameObject.SetActive(false);
        Second_Comb_Parts.gameObject.SetActive(false);
        Third_Comb_Parts.gameObject.SetActive(false);
        Four_Comb_Parts.gameObject.SetActive(false);
        Selected_Item_Parts.gameObject.SetActive(false);

        #region Combination_Items
        is_Comb_Scroll = false;
        is_Chaos_DarkHero_Book = false;
        is_Summon_DarkHero = false;
        is_Meat_To_Dia = false;
        is_Potion_To_Ball = false;
        is_Chaos_Caster_Book = false;
        is_Summon_Chaos_Caster = false;
        is_Summon_Aqua = false;
        is_Aqua_Book = false;
        #endregion

        #region Research_Items
        is_Slime_Potion = false;
        is_Spider_Potion = false;
        is_Skeleton_Potion = false;
        is_Plant_Potion = false;
        is_Orc_Potion = false;
        is_Mushroom_Potion = false;
        is_Turtle_Potion = false;

        is_Research_Levelup_Book = false;
        #endregion

        return base.Init();
    }

    #region Combination_Items
    /// <summary>
    /// 조합스크롤 제작식을 노출시킵니다.
    /// </summary>
    public void Set_Comb_Scroll()
    {
        Init();

        is_Comb_Scroll = true;
        Selected_Item_Parts.gameObject.SetActive(true);
        Holder Select_holder = new Holder();
        Select_holder.Hero_Card_Amount = 5000;
        Selected_Item_Parts.Init("Scroll_Comb", Select_holder);
        
        First_Comb_Parts.gameObject.SetActive(true);
        Second_Comb_Parts.gameObject.SetActive(true);
        Holder holder = new Holder();
        holder.Hero_Card_Amount = 10000;      
        First_Comb_Parts.Init("Potion", holder);
        First_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Potion"].Hero_Card_Amount, holder.Hero_Card_Amount);
        First_Parts_Count_Text.color = Utils.Item_Count("Potion", 10000) ? Color.green : Color.red;
        Holder holder_2 = new Holder();
        holder_2.Hero_Card_Amount = 15000;
        Second_Comb_Parts.Init("scroll", holder_2);
        Second_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["scroll"].Hero_Card_Amount, holder_2.Hero_Card_Amount);
        Second_Parts_Count_Text.color = Utils.Item_Count("scroll", 15000) ? Color.green : Color.red;
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
        Select_holder.Hero_Card_Amount = 500;
        Selected_Item_Parts.Init("Combination_Dia", Select_holder);

        First_Comb_Parts.gameObject.SetActive(true);
        
        Holder holder = new Holder();
        holder.Hero_Card_Amount = 10000;
        First_Comb_Parts.Init("Meat", holder);
        First_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Meat"].Hero_Card_Amount, holder.Hero_Card_Amount);
        First_Parts_Count_Text.color = Utils.Item_Count("Meat", 10000) ? Color.green : Color.red;
        
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
        Select_holder.Hero_Card_Amount = 1;
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
        holder.Hero_Card_Amount = 50000;
        First_Comb_Parts.Init("Book", holder);
        First_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Book"].Hero_Card_Amount, holder.Hero_Card_Amount);
        First_Parts_Count_Text.color = Utils.Item_Count("Book", 50000) ? Color.green : Color.red;
        Holder holder_2 = new Holder();
        holder_2.Hero_Card_Amount = 3500;
        Second_Comb_Parts.Init("Hondon_Potion", holder_2);
        Second_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Hondon_Potion"].Hero_Card_Amount, holder_2.Hero_Card_Amount);
        Second_Parts_Count_Text.color = Utils.Item_Count("Hondon_Potion", 3500) ? Color.green : Color.red;
        Holder holder_3 = new Holder();
        holder_3.Hero_Card_Amount = 2000;
        Third_Comb_Parts.Init("Steel", holder_3);
        Third_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount, holder_3.Hero_Card_Amount);
        Third_Parts_Count_Text.color = Utils.Item_Count("Steel", 2000) ? Color.green : Color.red;
        Holder holder_4 = new Holder();
        holder_4.Hero_Card_Amount = 50000;
        Four_Comb_Parts.Init("Scroll_Comb", holder_4);
        Four_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Scroll_Comb"].Hero_Card_Amount, holder_4.Hero_Card_Amount);
        Four_Parts_Count_Text.color = Utils.Item_Count("Scroll_Comb", 50000) ? Color.green : Color.red;
    }
    public void Set_Chaos_Chaos_Caster_Book()
    {
        Init();
        is_Chaos_Caster_Book = true;

        Selected_Item_Parts.gameObject.SetActive(true);
        Holder Select_holder = new Holder();
        Select_holder.Hero_Card_Amount = 1;
        Selected_Item_Parts.Init("Comb_Book_Summon_Chaos_Caster", Select_holder);

        First_Comb_Parts.gameObject.SetActive(true);
        Second_Comb_Parts.gameObject.SetActive(true);
        Third_Comb_Parts.gameObject.SetActive(true);
        Four_Comb_Parts.gameObject.SetActive(true);
        Holder holder = new Holder();
        holder.Hero_Card_Amount = 50000;
        First_Comb_Parts.Init("Book", holder);
        First_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Book"].Hero_Card_Amount, holder.Hero_Card_Amount);
        First_Parts_Count_Text.color = Utils.Item_Count("Book", 50000) ? Color.green : Color.red;
        Holder holder_2 = new Holder();
        holder_2.Hero_Card_Amount = 3500;
        Second_Comb_Parts.Init("Hondon_Ball", holder_2);
        Second_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Hondon_Ball"].Hero_Card_Amount, holder_2.Hero_Card_Amount);
        Second_Parts_Count_Text.color = Utils.Item_Count("Hondon_Ball", 3500) ? Color.green : Color.red;
        Holder holder_3 = new Holder();
        holder_3.Hero_Card_Amount = 2000;
        Third_Comb_Parts.Init("Steel", holder_3);
        Third_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount, holder_3.Hero_Card_Amount);
        Third_Parts_Count_Text.color = Utils.Item_Count("Steel", 2000) ? Color.green : Color.red;
        Holder holder_4 = new Holder();
        holder_4.Hero_Card_Amount = 50000;
        Four_Comb_Parts.Init("Scroll_Comb", holder_4);
        Four_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Scroll_Comb"].Hero_Card_Amount, holder_4.Hero_Card_Amount);
        Four_Parts_Count_Text.color = Utils.Item_Count("Scroll_Comb", 50000) ? Color.green : Color.red;
    }
    /// <summary>
    /// 혼돈등급 카오스캐스터 소환
    /// </summary>
    public void Set_Summon_Chaos_Caster_Summon()
    {
        Init();

        is_Summon_Chaos_Caster = true;
        Selected_Item_Parts.gameObject.SetActive(true);
        Holder Select_holder = new Holder();
        Select_holder.Hero_Card_Amount = 1;
        Selected_Item_Parts.Init("Summon_Chaos_Caster", Select_holder);

        First_Comb_Parts.gameObject.SetActive(true);
        Holder holder = new Holder();
        holder.Hero_Card_Amount = 1;
        First_Comb_Parts.Init("Comb_Book_Summon_Chaos_Caster", holder);
        First_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Comb_Book_Summon_Chaos_Caster"].Hero_Card_Amount, holder.Hero_Card_Amount);
        First_Parts_Count_Text.color = Utils.Item_Count("Comb_Book_Summon_Chaos_Caster", 1) ? Color.green : Color.red;
    }
    /// <summary>
    /// 아쿠아 템페스트 소환서
    /// </summary>
    public void Set_Chaos_Aqua_Book()
    {
        Init();
        is_Aqua_Book = true;

        Selected_Item_Parts.gameObject.SetActive(true);
        Holder Select_holder = new Holder();
        Select_holder.Hero_Card_Amount = 1;
        Selected_Item_Parts.Init("Comb_Book_Summon_Aqua", Select_holder);

        First_Comb_Parts.gameObject.SetActive(true);
        Second_Comb_Parts.gameObject.SetActive(true);
        Third_Comb_Parts.gameObject.SetActive(true);
        Four_Comb_Parts.gameObject.SetActive(true);
        Holder holder = new Holder();
        holder.Hero_Card_Amount = 50000;
        First_Comb_Parts.Init("Book", holder);
        First_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Book"].Hero_Card_Amount, holder.Hero_Card_Amount);
        First_Parts_Count_Text.color = Utils.Item_Count("Book", 50000) ? Color.green : Color.red;
        Holder holder_2 = new Holder();
        holder_2.Hero_Card_Amount = 3500;
        Second_Comb_Parts.Init("Hondon_Ball", holder_2);
        Second_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Hondon_Ball"].Hero_Card_Amount, holder_2.Hero_Card_Amount);
        Second_Parts_Count_Text.color = Utils.Item_Count("Hondon_Ball", 3500) ? Color.green : Color.red;
        Holder holder_3 = new Holder();
        holder_3.Hero_Card_Amount = 2000;
        Third_Comb_Parts.Init("Steel", holder_3);
        Third_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount, holder_3.Hero_Card_Amount);
        Third_Parts_Count_Text.color = Utils.Item_Count("Steel", 2000) ? Color.green : Color.red;
        Holder holder_4 = new Holder();
        holder_4.Hero_Card_Amount = 50000;
        Four_Comb_Parts.Init("Scroll_Comb", holder_4);
        Four_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Scroll_Comb"].Hero_Card_Amount, holder_4.Hero_Card_Amount);
        Four_Parts_Count_Text.color = Utils.Item_Count("Scroll_Comb", 50000) ? Color.green : Color.red;
    }
    /// <summary>
    /// 혼돈등급 아쿠아 템페스트 소환
    /// </summary>
    public void Set_Summon_Aqua()
    {
        Init();

        is_Summon_Aqua = true;
        Selected_Item_Parts.gameObject.SetActive(true);
        Holder Select_holder = new Holder();
        Select_holder.Hero_Card_Amount = 1;
        Selected_Item_Parts.Init("Summon_Aqua", Select_holder);

        First_Comb_Parts.gameObject.SetActive(true);
        Holder holder = new Holder();
        holder.Hero_Card_Amount = 1;
        First_Comb_Parts.Init("Comb_Book_Summon_Aqua", holder);
        First_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Comb_Book_Summon_Aqua"].Hero_Card_Amount, holder.Hero_Card_Amount);
        First_Parts_Count_Text.color = Utils.Item_Count("Comb_Book_Summon_Aqua", 1) ? Color.green : Color.red;
    }
    #endregion

    #region Research_Items
    public void Set_Slime_Potion()
    {
        Init();

        is_Slime_Potion = true;
        Selected_Item_Parts.gameObject.SetActive(true);
        Holder Select_holder = new Holder();
        Select_holder.Hero_Card_Amount = 2000;
        Selected_Item_Parts.Init("Slime_Potion", Select_holder);

        First_Comb_Parts.gameObject.SetActive(true);

        Holder holder = new Holder();
        holder.Hero_Card_Amount = 1;
        First_Comb_Parts.Init("Research_Item_Ticket", holder);
        First_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount, holder.Hero_Card_Amount);
        First_Parts_Count_Text.color = Utils.Item_Count("Research_Item_Ticket", 1) ? Color.green : Color.red;

    }
    public void Set_Spider_Potion()
    {
        Init();

        is_Spider_Potion = true;
        Selected_Item_Parts.gameObject.SetActive(true);
        Holder Select_holder = new Holder();
        Select_holder.Hero_Card_Amount = 2000;
        Selected_Item_Parts.Init("Spider_Potion", Select_holder);

        First_Comb_Parts.gameObject.SetActive(true);

        Holder holder = new Holder();
        holder.Hero_Card_Amount = 1;
        First_Comb_Parts.Init("Research_Item_Ticket", holder);
        First_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount, holder.Hero_Card_Amount);
        First_Parts_Count_Text.color = Utils.Item_Count("Research_Item_Ticket", 1) ? Color.green : Color.red;

    }
    public void Set_Skeleton_Potion()
    {
        Init();

        is_Skeleton_Potion = true;
        Selected_Item_Parts.gameObject.SetActive(true);
        Holder Select_holder = new Holder();
        Select_holder.Hero_Card_Amount = 2000;
        Selected_Item_Parts.Init("Skeleton_Potion", Select_holder);

        First_Comb_Parts.gameObject.SetActive(true);

        Holder holder = new Holder();
        holder.Hero_Card_Amount = 1;
        First_Comb_Parts.Init("Research_Item_Ticket", holder);
        First_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount, holder.Hero_Card_Amount);
        First_Parts_Count_Text.color = Utils.Item_Count("Research_Item_Ticket", 1) ? Color.green : Color.red;

    }
    public void Set_Plant_Potion()
    {
        Init();

        is_Plant_Potion = true;
        Selected_Item_Parts.gameObject.SetActive(true);
        Holder Select_holder = new Holder();
        Select_holder.Hero_Card_Amount = 2000;
        Selected_Item_Parts.Init("Plant_Potion", Select_holder);

        First_Comb_Parts.gameObject.SetActive(true);

        Holder holder = new Holder();
        holder.Hero_Card_Amount = 1;
        First_Comb_Parts.Init("Research_Item_Ticket", holder);
        First_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount, holder.Hero_Card_Amount);
        First_Parts_Count_Text.color = Utils.Item_Count("Research_Item_Ticket", 1) ? Color.green : Color.red;

    }
    public void Set_Orc_Potion()
    {
        Init();

        is_Orc_Potion = true;
        Selected_Item_Parts.gameObject.SetActive(true);
        Holder Select_holder = new Holder();
        Select_holder.Hero_Card_Amount = 2000;
        Selected_Item_Parts.Init("Orc_Potion", Select_holder);

        First_Comb_Parts.gameObject.SetActive(true);

        Holder holder = new Holder();
        holder.Hero_Card_Amount = 1;
        First_Comb_Parts.Init("Research_Item_Ticket", holder);
        First_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount, holder.Hero_Card_Amount);
        First_Parts_Count_Text.color = Utils.Item_Count("Research_Item_Ticket", 1) ? Color.green : Color.red;

    }
    public void Set_Mushroom_Potion()
    {
        Init();

        is_Mushroom_Potion = true;
        Selected_Item_Parts.gameObject.SetActive(true);
        Holder Select_holder = new Holder();
        Select_holder.Hero_Card_Amount = 2000;
        Selected_Item_Parts.Init("Mushroom_Potion", Select_holder);

        First_Comb_Parts.gameObject.SetActive(true);

        Holder holder = new Holder();
        holder.Hero_Card_Amount = 1;
        First_Comb_Parts.Init("Research_Item_Ticket", holder);
        First_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount, holder.Hero_Card_Amount);
        First_Parts_Count_Text.color = Utils.Item_Count("Research_Item_Ticket", 1) ? Color.green : Color.red;

    }
    public void Set_Turtle_Potion()
    {
        Init();

        is_Turtle_Potion = true;
        Selected_Item_Parts.gameObject.SetActive(true);
        Holder Select_holder = new Holder();
        Select_holder.Hero_Card_Amount = 2000;
        Selected_Item_Parts.Init("Turtle_Potion", Select_holder);

        First_Comb_Parts.gameObject.SetActive(true);

        Holder holder = new Holder();
        holder.Hero_Card_Amount = 1;
        First_Comb_Parts.Init("Research_Item_Ticket", holder);
        First_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount, holder.Hero_Card_Amount);
        First_Parts_Count_Text.color = Utils.Item_Count("Research_Item_Ticket", 1) ? Color.green : Color.red;

    }
    public void Set_Research_Levelup_Book()
    {
        Init();

        is_Research_Levelup_Book = true;
        Selected_Item_Parts.gameObject.SetActive(true);
        Holder Select_holder = new Holder();
        Select_holder.Hero_Card_Amount = 1;
        Selected_Item_Parts.Init("Research_Levelup_Book", Select_holder);

        First_Comb_Parts.gameObject.SetActive(true);
        Second_Comb_Parts.gameObject.SetActive(true);
        Holder holder = new Holder();
        holder.Hero_Card_Amount = 100;
        First_Comb_Parts.Init("Research_Book_Stone", holder);
        First_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Research_Book_Stone"].Hero_Card_Amount, holder.Hero_Card_Amount);
        First_Parts_Count_Text.color = Utils.Item_Count("Research_Book_Stone", 100) ? Color.green : Color.red;
        Holder holder_2 = new Holder();
        holder_2.Hero_Card_Amount = 3000;
        Second_Comb_Parts.Init("Steel", holder_2);
        Second_Parts_Count_Text.text = string.Format("({0}/{1})", Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount, holder_2.Hero_Card_Amount);
        Second_Parts_Count_Text.color = Utils.Item_Count("Steel", 3000) ? Color.green : Color.red;
    }
    #endregion
    public void Combination()
    {        
        if (is_Comb_Scroll)
        {
            if(Base_Manager.Data.Item_Holder["Potion"].Hero_Card_Amount >= 10000 && Base_Manager.Data.Item_Holder["scroll"].Hero_Card_Amount >= 15000)
            {
                Base_Manager.Data.Item_Holder["Potion"].Hero_Card_Amount -= 10000;
                Base_Manager.Data.Item_Holder["scroll"].Hero_Card_Amount -= 15000;
                var item = Base_Manager.Data.Data_Item_Dictionary["Scroll_Comb"];
                Base_Manager.Inventory.Get_Item(item , 5000);
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
            if (Base_Manager.Data.Item_Holder["Book"].Hero_Card_Amount >= 50000 && 
                Base_Manager.Data.Item_Holder["Hondon_Potion"].Hero_Card_Amount >= 3500 &&
                Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount >= 2000 &&
                Base_Manager.Data.Item_Holder["Scroll_Comb"].Hero_Card_Amount >= 50000)
            {
                Base_Manager.Data.Item_Holder["Book"].Hero_Card_Amount -= 50000;
                Base_Manager.Data.Item_Holder["Hondon_Potion"].Hero_Card_Amount -= 3500;
                Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount -= 2000;
                Base_Manager.Data.Item_Holder["Scroll_Comb"].Hero_Card_Amount -= 50000;
                var item = Base_Manager.Data.Data_Item_Dictionary["Comb_Book_Summon_Hero"];
                Base_Manager.Inventory.Get_Item(item);
                Base_Canvas.instance.Get_Toast_Popup().Initialize("소환서 - 다크히어로 제작 성공");
                Utils.SendSystemLikeMessage("★ <color=#FFFF00>[소환서 - 다크히어로]</color>를 제작했습니다!");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");
                Set_Chaos_DarkHero_Book();
            }
            else
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("소환서 - 다크히어로 제작에 필요한 재료가 부족합니다.");
                return;
            }

            
        }

        else if (is_Chaos_Caster_Book)
        {
            if (Base_Manager.Data.Item_Holder["Book"].Hero_Card_Amount >= 50000 &&
                Base_Manager.Data.Item_Holder["Hondon_Ball"].Hero_Card_Amount >= 3500 &&
                Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount >= 2000 &&
                Base_Manager.Data.Item_Holder["Scroll_Comb"].Hero_Card_Amount >= 50000)
            {
                Base_Manager.Data.Item_Holder["Book"].Hero_Card_Amount -= 5000;
                Base_Manager.Data.Item_Holder["Hondon_Ball"].Hero_Card_Amount -= 3500;
                Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount -= 2000;
                Base_Manager.Data.Item_Holder["Scroll_Comb"].Hero_Card_Amount -= 50000;
                var item = Base_Manager.Data.Data_Item_Dictionary["Comb_Book_Summon_Chaos_Caster"];
                Base_Manager.Inventory.Get_Item(item);
                Base_Canvas.instance.Get_Toast_Popup().Initialize("소환서 - 카오스캐스터 제작 성공");
                Utils.SendSystemLikeMessage("★ <color=#FFFF00>[소환서 - 카오스캐스터]</color>를 제작했습니다!");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");
                Set_Chaos_Chaos_Caster_Book();
            }
            else
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("소환서 - 카오스캐스터 제작에 필요한 재료가 부족합니다.");
                return;
            }


        }

        else if (is_Aqua_Book)
        {
            if (Base_Manager.Data.Item_Holder["Book"].Hero_Card_Amount >= 50000 &&
                Base_Manager.Data.Item_Holder["Hondon_Ball"].Hero_Card_Amount >= 3500 &&
                Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount >= 2000 &&
                Base_Manager.Data.Item_Holder["Scroll_Comb"].Hero_Card_Amount >= 50000)
            {
                Base_Manager.Data.Item_Holder["Book"].Hero_Card_Amount -= 5000;
                Base_Manager.Data.Item_Holder["Hondon_Ball"].Hero_Card_Amount -= 3500;
                Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount -= 2000;
                Base_Manager.Data.Item_Holder["Scroll_Comb"].Hero_Card_Amount -= 50000;
                var item = Base_Manager.Data.Data_Item_Dictionary["Comb_Book_Summon_Aqua"];
                Base_Manager.Inventory.Get_Item(item);
                Base_Canvas.instance.Get_Toast_Popup().Initialize("소환서 - 아쿠아 템페스트 제작 성공");
                Utils.SendSystemLikeMessage("★ <color=#FFFF00>[소환서 - 아쿠아 템페스트]</color>를 제작했습니다!");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");
                Set_Chaos_Aqua_Book();
            }
            else
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("소환서 - 아쿠아 템페스트 제작에 필요한 재료가 부족합니다.");
                return;
            }


        }

        else if (is_Summon_Aqua)
        {
            if (Base_Manager.Data.Item_Holder["Comb_Book_Summon_Aqua"].Hero_Card_Amount >= 1)
            {
                Base_Manager.Data.Item_Holder["Comb_Book_Summon_Aqua"].Hero_Card_Amount -= 1;

                Base_Manager.Data.character_Holder["Aqua_Tempest"].Hero_Card_Amount++;

                Base_Canvas.instance.Get_Toast_Popup().Initialize("아쿠아 템페스트 제작 성공");
                
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

                Set_Summon_Aqua();
            }
            else
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("아쿠아 템페스트 제작에 필요한 재료가 부족합니다.");
                return;
            }

        }

        else if (is_Summon_DarkHero)
        {
            if (Base_Manager.Data.Item_Holder["Comb_Book_Summon_Hero"].Hero_Card_Amount >= 1)
            {
                Base_Manager.Data.Item_Holder["Comb_Book_Summon_Hero"].Hero_Card_Amount -= 1;

                Base_Manager.Data.character_Holder["DarkHero"].Hero_Card_Amount++;
                
                Base_Canvas.instance.Get_Toast_Popup().Initialize("다크히어로 제작 성공");
                
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

                Set_Summon_DarkHero_Summon();
            }
            else
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("다크히어로 제작에 필요한 재료가 부족합니다.");
                return;
            }
           
        }

        else if (is_Summon_Chaos_Caster)
        {
            if (Base_Manager.Data.Item_Holder["Comb_Book_Summon_Chaos_Caster"].Hero_Card_Amount >= 1)
            {
                Base_Manager.Data.Item_Holder["Comb_Book_Summon_Chaos_Caster"].Hero_Card_Amount -= 1;

                Base_Manager.Data.character_Holder["Chaos_Caster"].Hero_Card_Amount++;
                
                Base_Canvas.instance.Get_Toast_Popup().Initialize("카오스캐스터 제작 성공");
                
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

                Set_Summon_Chaos_Caster_Summon();
            }
            else
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("카오스캐스터 제작에 필요한 재료가 부족합니다.");
                return;
            }

        }

        else if (is_Meat_To_Dia)
        {
            if (Base_Manager.Data.Item_Holder["Meat"].Hero_Card_Amount >= 10000)
            {
                Base_Manager.Data.Item_Holder["Meat"].Hero_Card_Amount -= 10000;

                Data_Manager.Main_Players_Data.DiaMond += 500;
                Base_Manager.BACKEND.Log_Get_Dia("Combination_Dia");
                
                Base_Canvas.instance.Get_Toast_Popup().Initialize("다이아몬드 제작 성공");
                Utils.SendSystemLikeMessage("★ <color=#FFFF00>[다이아몬드 500개]</color>를 제작했습니다!");
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

        else if (is_Slime_Potion)
        {
            if (Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount >= 1)
            {
                Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount --;

                Base_Manager.Data.Item_Holder["Slime_Potion"].Hero_Card_Amount += 2000;

                Base_Canvas.instance.Get_Toast_Popup().Initialize("붉은 성수 제작 성공");
                Utils.SendSystemLikeMessage("★ <color=#FFFF00>[붉은 성수 2000개]</color>를 제작했습니다!");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

                Set_Slime_Potion();
            }
            else
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("붉은 성수 제작에 필요한 재료가 부족합니다.");
                return;
            }

        }

        else if (is_Spider_Potion)
        {
            if (Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount >= 1)
            {
                Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount--;

                Base_Manager.Data.Item_Holder["Spider_Potion"].Hero_Card_Amount += 2000;

                Base_Canvas.instance.Get_Toast_Popup().Initialize("맹독 추출액 제작 성공");
                Utils.SendSystemLikeMessage("★ <color=#FFFF00>[맹독 추출액 2000개]</color>를 제작했습니다!");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

                Set_Spider_Potion();
            }
            else
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("맹독 추출액 제작에 필요한 재료가 부족합니다.");
                return;
            }

        }

        else if (is_Skeleton_Potion)
        {
            if (Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount >= 1)
            {
                Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount--;

                Base_Manager.Data.Item_Holder["Skeleton_Potion"].Hero_Card_Amount += 2000;

                Base_Canvas.instance.Get_Toast_Popup().Initialize("망자의 골수액 제작 성공");
                Utils.SendSystemLikeMessage("★ <color=#FFFF00>[망자의 골수액 2000개]</color>를 제작했습니다!");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

                Set_Skeleton_Potion();
            }
            else
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("망자의 골수액 제작에 필요한 재료가 부족합니다.");
                return;
            }

        }

        else if (is_Orc_Potion)
        {
            if (Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount >= 1)
            {
                Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount--;

                Base_Manager.Data.Item_Holder["Orc_Potion"].Hero_Card_Amount += 2000;

                Base_Canvas.instance.Get_Toast_Popup().Initialize("분노의 근섬유액 제작 성공");
                Utils.SendSystemLikeMessage("★ <color=#FFFF00>[분노의 근섬유액 2000개]</color>를 제작했습니다!");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

                Set_Orc_Potion();
            }
            else
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("분노의 근섬유액 제작에 필요한 재료가 부족합니다.");
                return;
            }

        }

        else if (is_Plant_Potion)
        {
            if (Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount >= 1)
            {
                Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount--;

                Base_Manager.Data.Item_Holder["Plant_Potion"].Hero_Card_Amount += 2000;

                Base_Canvas.instance.Get_Toast_Popup().Initialize("탐식의 수액 제작 성공");
                Utils.SendSystemLikeMessage("★ <color=#FFFF00>[탐식의 수액 2000개]</color>를 제작했습니다!");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

                Set_Plant_Potion();
            }
            else
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("탐식의 수액 제작에 필요한 재료가 부족합니다.");
                return;
            }

        }

        else if (is_Mushroom_Potion)
        {
            if (Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount >= 1)
            {
                Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount--;

                Base_Manager.Data.Item_Holder["Mushroom_Potion"].Hero_Card_Amount += 2000;

                Base_Canvas.instance.Get_Toast_Popup().Initialize("포자 추출액 제작 성공");
                Utils.SendSystemLikeMessage("★ <color=#FFFF00>[포자 추출액 2000개]</color>를 제작했습니다!");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

                Set_Mushroom_Potion();
            }
            else
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("포자 추출액 제작에 필요한 재료가 부족합니다.");
                return;
            }

        }

        else if (is_Turtle_Potion)
        {
            if (Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount >= 1)
            {
                Base_Manager.Data.Item_Holder["Research_Item_Ticket"].Hero_Card_Amount--;

                Base_Manager.Data.Item_Holder["Turtle_Potion"].Hero_Card_Amount += 2000;

                Base_Canvas.instance.Get_Toast_Popup().Initialize("경화 점액 제작 성공");
                Utils.SendSystemLikeMessage("★ <color=#FFFF00>[경화 점액 2000개]</color>를 제작했습니다!");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

                Set_Turtle_Potion();
            }
            else
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("경화점액 제작에 필요한 재료가 부족합니다.");
                return;
            }

        }

        else if (is_Research_Levelup_Book)
        {
            if (Base_Manager.Data.Item_Holder["Research_Book_Stone"].Hero_Card_Amount >= 100 && Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount >= 3000)
            {
                Base_Manager.Data.Item_Holder["Research_Book_Stone"].Hero_Card_Amount -= 100;
                Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount -= 3000;
                var item = Base_Manager.Data.Data_Item_Dictionary["Research_Levelup_Book"];
                Base_Manager.Inventory.Get_Item(item, 1);
                Base_Canvas.instance.Get_Toast_Popup().Initialize("연구 비급서 제작 성공");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");
                Set_Research_Levelup_Book();

            }
            else
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("연구 비급서 제작에 필요한 재료가 부족합니다.");
                return;
            }

        }
    }


    public override void DisableOBJ()
    {
        _ = Base_Manager.BACKEND.WriteData();
        base.DisableOBJ();
    }
}
