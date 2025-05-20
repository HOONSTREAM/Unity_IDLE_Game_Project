using AssetKits.ParticleImage;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Skill_ToolTip : MonoBehaviour
{
    
    private RectTransform Rect;
    [SerializeField]
    private Image Skill_Image;
    [SerializeField]
    private TextMeshProUGUI Skill_Name, Heal_Amount, Description_Text;
   

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
            Base_Canvas.instance.skill_tooltip = null;
            Destroy(gameObject);
        }


    }

    public void Show_Skill_ToolTip(Vector2 pos)
    {
        RectTransform canvasRect = Base_Canvas.instance.GetComponent<RectTransform>();

        // ������ ũ�� ��������
        Vector2 tooltipSize = Rect.sizeDelta;

        // ĵ������ ũ�� ��������
        Vector2 canvasSize = canvasRect.sizeDelta;

        // ������ ȭ�� ������ ������ �ʵ��� ��ġ ����
        float clampedX = Mathf.Clamp(Rect.anchoredPosition.x, -canvasSize.x / 2 + tooltipSize.x / 2, canvasSize.x / 2 - tooltipSize.x / 2);
        float clampedY = Mathf.Clamp(Rect.anchoredPosition.y, -canvasSize.y / 2 + tooltipSize.y / 2, canvasSize.y / 2 - tooltipSize.y / 2);

        // ������ ��ġ ����
        Rect.anchoredPosition = new Vector2(clampedX, clampedY);
    
        Heal_Amount.text = StringMethod.ToCurrencyString((GameObject.Find("Cleric").gameObject.GetComponent<Character>().ATK * (500.0f / 100.0f)));
    }

        
  
}
