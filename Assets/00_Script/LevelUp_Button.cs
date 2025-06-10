using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class LevelUp_Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField]
    private TextMeshProUGUI ATK_Text, HP_Text, Gold_Text;

    private bool isPush = false;
    private bool isLongPush = false;
    private float holdDuration = 0f;
    private float repeatTimer = 0f;

    public static Action Pressed_Levelup_Button_Tutorial;


    private void Update()
    {
        if (!isPush) return;

        holdDuration += Time.deltaTime;

        // 1초 이상 누르면 일정 간격으로 Exp_Up()
        if (holdDuration >= 1f && !isLongPush)
        {
            repeatTimer += Time.deltaTime;

            if (repeatTimer >= 0.01f)
            {
                repeatTimer = 0f;
                Exp_Up();
            }
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isLongPush)
        {
            isPush = false;
            isLongPush = false;
            return;
        }

        holdDuration = 0f;
        repeatTimer = 0f;     
        isPush = true;

        Exp_Up(); // 첫 클릭 1회 실행
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPush = false;
        isLongPush = false;
        holdDuration = 0f;
        repeatTimer = 0f;
    }

    public void Exp_Up()
    {       
        if (Utils.is_Tutorial)
        {
            Pressed_Levelup_Button_Tutorial?.Invoke();
        }

        if (Stage_Manager.M_State == Stage_State.Dead)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("사망 상태에서는, 레벨업이 불가합니다.");
            return;
        }

        
        ApplyLevelUp();
    }

    private void ApplyLevelUp()
    {
        double unitCost = Utils.Data.levelData.Get_LEVELUP_MONEY();  // 1회 클릭당 드는 골드
       
        double unitExp = Utils.Data.levelData.Get_EXP();             // 1회 클릭당 증가하는 EXP
        
        double maxExp = Utils.Data.levelData.Get_MAXEXP();           // 전체 EXP (100%)
        

        double totalCost = unitCost * (maxExp / unitExp);            // 총 필요 골드

        int Exp_Amount = (int)maxExp / (int)unitExp;

       

        if (Data_Manager.Main_Players_Data.Player_Money < totalCost)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("골드가 부족합니다.");
            return;
        }

        Data_Manager.Main_Players_Data.Player_Money -= totalCost;
        Data_Manager.Main_Players_Data.Levelup++;
        Data_Manager.Main_Players_Data.Player_Level++;

        // ATK / HP 재계산
        Data_Manager.Main_Players_Data.ATK = Utils.Data.levelData.Get_Levelup_Next_ATK();
        Data_Manager.Main_Players_Data.HP = Utils.Data.levelData.Get_Levelup_Next_HP();

        Data_Manager.Main_Players_Data.EXP_Upgrade_Count += Exp_Amount;

        Main_UI.Instance.Main_UI_PlayerInfo_Text_Check();
        transform.DORewind();
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.25f);
    }

}
