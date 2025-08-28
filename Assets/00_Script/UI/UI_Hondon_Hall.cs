using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Hondon_Hall : UI_Base
{
    [SerializeField]
    private GameObject _hondon_sword_sold_out_OBJ;
    [SerializeField]
    private GameObject _hondon_Armor_sold_out_OBJ;
    [SerializeField]
    private TextMeshProUGUI Player_Diamond;

    [SerializeField]
    private TextMeshProUGUI Player_Additional_STR;
    [SerializeField]
    private TextMeshProUGUI Player_Additional_DEX;
    [SerializeField]
    private TextMeshProUGUI Player_Additional_ATK;
    [SerializeField]
    private TextMeshProUGUI Player_Additional_HP;

    private int Additional_STR = 0;
    private int Additional_DEX = 0;


    private const int HONDON_ARMOR_DIAMOND_PRICE = 15000;

    public override bool Init()
    {
        Player_Diamond.text = Data_Manager.Main_Players_Data.DiaMond.ToString(); // �÷��̾� ���̾� ������

        Init_Stat_Text();          
        Check_Buy_Hondon_Hall_Items();

        return base.Init();
    }

    /// <summary>
    /// ���� ������ �ؽ�Ʈ�� �ʱ�ȭ�մϴ�.
    /// </summary>
    private void Init_Stat_Text()
    {
        Player_Additional_STR.text = "0";
        Player_Additional_DEX.text = "0";
        Player_Additional_ATK.text = "������ ����";
        Player_Additional_HP.text = "������ ����";

        _hondon_sword_sold_out_OBJ.gameObject.SetActive(false);
        _hondon_Armor_sold_out_OBJ.gameObject.SetActive(false);
    }

    public void Purchase(string purchase_name)
    {
        Base_Manager.IAP.Purchase(purchase_name, () =>
        {
            StartCoroutine(Init_Delay_Coroutine());
        });
    }

    /// <summary>
    /// ȥ���� ���� ���� ���� ���θ� üũ�Ͽ�, �ؽ�Ʈ�� ������Ʈ�մϴ�. (������ �������� X), Player_Manager���� �Ҹ��� üũ�Ͽ� ���� ��������
    /// </summary>
    private void Check_Buy_Hondon_Hall_Items()
    {
        if (Data_Manager.Main_Players_Data.isBuy_Hondon_Sword)
        {
            _hondon_sword_sold_out_OBJ.gameObject.SetActive(true);
            Additional_STR += 500;
            Player_Additional_STR.text = Additional_STR.ToString();
            Player_Additional_ATK.text = "2�� ����";
        }
        if (Data_Manager.Main_Players_Data.isBuy_Hondon_Armor)
        {
            _hondon_Armor_sold_out_OBJ.gameObject.SetActive(true);
            Additional_DEX += 500;
            Player_Additional_DEX.text = Additional_DEX.ToString();
            Player_Additional_HP.text = "2�� ����";
        }
    }

    
    /// <summary>
    /// ���̾Ƹ��� ȥ���� ���� ����
    /// </summary>
    public void Buy_Hondon_Armor()
    {
        if(Data_Manager.Main_Players_Data.DiaMond < HONDON_ARMOR_DIAMOND_PRICE)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("���̾Ƹ�尡 �����մϴ�.");
            return;
        }

        Base_Canvas.instance.Get_MainGame_Error_UI().Initialize("ȥ���� ���� ���Ű� �Ϸ�Ǿ����ϴ�. �ɷ�ġ�� ��� ����˴ϴ�.");
        Base_Manager.SOUND.Play(Sound.BGS,"Gacha");
        Data_Manager.Main_Players_Data.DiaMond -= HONDON_ARMOR_DIAMOND_PRICE;
        Data_Manager.Main_Players_Data.isBuy_Hondon_Armor = true;
        Init();
        _ = Base_Manager.BACKEND.WriteData();
    }

    private IEnumerator Init_Delay_Coroutine()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        //TODO : START ���� �� 1���� ���� ��� ����
        Init();
    }

    public override void DisableOBJ()
    {
        base.DisableOBJ();
    }
}
