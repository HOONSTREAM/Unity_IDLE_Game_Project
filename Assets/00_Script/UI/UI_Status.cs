using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Status : UI_Base
{

    public Status_Type status_type;
    [SerializeField]
    private RectTransform Bar; // �κ��丮�� �޴� (��ü������, ���, �Һ�, ��Ÿ)   
    [SerializeField]
    private RectTransform Top_Content;
    [SerializeField]
    private Button[] Status_Bottom_Buttons;

    [SerializeField]
    private TextMeshProUGUI Ability, ATK, HP;

    public override bool Init()
    {
        Ability.text = Base_Manager.Player.Player_ALL_Ability_ATK_HP().ToString();
       // ATK.text = StringMethod.ToCurrencyString(Base_Manager.Player.Get_ATK(Data.Rarity, Base_Manager.Data.character_Holder[Data.name]));
       // HP.text = StringMethod.ToCurrencyString(Base_Manager.Player.Get_HP(Data.Rarity, Base_Manager.Data.character_Holder[Data.name]));
        return base.Init();
    }

    

    /// <summary>
    /// ������ ���ϴ� �޴��� ������ �ٰ� �����̴� ����� �����մϴ�.
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
        status_type = state;
        StartCoroutine(Bar_Movement_Coroutine(Status_Bottom_Buttons[(int)state].GetComponent<RectTransform>().anchoredPosition));
        Init();
    }



    public override void DisableOBJ()
    {
        Main_UI.Instance.Layer_Check(-1); // ��ư�� �ٽ� ���� ũ��� �ǵ����ϴ�.

        Main_UI.Instance.FadeInOut(false, true, () =>
        {
            Main_UI.Instance.FadeInOut(true, false, null);
            base.DisableOBJ();
        });

    }
}
