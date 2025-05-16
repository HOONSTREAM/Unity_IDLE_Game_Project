using AssetKits.ParticleImage;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hero_ToolTip : MonoBehaviour
{
    
    private RectTransform Rect;
    [SerializeField]
    private Image Hero_Image;
    [SerializeField]
    private TextMeshProUGUI Hero_Name_Text, Rarity_Text;
    [SerializeField]
    private ParticleImage Legendary_Particle;

    private string start_percent;
    private string effect_percent = default;

    private void Awake()
    {

        Rect = this.GetComponent<RectTransform>();
       
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

            // ���� �ٱ��� Ŭ���ϸ� ����
            Base_Canvas.instance.item_tooltip = null;
            Destroy(gameObject);
        }


    }

 
    public void Show_Hero_ToolTip(Character_Scriptable hero, Vector2 pos)
    {
        // ���� ��ġ ����
        Rect.anchoredPosition = pos;

        Debug.Log(hero.name);

        Hero_Image.sprite = Utils.Get_Atlas(hero.name);
        Hero_Name_Text.text = hero.M_Character_Name;
        Rarity_Text.text = Utils.String_Color_Rarity(hero.Rarity) + hero.KO_Rarity.ToString();


        if (hero.Rarity >= Rarity.Legendary)
        {
            Legendary_Particle.gameObject.SetActive(true);
        }

        else
        {
            Legendary_Particle.gameObject.SetActive(false);
        }
    }

  
}
