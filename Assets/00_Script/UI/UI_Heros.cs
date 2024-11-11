using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 영웅 배치 창을 다루는 스크립트 입니다.
/// </summary>
public class UI_Heros : UI_Base
{
    public Transform Content;
    public GameObject Parts;
    public List<UI_Heros_Parts> hero_parts = new List<UI_Heros_Parts>();
    private Dictionary<string, Character_Scriptable> _dict = new Dictionary<string, Character_Scriptable>();

    public override bool Init()
    {

        InitButtons();
        Main_UI.Instance.FadeInOut(true, true, null);


        var Data = Base_Manager.Data.Data_Character_Dictionary;

        foreach (var data in Data)
        {
            _dict.Add(data.Value.Data.M_Character_Name, data.Value.Data);
        }
      

        var sort_dict = _dict.OrderByDescending(x => x.Value.Rarity);


        foreach (var data in sort_dict)
        {
            var Object = Instantiate(Parts, Content).GetComponent<UI_Heros_Parts>(); // Content를 부모오브젝트로 해서 Parts를 생성
            hero_parts.Add(Object);
            Object.Init(data.Value, this);
        }

        return base.Init();
    }

    public override void DisableOBJ()
    {
        Main_UI.Instance.FadeInOut(false, true, () =>
        {
            Main_UI.Instance.FadeInOut(true, false, null);
            base.DisableOBJ();
        });

    }

    /// <summary>
    /// 플레이어가, 영웅 창에서 특정 영웅을 터치했을때의 동작을 정의합니다.
    /// </summary>
    public void Set_Click(UI_Heros_Parts parts)
    {
        for(int i = 0; i < hero_parts.Count; i++)
        {
            hero_parts[i].Lock_OBJ.SetActive(true);
            hero_parts[i].GetComponent<Outline>().enabled = false;
        }

        parts.Lock_OBJ.SetActive(false);
        parts.GetComponent<Outline>().enabled = true;

    }

    /// <summary>
    /// 영웅을 클릭 후, 플러스 버튼이 생성되면, 영웅을 등록하기 위해, 플러스 버튼 위에 보이지 않는 가상의 버튼을 설정합니다.
    /// </summary>
    public void InitButtons()
    {
        for(int i = 0; i<Render_Manager.instance.HERO.Circles.Length; i++)
        {
            var go = new GameObject("Button").AddComponent<Button>();

            go.transform.SetParent(this.transform); // 해당 오브젝트를 UI_Heros 팝업 하단에 자식오브젝트로 설정
            go.gameObject.AddComponent<Image>();
            go.gameObject.AddComponent<RectTransform>();
            
            RectTransform rect = go.GetComponent<RectTransform>();

            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            go.transform.position = Render_Manager.instance.ReturnScreenPoint(Render_Manager.instance.HERO.Circles[i]);        
        }
        
    }

}
