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
            // UI시스템에서 스크린 좌표를 특정 UI요소
            // (RectTransform{UI 내부에서 위치를 결정하는 핵심 속성!})의 로컬좌표로 변환하는 기능을 수행.
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)transform, Input.mousePosition, null, out localMousePos);

            // 툴팁 내부를 클릭한 경우 아무것도 안 함
            if (((RectTransform)transform).rect.Contains(localMousePos))
                return;

            // 툴팁 바깥을 클릭하면 제거
            Base_Canvas.instance.skill_tooltip = null;
            Destroy(gameObject);
        }


    }

    public void Show_Skill_ToolTip(Vector2 pos)
    {
        RectTransform canvasRect = Base_Canvas.instance.GetComponent<RectTransform>();

        // 툴팁의 크기 가져오기
        Vector2 tooltipSize = Rect.sizeDelta;

        // 캔버스의 크기 가져오기
        Vector2 canvasSize = canvasRect.sizeDelta;

        // 툴팁이 화면 밖으로 나가지 않도록 위치 조정
        float clampedX = Mathf.Clamp(Rect.anchoredPosition.x, -canvasSize.x / 2 + tooltipSize.x / 2, canvasSize.x / 2 - tooltipSize.x / 2);
        float clampedY = Mathf.Clamp(Rect.anchoredPosition.y, -canvasSize.y / 2 + tooltipSize.y / 2, canvasSize.y / 2 - tooltipSize.y / 2);

        // 조정된 위치 적용
        Rect.anchoredPosition = new Vector2(clampedX, clampedY);
    
        Heal_Amount.text = StringMethod.ToCurrencyString((GameObject.Find("Cleric").gameObject.GetComponent<Character>().ATK * (500.0f / 100.0f)));
    }

        
  
}
