using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Status : UI_Base
{

    public Status_Type status_type;
    [SerializeField]
    private RectTransform Bar; // 인벤토리의 메뉴 (전체아이템, 장비, 소비, 기타)   
    [SerializeField]
    private RectTransform Top_Content;
    [SerializeField]
    private Button[] Status_Bottom_Buttons;
    [SerializeField]
    private GameObject[] Panel_Objs;

    [SerializeField]
    private TextMeshProUGUI Player_Level_Text,User_NickName,Grade_Title,Ability, ATK, HP;
    [SerializeField]
    private TextMeshProUGUI GoldDrop, ItemDrop, Atk_Speed, Critical, Cri_Damage;

    public override bool Init()
    {
        Ability.text = Base_Manager.Player.Player_ALL_Ability_ATK_HP().ToString();
        Player_Level_Text.text = "LV." + (Data_Manager.Main_Players_Data.Player_Level + 1).ToString();
        ATK.text = StringMethod.ToCurrencyString(Base_Manager.Player.Calculate_Player_ATK());
        HP.text = StringMethod.ToCurrencyString(Base_Manager.Player.Calculate_Player_HP());
        GoldDrop.text = string.Format("{0:0}%", Base_Manager.Player.Calculate_Gold_Drop_Percentage());
        ItemDrop.text = string.Format("{0:0}%", Base_Manager.Player.Calculate_Item_Drop_Percentage());
        Atk_Speed.text = string.Format("{0:0.0}%", Base_Manager.Player.Calculate_Atk_Speed_Percentage());
        Critical.text = string.Format("{0:0.0}%", Base_Manager.Player.Calculate_Critical_Percentage());
        Cri_Damage.text = string.Format("{0:0.0}%", Base_Manager.Player.Calculate_Cri_Damage_Percentage());

        for (int i = 0; i< Status_Bottom_Buttons.Length; i++)
        {
            int index = i;

            Status_Bottom_Buttons[index].onClick.RemoveAllListeners();
            Status_Bottom_Buttons[index].onClick.AddListener(() => Status_Menu_Check((Status_Type)index));
        }

        return base.Init();
    }

    /// <summary>
    /// 유저가 원하는 메뉴를 누르면 바가 움직이는 기능을 구현합니다.
    /// </summary>
    /// <returns></returns>
    IEnumerator Bar_Movement_Coroutine(Vector2 endPos) //float endXPos)
    {
        float current = 0;
        float percent = 0;
        Vector2 start = Bar.anchoredPosition;
        Vector2 end = new Vector2(endPos.x, Top_Content.anchoredPosition.y);


        float startX = Bar.sizeDelta.x;
        //float endX = endXPos;

        while (percent < 1)
        {

            current += Time.deltaTime;
            percent = current / 0.1f;
            Vector2 LerpPos = Vector2.Lerp(start, end, percent);
            // float LerpPosX = Mathf.Lerp(startX, endX, percent);

            Bar.anchoredPosition = LerpPos;

            //Bar.sizeDelta = new Vector2(LerpPosX, Bar.sizeDelta.y);

            yield return null;
        }
    }

    public void Status_Menu_Check(Status_Type state)
    {
        for(int i = 0; i<Panel_Objs.Length; i++)
        {
            Panel_Objs[i].gameObject.SetActive(false);
        }

        Panel_Objs[(int)state].gameObject.SetActive(true);

        status_type = state;
        StartCoroutine(Bar_Movement_Coroutine(Status_Bottom_Buttons[(int)state].GetComponent<RectTransform>().anchoredPosition));       
    }

    public override void DisableOBJ()
    {
        Main_UI.Instance.Layer_Check(-1); // 버튼을 다시 원래 크기로 되돌립니다.

        Main_UI.Instance.FadeInOut(false, true, () =>
        {
            Main_UI.Instance.FadeInOut(true, false, null);
            base.DisableOBJ();
        });

    }
}
