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
    private Image Main_Exp_Silder;
    [SerializeField]
    private TextMeshProUGUI Exp_Text, ATK_Text, HP_Text, Gold_Text, Get_Exp_Text;
    [SerializeField]
    private GameObject Auto_Levelup_Stop_Button;

    private bool isPush = false;
    private bool isLongPush = false;
    private float holdDuration = 0f;
    private float repeatTimer = 0f;

    private bool notified_3 = false;
    private bool notified_2 = false;
    private bool notified_1 = false;

    private Coroutine levelUpCoroutine;
    public static Action Pressed_Levelup_Button_Tutorial;

    private void Start()
    {
        Auto_Levelup_Stop_Button.gameObject.SetActive(false);
    }

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

        // 안내 메시지
        if (holdDuration >= 7f && !notified_3)
        {
            Base_Canvas.instance.Get_MainGame_Error_UI().Initialize("계속 유지할 시, 3초 뒤에 모든 골드를 소비할 때 까지, 자동레벨업 합니다.");
            notified_3 = true;
        }
        else if (holdDuration >= 8f && !notified_2)
        {
            Base_Canvas.instance.Get_MainGame_Error_UI().Initialize("계속 유지할 시, 2초 뒤에 모든 골드를 소비할 때 까지, 자동레벨업 합니다.");
            notified_2 = true;
        }
        else if (holdDuration >= 9f && !notified_1)
        {
            Base_Canvas.instance.Get_MainGame_Error_UI().Initialize("계속 유지할 시, 1초 뒤에 모든 골드를 소비할 때 까지, 자동레벨업 합니다.");
            notified_1 = true;
        }

        // 10초 이상 유지 시 모든 골드 소모하여 즉시 레벨업
        if (holdDuration >= 10f && !isLongPush)
        {
            isLongPush = true;
            LevelUpAllAtOnce();
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
        notified_1 = false;
        notified_2 = false;
        notified_3 = false;
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
        Debug.Log($"{unitCost}의 1회 클릭당 드는 골드");
        double unitExp = Utils.Data.levelData.Get_EXP();             // 1회 클릭당 증가하는 EXP
        Debug.Log($"{unitExp}의 1회 클릭당 증가하는 EXP");
        double maxExp = Utils.Data.levelData.Get_MAXEXP();           // 전체 EXP (100%)
        Debug.Log($"{maxExp}의 전체 EXP");

        double totalCost = unitCost * (maxExp / unitExp);            // 총 필요 골드
        Debug.Log($"{totalCost}의 레벨업 필요 골드");

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

        Main_UI.Instance.Main_UI_PlayerInfo_Text_Check();
        transform.DORewind();
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.25f);
    }

    private void LevelUpAllAtOnce()
    {
        if (Stage_Manager.M_State == Stage_State.Dead)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("사망 상태에서는, 레벨업이 불가합니다.");
            return;
        }

        if (levelUpCoroutine != null)
        {
            StopCoroutine(levelUpCoroutine);
        }

        Base_Canvas.instance.Get_MainGame_Error_UI().Initialize("자동 레벨업을 시작합니다. 중단하려면 레벨업 중단 버튼을 누르세요. ");
        Base_Manager.Player.isAutoLeveling = true;

        levelUpCoroutine = StartCoroutine(LevelUpAllCoroutine());
        Auto_Levelup_Stop_Button.gameObject.SetActive(true);
    }

    private IEnumerator LevelUpAllCoroutine()
    {
        int levelsGained = 0;

        while (true)
        {
            double unitCost = Utils.Data.levelData.Get_LEVELUP_MONEY();
            double maxExp = Utils.Data.levelData.Get_MAXEXP();
            double totalCost = unitCost * maxExp;

            if (Data_Manager.Main_Players_Data.Player_Money < totalCost)
                break;

            Data_Manager.Main_Players_Data.Player_Money -= totalCost;
            Data_Manager.Main_Players_Data.Levelup++;
            Data_Manager.Main_Players_Data.Player_Level++;

            // ATK/HP 갱신
            Data_Manager.Main_Players_Data.ATK = Utils.Data.levelData.Get_Levelup_Next_ATK();
            Data_Manager.Main_Players_Data.HP = Utils.Data.levelData.Get_Levelup_Next_HP();

            levelsGained++;
            yield return null; // 프레임 분산
        }

        if (levelsGained > 0)
        {
            Main_UI.Instance.Main_UI_PlayerInfo_Text_Check();
            transform.DORewind();
            transform.DOPunchScale(new Vector3(0.4f, 0.4f, 0.4f), 0.4f);
        }
        else
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("레벨업 가능한 골드가 없습니다.");
        }

        levelUpCoroutine = null;
    }

    public void StopAutoLevelUp()
    {
        if (levelUpCoroutine != null)
        {
            StopCoroutine(levelUpCoroutine);
            levelUpCoroutine = null;
            Base_Manager.Player.isAutoLeveling = false;
            Base_Canvas.instance.Get_MainGame_Error_UI().Initialize("자동 레벨업을 수동으로 중단했습니다.");
            Auto_Levelup_Stop_Button.gameObject.SetActive(false);
        }

        Auto_Levelup_Stop_Button.gameObject.SetActive(false);
    }
}
