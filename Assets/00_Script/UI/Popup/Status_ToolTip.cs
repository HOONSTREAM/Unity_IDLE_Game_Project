using AssetKits.ParticleImage;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Status_ToolTip : MonoBehaviour
{

    private RectTransform Rect;
    [SerializeField]
    private Image Item_Image;
    [SerializeField]
    private TextMeshProUGUI Item_Name_Text, Rarity_Text, Item_Level_Text, Item_Position_Text, Item_Enhance_Level_Text;
    [SerializeField]
    private TextMeshProUGUI Base_ATK, Base_HP, Base_STR, Base_DEX, Base_VIT;
    [SerializeField]
    private TextMeshProUGUI Additional_ATK, Additional_HP, Additional_STR, Additional_DEX, Additional_VIT;   
    [SerializeField]
    private TextMeshProUGUI Weapon_Description;
    [SerializeField]
    private ParticleImage[] Legendary_Particle;

    private GameObject ToolTip_Background;
    private Status_Item_Scriptable Selected_Status_Item;

    private string start_percent;
    private string effect_percent = default;

    private void Awake()
    {
        Rect = this.GetComponent<RectTransform>();
        
    }

    private void Start()
    {
        int tooltipIndex = transform.GetSiblingIndex();
        ToolTip_Background = Instantiate(Resources.Load<GameObject>("UI/ToolTip_Background"), Base_Canvas.instance.transform);
        ToolTip_Background.transform.SetSiblingIndex(tooltipIndex - 1);
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 localMousePos;
            // ScreenPointToLocalPointInRectangle
            // UI�ý��ۿ��� ��ũ�� ��ǥ�� Ư�� UI���
            // (RectTransform{UI ���ο��� ��ġ�� �����ϴ� �ٽ� �Ӽ�!})�� ������ǥ�� ��ȯ�ϴ� ����� ����.
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)transform, Input.mousePosition, null, out localMousePos);

            // ���� ���θ� Ŭ���� ��� �ƹ��͵� �� ��
            if (((RectTransform)transform).rect.Contains(localMousePos))
                return;

        }

    }

    public void Exit_Tooltip()
    {
        Base_Canvas.instance.item_tooltip = null;
        Destroy(ToolTip_Background);
        Destroy(gameObject);
        _ = Base_Manager.BACKEND.WriteData();
    }

    public void Show_Status_Item_ToolTip(Vector2 screenPos, Status_Item_Scriptable status_item)
    {
        Selected_Status_Item = status_item;
        RectTransform canvasRect = Base_Canvas.instance.GetComponent<RectTransform>();

        // �ǹ��� (0.5, 0.5) ����
        Rect.pivot = new Vector2(0.5f, 0.5f);

        // ��� ��ġ�� ����
        Rect.anchoredPosition = Vector2.zero;

        Calculate_Status_Item_Stat(status_item);
    }

    /// <summary>
    /// ������� ���� �� �ɷ�ġ�� �����Ͽ� ��Ÿ���ϴ�.
    /// </summary>
    /// <param name="status_item"></param>
    private void Calculate_Status_Item_Stat(Status_Item_Scriptable status_item)
    {
        Item_Image.sprite = Utils.Get_Atlas(status_item.name);
        Item_Name_Text.text = Utils.String_Color_Rarity(status_item.rarity) + status_item.Item_Name;
        Rarity_Text.text = Utils.String_Color_Rarity(status_item.rarity) + status_item.KO_rarity.ToString();
        Item_Level_Text.text = Base_Manager.Data.Status_Item_Holder[status_item.name].Item_Level.ToString();
        Item_Position_Text.text = status_item.Position.ToString();
        Item_Enhance_Level_Text.text = $"{Utils.String_Color_Rarity(status_item.rarity) + "+"} {Utils.String_Color_Rarity(status_item.rarity) + Base_Manager.Data.Status_Item_Holder[status_item.name].Enhancement.ToString()}";

        Base_ATK.text = $"���ݷ� : {StringMethod.ToCurrencyString(status_item.Base_ATK)}";
        Base_HP.text = $"ü�� : {StringMethod.ToCurrencyString(status_item.Base_HP)}";
        Base_STR.text = $"STR : {status_item.Base_STR.ToString()}";
        Base_DEX.text = $"DEX : {status_item.Base_DEX.ToString()}";
        Base_VIT.text = $"VIT : {status_item.Base_VIT.ToString()}";

        Additional_ATK.text = $"�߰� ���ݷ� : {StringMethod.ToCurrencyString(Base_Manager.Data.Status_Item_Holder[status_item.name].Additional_ATK)}";
        Additional_HP.text = $"�߰� ü�� : {StringMethod.ToCurrencyString(Base_Manager.Data.Status_Item_Holder[status_item.name].Additional_HP)}";
        Additional_STR.text = $"�߰� STR : {Base_Manager.Data.Status_Item_Holder[status_item.name].Additional_STR.ToString()}";
        Additional_DEX.text = $"�߰� DEX : {Base_Manager.Data.Status_Item_Holder[status_item.name].Additional_DEX.ToString()}";
        Additional_VIT.text = $"�߰� VIT : {Base_Manager.Data.Status_Item_Holder[status_item.name].Additional_VIT.ToString()}";

        Weapon_Description.text = status_item.Item_Description;

        switch (status_item.rarity)
        {
            case Rarity.Common:
                Legendary_Particle[0].gameObject.SetActive(true);
                this.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.4f);
                break;
            case Rarity.UnCommon:
                Legendary_Particle[0].gameObject.SetActive(false);
                Legendary_Particle[1].gameObject.SetActive(true);
                this.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0f, 1f, 0f, 0.4f);
                break;
            case Rarity.Rare:
                Legendary_Particle[0].gameObject.SetActive(false);
                Legendary_Particle[1].gameObject.SetActive(false);
                Legendary_Particle[2].gameObject.SetActive(true);
                this.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0f, 0f, 1f, 0.4f);
                break;
            case Rarity.Epic:
                Legendary_Particle[0].gameObject.SetActive(false);
                Legendary_Particle[1].gameObject.SetActive(false);
                Legendary_Particle[2].gameObject.SetActive(false);
                Legendary_Particle[3].gameObject.SetActive(true);
                this.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0.5f, 0f, 0.5f, 0.4f);
                break;
            case Rarity.Legendary:
                Legendary_Particle[0].gameObject.SetActive(false);
                Legendary_Particle[1].gameObject.SetActive(false);
                Legendary_Particle[2].gameObject.SetActive(false);
                Legendary_Particle[3].gameObject.SetActive(false);
                Legendary_Particle[4].gameObject.SetActive(true);
                this.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1f, 1f, 0f, 0.4f);
                break;
            case Rarity.Chaos:
                Legendary_Particle[0].gameObject.SetActive(false);
                Legendary_Particle[1].gameObject.SetActive(false);
                Legendary_Particle[2].gameObject.SetActive(false);
                Legendary_Particle[3].gameObject.SetActive(false);
                Legendary_Particle[4].gameObject.SetActive(false);
                Legendary_Particle[5].gameObject.SetActive(true);
                this.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1f, 0f, 0f, 0.4f);
                break;

        }
    }

    public void Increase_Rarity()
    {
        if (Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount < 2000)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("������ ���� ö�� �����մϴ�.");
            return;
        }

        if(Selected_Status_Item != null && Base_Manager.Data.Status_Item_Holder[Selected_Status_Item.name].Item_Amount == 1)
        {
            if (Base_Manager.Data.Status_Item_Holder[Selected_Status_Item.name].Enhancement < 3)
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("��ȭ��ġ 3 �̻� ������ �����մϴ�.");
                return;
            }

            if((Selected_Status_Item.rarity == Rarity.Chaos))
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("�ְ� ����Դϴ�.");
                return;
            }

            Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount -= 2000;

            #region ����

            if (Selected_Status_Item.rarity == Rarity.Common && Selected_Status_Item.Position == "����")
            {
                Base_Manager.Data.Status_Item_Holder[Selected_Status_Item.name].Item_Amount = 0;
                Base_Manager.Data.Status_Item_Holder["Weapon_2"].Item_Amount = 1;
                Destroy(this.gameObject);
                Destroy(ToolTip_Background);
                GameObject.Find("@Status").gameObject.GetComponent<UI_Status>().Init();

                Base_Canvas.instance.Get_Toast_Popup().Initialize("������� ���� ����Ϸ�");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

            }

            if (Selected_Status_Item.rarity == Rarity.UnCommon && Selected_Status_Item.Position == "����")
            {
                Base_Manager.Data.Status_Item_Holder[Selected_Status_Item.name].Item_Amount = 0;
                Base_Manager.Data.Status_Item_Holder["Weapon_3"].Item_Amount = 1;
                Destroy(this.gameObject);
                Destroy(ToolTip_Background);
                GameObject.Find("@Status").gameObject.GetComponent<UI_Status>().Init();

                Base_Canvas.instance.Get_Toast_Popup().Initialize("������� ���� ����Ϸ�");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

            }

            if (Selected_Status_Item.rarity == Rarity.Rare && Selected_Status_Item.Position == "����")
            {
                Base_Manager.Data.Status_Item_Holder[Selected_Status_Item.name].Item_Amount = 0;
                Base_Manager.Data.Status_Item_Holder["Weapon_4"].Item_Amount = 1;
                Destroy(this.gameObject);
                Destroy(ToolTip_Background);
                GameObject.Find("@Status").gameObject.GetComponent<UI_Status>().Init();

                Base_Canvas.instance.Get_Toast_Popup().Initialize("������� ���� ����Ϸ�");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

            }

            if (Selected_Status_Item.rarity == Rarity.Epic && Selected_Status_Item.Position == "����")
            {
                Base_Manager.Data.Status_Item_Holder[Selected_Status_Item.name].Item_Amount = 0;
                Base_Manager.Data.Status_Item_Holder["Weapon_5"].Item_Amount = 1;
                Destroy(this.gameObject);
                Destroy(ToolTip_Background);
                GameObject.Find("@Status").gameObject.GetComponent<UI_Status>().Init();

                Base_Canvas.instance.Get_Toast_Popup().Initialize("������� ���� ����Ϸ�");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

            }

            if (Selected_Status_Item.rarity == Rarity.Legendary && Selected_Status_Item.Position == "����")
            {
                Base_Manager.Data.Status_Item_Holder[Selected_Status_Item.name].Item_Amount = 0;
                Base_Manager.Data.Status_Item_Holder["Weapon_6"].Item_Amount = 1;
                Destroy(this.gameObject);
                Destroy(ToolTip_Background);
                GameObject.Find("@Status").gameObject.GetComponent<UI_Status>().Init();

                Base_Canvas.instance.Get_Toast_Popup().Initialize("������� ���� ����Ϸ�");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

            }

            #endregion

            #region �Ǽ��縮
            if (Selected_Status_Item.rarity == Rarity.Common && Selected_Status_Item.Position == "�Ǽ��縮")
            {
                Base_Manager.Data.Status_Item_Holder[Selected_Status_Item.name].Item_Amount = 0;
                Base_Manager.Data.Status_Item_Holder["Ring_2"].Item_Amount = 1;
                Destroy(this.gameObject);
                Destroy(ToolTip_Background);
                GameObject.Find("@Status").gameObject.GetComponent<UI_Status>().Init();

                Base_Canvas.instance.Get_Toast_Popup().Initialize("������� ���� ����Ϸ�");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

            }
            if (Selected_Status_Item.rarity == Rarity.UnCommon && Selected_Status_Item.Position == "�Ǽ��縮")
            {
                Base_Manager.Data.Status_Item_Holder[Selected_Status_Item.name].Item_Amount = 0;
                Base_Manager.Data.Status_Item_Holder["Ring_3"].Item_Amount = 1;
                Destroy(this.gameObject);
                Destroy(ToolTip_Background);
                GameObject.Find("@Status").gameObject.GetComponent<UI_Status>().Init();

                Base_Canvas.instance.Get_Toast_Popup().Initialize("������� ���� ����Ϸ�");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

            }
            if (Selected_Status_Item.rarity == Rarity.Rare && Selected_Status_Item.Position == "�Ǽ��縮")
            {
                Base_Manager.Data.Status_Item_Holder[Selected_Status_Item.name].Item_Amount = 0;
                Base_Manager.Data.Status_Item_Holder["Ring_4"].Item_Amount = 1;
                Destroy(this.gameObject);
                Destroy(ToolTip_Background);
                GameObject.Find("@Status").gameObject.GetComponent<UI_Status>().Init();

                Base_Canvas.instance.Get_Toast_Popup().Initialize("������� ���� ����Ϸ�");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

            }
            if (Selected_Status_Item.rarity == Rarity.Epic && Selected_Status_Item.Position == "�Ǽ��縮")
            {
                Base_Manager.Data.Status_Item_Holder[Selected_Status_Item.name].Item_Amount = 0;
                Base_Manager.Data.Status_Item_Holder["Ring_5"].Item_Amount = 1;
                Destroy(this.gameObject);
                Destroy(ToolTip_Background);
                GameObject.Find("@Status").gameObject.GetComponent<UI_Status>().Init();

                Base_Canvas.instance.Get_Toast_Popup().Initialize("������� ���� ����Ϸ�");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

            }
            if (Selected_Status_Item.rarity == Rarity.Legendary && Selected_Status_Item.Position == "�Ǽ��縮")
            {
                Base_Manager.Data.Status_Item_Holder[Selected_Status_Item.name].Item_Amount = 0;
                Base_Manager.Data.Status_Item_Holder["Ring_6"].Item_Amount = 1;
                Destroy(this.gameObject);
                Destroy(ToolTip_Background);
                GameObject.Find("@Status").gameObject.GetComponent<UI_Status>().Init();

                Base_Canvas.instance.Get_Toast_Popup().Initialize("������� ���� ����Ϸ�");
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

            }
            #endregion

        }

    }

    public void Increase_Level()
    {
        if (Selected_Status_Item != null)
        {
            var holder = Base_Manager.Data.Status_Item_Holder[Selected_Status_Item.name];

            if ((Selected_Status_Item.Position == "����" || Selected_Status_Item.Position == "�Ǽ��縮") && holder.Item_Amount == 1)
            {
                int playerLevel = Data_Manager.Main_Players_Data.Player_Level;
                int maxAllowedItemLevel = (playerLevel / 1000) * 3;

                if (holder.Item_Level >= maxAllowedItemLevel)
                {
                    Base_Canvas.instance.Get_Toast_Popup().Initialize($"�÷��̾� ������ �����մϴ�.");                    
                    return;
                }

                holder.Item_Level++;

                int bonus = 0;
                switch (Selected_Status_Item.rarity)
                {
                    case Rarity.Common: bonus = 5000; break;
                    case Rarity.UnCommon: bonus = 20000; break;
                    case Rarity.Rare: bonus = 50000; break;
                    case Rarity.Epic: bonus = 100000; break;
                    case Rarity.Legendary: bonus = 200000; break;
                    case Rarity.Chaos: bonus = 500000; break;
                }

                holder.Additional_ATK += bonus;
                holder.Additional_HP += bonus;

                Base_Canvas.instance.Get_Toast_Popup().Initialize("������� ������ ����Ϸ�");
                Calculate_Status_Item_Stat(Selected_Status_Item);
                Base_Manager.SOUND.Play(Sound.BGS, "Gacha");                
                GameObject.Find("@Status").GetComponent<UI_Status>().Init();
            }
        }

    }

    public void Increase_Enhancement()
    {
        if (Selected_Status_Item == null) return;

        var holder = Base_Manager.Data.Status_Item_Holder[Selected_Status_Item.name];
        if (holder.Item_Amount != 1) return;

        float[] enhancementRates = new float[]
        {
        100f, 35f, 15.5f, 9f, 5f,
        1f, 0.5f, 0.3f, 0.1f, 0.05f
        };

        int currentEnhance = holder.Enhancement;

        if (currentEnhance >= enhancementRates.Length)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("�̹� �ִ� ��ȭ ��ġ�Դϴ�.");
            return;
        }

        if(Base_Manager.Data.Item_Holder["Enhancement"].Hero_Card_Amount < 1)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("�̽����� �����մϴ�.");
            return;
        }
        Base_Manager.Data.Item_Holder["Enhancement"].Hero_Card_Amount--;
        float successRate = enhancementRates[currentEnhance];
        float BonusRate = Base_Manager.Data.Item_Holder["Bonus_Enhancement"].Hero_Card_Amount >= 1 ? 10.0f : 0.0f;
        float rand = UnityEngine.Random.Range(0f, 100f);

        Debug.Log($"{successRate}�� �⺻����Ȯ���� {BonusRate}�� �߰� Ȯ���� ������.");

        if (rand < successRate + BonusRate)
        {
            
            if(Base_Manager.Data.Item_Holder["Bonus_Enhancement"].Hero_Card_Amount >= 1)
            {
                Base_Manager.Data.Item_Holder["Bonus_Enhancement"].Hero_Card_Amount--;
            }

            holder.Enhancement++;

            int bonus = GetEnhanceBonus(holder.Enhancement, Selected_Status_Item);
            holder.Additional_STR += bonus;
            holder.Additional_DEX += bonus;
            holder.Additional_VIT += bonus;

            Base_Canvas.instance.Get_Toast_Popup().Initialize($"����! +{holder.Enhancement} ��ȭ�Ǿ����ϴ�.");
            Calculate_Status_Item_Stat(Selected_Status_Item);
            Base_Manager.SOUND.Play(Sound.BGS, "Success");
        }
        else
        {
            if (Base_Manager.Data.Item_Holder["DEF_Enhancement"].Hero_Card_Amount >= 1)
            {
                if (Base_Manager.Data.Item_Holder["Bonus_Enhancement"].Hero_Card_Amount >= 1)
                {
                    Base_Manager.Data.Item_Holder["Bonus_Enhancement"].Hero_Card_Amount--;
                }

                Base_Canvas.instance.Get_Toast_Popup().Initialize("��ȭ ��ȣ���� �Ҹ��Ͽ� �϶��� �����ϴ�.");
                Calculate_Status_Item_Stat(Selected_Status_Item);
                Base_Manager.Data.Item_Holder["DEF_Enhancement"].Hero_Card_Amount--;
                Base_Manager.SOUND.Play(Sound.BGS, "Lose");
            }
            else
            {
                if (Base_Manager.Data.Item_Holder["Bonus_Enhancement"].Hero_Card_Amount >= 1)
                {
                    Base_Manager.Data.Item_Holder["Bonus_Enhancement"].Hero_Card_Amount--;
                }

                int previousEnhance = holder.Enhancement;
                holder.Enhancement = Mathf.Max(0, holder.Enhancement - 1);

                // ���� ��ȭ ��ġ�� ���� ���ʽ��� ����
                int lostBonus = GetEnhanceBonus(previousEnhance, Selected_Status_Item);
                holder.Additional_STR -= lostBonus;
                holder.Additional_DEX -= lostBonus;
                holder.Additional_VIT -= lostBonus;

                // ���Ѽ� ��ȣ
                holder.Additional_STR = Mathf.Max(0, (float)holder.Additional_STR);
                holder.Additional_DEX = Mathf.Max(0, (float)holder.Additional_DEX);
                holder.Additional_VIT = Mathf.Max(0, (float)holder.Additional_VIT);

                Base_Canvas.instance.Get_Toast_Popup().Initialize("��ȭ�� �����߽��ϴ�. ��ȭ ��ġ�� �϶��մϴ�.");
                Calculate_Status_Item_Stat(Selected_Status_Item);
                Base_Manager.SOUND.Play(Sound.BGS, "Lose");
            }
            
        }
       
       
        GameObject.Find("@Status").GetComponent<UI_Status>().Init();
    }

    // ���ʽ� ��� ���� �Լ�
    private int GetEnhanceBonus(int enhancementLevel, Status_Item_Scriptable scriptable)
    {
        int rarityFactor = 1;
        switch (scriptable.rarity)
        {
            case Rarity.Common: rarityFactor = 1; break;
            case Rarity.UnCommon: rarityFactor = 2; break;
            case Rarity.Rare: rarityFactor = 4; break;
            case Rarity.Epic: rarityFactor = 5; break;
            case Rarity.Legendary: rarityFactor = 8; break;
            case Rarity.Chaos: rarityFactor = 15; break;
        }

        int multiplier = 0;
        switch (enhancementLevel)
        {
            case 1: multiplier = 1; break;
            case 2: multiplier = 2; break;
            case 3: multiplier = 4; break;
            case 4: multiplier = 8; break;
            case 5: multiplier = 16; break;
            case 6: multiplier = 32; break;
            case 7: multiplier = 64; break;
            case 8: multiplier = 128; break;
            case 9: multiplier = 240; break;
            case 10: multiplier = 400; break;
        }

        return rarityFactor * multiplier;
    }
}



