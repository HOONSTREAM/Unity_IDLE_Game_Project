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

    private const int HONDON_ARMOR_DIAMOND_PRICE = 15000;

    public override bool Init()
    {
        Player_Diamond.text = Data_Manager.Main_Players_Data.DiaMond.ToString();
        _hondon_sword_sold_out_OBJ.gameObject.SetActive(false);
        _hondon_Armor_sold_out_OBJ.gameObject.SetActive(false);
        Check_Buy_Hondon_Hall_Items();

        return base.Init();
    }

    public void Purchase(string purchase_name)
    {
        Base_Manager.IAP.Purchase(purchase_name, () =>
        {
            StartCoroutine(Init_Delay_Coroutine());
        });
    }

    private void Check_Buy_Hondon_Hall_Items()
    {
        if (Data_Manager.Main_Players_Data.isBuy_Hondon_Sword)
        {
            _hondon_sword_sold_out_OBJ.gameObject.SetActive(true);
        }
        if (Data_Manager.Main_Players_Data.isBuy_Hondon_Armor)
        {
            _hondon_Armor_sold_out_OBJ.gameObject.SetActive(true);
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
