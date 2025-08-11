using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Attendance : UI_Base
{
    [SerializeField] private Transform Content; // ScrollView > Content
    [SerializeField] private Button ClaimButton;

    private List<Transform> diaPanels = new List<Transform>();

    private void Awake()
    {
        // Content ������ Dia_Panel �ڵ� ���
        diaPanels.Clear();
        for (int i = 0; i < Content.childCount; i++)
        {
            diaPanels.Add(Content.GetChild(i));
        }
    }

    private void Start()
    {
        ClaimButton.onClick.AddListener(() =>
        {
            TryClaimAttendance();
            RefreshUI();
        });

        ResetAttendanceIfNeeded();
        RefreshUI();
    }

    private void RefreshUI()
    {
        var data = Data_Manager.Main_Players_Data;
        int currentDay = data.Attendance_Day;

        for (int i = 0; i < diaPanels.Count; i++)
        {
            var getObj = diaPanels[i].Find("Get")?.gameObject;
            if (getObj != null)
            {
                getObj.SetActive(i < currentDay);
            }
        }

        ClaimButton.interactable = !data.Get_Attendance_Reward;
    }

    private void TryClaimAttendance()
    {
        var data = Data_Manager.Main_Players_Data;
        string today = Utils.Get_Server_Time().ToString("yyyy-MM-dd");

        if (data.Attendance_Last_Date == today && data.Get_Attendance_Reward)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("�̹� �⼮ ������ �����߽��ϴ�.");
            return;
        }

        if (data.Attendance_Last_Date != today)
        {
            data.Attendance_Day++;
            if (data.Attendance_Day > 25)
                data.Attendance_Day = 1; // ��ȯ �⼮

            data.Attendance_Last_Date = today;
        }

        data.Get_Attendance_Reward = true;

        GiveReward(data.Attendance_Day);
        _=Base_Manager.BACKEND.WriteData();
    }

    private void GiveReward(int day)
    {
        int rewardAmount = (day == 5 || day == 10 || day == 15 || day == 20) ? 20000 :
                   (day == 25) ? 20000 : 1000;

        Base_Canvas.instance.Get_UI("UI_Reward");
        Utils.UI_Holder.Peek().GetComponent<UI_Reward>().GetRewardInit("Dia", rewardAmount);
        Base_Manager.BACKEND.Log_Get_Dia($"Attendance_Dia_{rewardAmount},Day : {day}");
        Base_Canvas.instance.Get_Toast_Popup().Initialize($"{day}���� �⼮ �������� ���̾� {rewardAmount}���� �޾ҽ��ϴ�!");        
    }

    private void ResetAttendanceIfNeeded()
    {
        var data = Data_Manager.Main_Players_Data;
        string today = Utils.Get_Server_Time().ToString("yyyy-MM-dd");

        if (data.Attendance_Last_Date != today)
        {
            data.Get_Attendance_Reward = false;

            if (data.Attendance_Day >= 25)
            {
                data.Attendance_Day = 0;

                // ��� Get ������Ʈ ��Ȱ��ȭ
                foreach (var panel in diaPanels)
                {
                    var getObj = panel.Find("Get")?.gameObject;
                    if (getObj != null)
                        getObj.SetActive(false);
                }
            }
        }
    }

    public override void DisableOBJ()
    {
        base.DisableOBJ();
    }

}
