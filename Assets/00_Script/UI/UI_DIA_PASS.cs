using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DIA_PASS : UI_Base
{

    [SerializeField] private Transform Content; // ScrollView > Content
    [SerializeField] private GameObject Start_Panel;
    [SerializeField] private GameObject Lock; // 패스가 만약 구매상태라면, 버튼 락

    private List<Transform> diaPanels = new List<Transform>();
    private const int PASS_REWARD_AMOUNT = 30;
    private const int DAILY_DIA_REWARD = 10000;

    private void Awake()
    {
        // Content 하위의 Dia_Panel 자동 등록
        diaPanels.Clear();
        for (int i = 0; i < Content.childCount; i++)
        {
            diaPanels.Add(Content.GetChild(i));
        }
    }

    public override bool Init()
    {
        Lock.gameObject.SetActive(false);
        //TODO : 몇일차 보상 진행중인지 점검, 보상 수령 여부 점검하여 보상 안받았으면 자동 수령
        if (Data_Manager.Main_Players_Data.isBUY_DIA_PASS)
        {
            Lock.gameObject.SetActive(true);
            Start_Panel.gameObject.transform.GetChild(2).gameObject.SetActive(true);

            ResetAttendanceIfNeeded();
            RefreshUI();
            Try_Dia_Attendance();
        }
        else
        {
            Start_Panel.gameObject.transform.GetChild(2).gameObject.SetActive(false);
            ResetAttendanceIfNeeded();
            RefreshUI();
        }

        return base.Init();
    }

    private void Try_Dia_Attendance()
    {
        var data = Data_Manager.Main_Players_Data;
        string today = Utils.Get_Server_Time().ToString("yyyy-MM-dd");

        if (Data_Manager.Main_Players_Data.isBUY_DIA_PASS)
        {
            if (data.DIA_PASS_Last_Date == today && data.Get_DIA_PASS_Reward)
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("이미 패스 보상을 수령했습니다.");
                return;
            }

            if (data.DIA_PASS_Last_Date != today || !data.Get_DIA_PASS_Reward)
            {
                data.DIA_PASS_ATTENDANCE_DAY++;

                if (data.DIA_PASS_ATTENDANCE_DAY > PASS_REWARD_AMOUNT)
                {
                    data.DIA_PASS_ATTENDANCE_DAY = 0;                    
                    Data_Manager.Main_Players_Data.isBUY_DIA_PASS = false; // 패스 초기화

                                                                           
                    return;
                }

                
                GiveReward(data.DIA_PASS_ATTENDANCE_DAY);
                data.DIA_PASS_Last_Date = today;
                data.Get_DIA_PASS_Reward = true;
                ResetAttendanceIfNeeded();
                RefreshUI();
                _ = Base_Manager.BACKEND.WriteData();
            }

        }

    }

    private void ResetAttendanceIfNeeded()
    {
        var data = Data_Manager.Main_Players_Data;
        string today = Utils.Get_Server_Time().ToString("yyyy-MM-dd");

        if (data.DIA_PASS_Last_Date != today)
        {
            data.Get_DIA_PASS_Reward = false;

            if (data.DIA_PASS_ATTENDANCE_DAY > PASS_REWARD_AMOUNT)
            {
                data.DIA_PASS_ATTENDANCE_DAY = 0;
                data.isBUY_DIA_PASS = false;
                data.DIA_PASS_Last_Date = "";
                data.Get_DIA_PASS_Reward = false;

                // 모든 Get 오브젝트 비활성화
                foreach (var panel in diaPanels)
                {
                    var getObj = panel.Find("Get")?.gameObject;
                    if (getObj != null)
                        getObj.SetActive(false);
                }
            }
        }
    }
    private void RefreshUI()
    {
        var data = Data_Manager.Main_Players_Data;
        int currentDay = data.DIA_PASS_ATTENDANCE_DAY;

        for (int i = 0; i < diaPanels.Count; i++)
        {
            var getObj = diaPanels[i].Find("Get")?.gameObject;
            if (getObj != null)
            {
                getObj.SetActive(i < currentDay);
            }
        }
       
    }

    private void GiveReward(int day)
    {
        int rewardAmount = DAILY_DIA_REWARD;

        Data_Manager.Main_Players_Data.DiaMond += DAILY_DIA_REWARD;
        Base_Manager.BACKEND.Log_Get_Dia($"DIA_PASS {rewardAmount},Day : {day}");
        Base_Manager.SOUND.Play(Sound.BGS, "Gacha");
        Base_Canvas.instance.Get_MainGame_Error_UI().Initialize($"{day}일차 패스 보상으로 다이아 {rewardAmount}개를 받았습니다!");
    }


    public override void DisableOBJ()
    {
        base.DisableOBJ();
    }

    public void Purchase(string purchase_name)
    {
        Base_Manager.IAP.Purchase(purchase_name, () =>
        {
            StartCoroutine(Init_Delay_Coroutine());
        });
    }

    private IEnumerator Init_Delay_Coroutine()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        //TODO : START 보상 및 1일차 보상 즉시 지급
        Init();
    }
}
