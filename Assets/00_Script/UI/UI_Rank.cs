using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UI_Rank : UI_Base
{
    [SerializeField]
    private TextMeshProUGUI[] Nick_Name;
    [SerializeField]
    private TextMeshProUGUI[] Stage;
    [SerializeField]
    private TextMeshProUGUI[] DPS_Nick_Name;
    [SerializeField]
    private TextMeshProUGUI[] DPS;
    [SerializeField]
    private GameObject Fix_UI; // ��ũ������ ������ ������Ʈ
 
    [SerializeField]
    private GameObject DPS_Rank_Page;
    [SerializeField]
    private GameObject Stage_Rank_Page;


    private void Start()
    {
        Stage_Rank_Page.gameObject.SetActive(true);
        DPS_Rank_Page.gameObject.SetActive(true);
        Set_User_Rank();
        Set_User_Rank_DPS();
    }

    private void Set_User_Rank()
    {
        Fix_UI.gameObject.SetActive(true);

        var bro = Backend.Leaderboard.User.GetLeaderboard(Utils.LEADERBOARD_UUID);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError("�������� ��ȸ ���� : " + bro.GetStatusCode());
            return;
        }

        var list = bro.GetUserLeaderboardList();

        if (list == null || list.Count == 0)
        {
            Debug.LogWarning("�������忡 ���� ������ �����ϴ�.");
            return;
        }

        // Nick_Name �Ǵ� Stage �迭�� ������� ��� ���
        if (Nick_Name == null || Stage == null)
        {
            Debug.LogError("Nick_Name �Ǵ� Stage �迭�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }


        int count = Mathf.Min(Nick_Name.Length, Stage.Length, list.Count);

        for (int i = 0; i < count; i++)
        {
            var item = list[i];

            // ���� ��ҵ� null �˻�
            if (Nick_Name[i] != null)
                Nick_Name[i].text = string.IsNullOrEmpty(item.nickname) ? "-" : item.nickname;

            if (Stage[i] != null)
            {
                if (int.TryParse(item.score, out int tierIndex))
                {
                    Stage[i].text = $"{item.score}��";
                }
                else
                {
                    Stage[i].text = "-";
                }
            }
 
        }

        // ���� UI ���� �ʱ�ȭ (�� ĭ���� ó��)
        for (int i = list.Count; i < Mathf.Min(Nick_Name.Length, Stage.Length); i++)
        {
            if (Nick_Name[i] != null) Nick_Name[i].text = "-";
            if (Stage[i] != null) Stage[i].text = "-";
        }

        Fix_UI.gameObject.SetActive(false); // ������� ������ ���� ������, ��ũ������ �����̹Ƿ�, ����
      
    }
    private void Set_User_Rank_DPS()
    {
        Fix_UI.gameObject.SetActive(true);

        var bro = Backend.Leaderboard.User.GetLeaderboard(Utils.DPS_LEADERBOARD_UUID);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError("�������� ��ȸ ���� : " + bro.GetStatusCode());
            return;
        }

        var list = bro.GetUserLeaderboardList();

        if (list == null || list.Count == 0)
        {
            Debug.LogWarning("�������忡 ���� ������ �����ϴ�.");
            return;
        }

        // Nick_Name �Ǵ� Stage �迭�� ������� ��� ���
        if (DPS_Nick_Name == null || DPS == null)
        {
            Debug.LogError("Nick_Name �Ǵ� Stage �迭�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }


        int count = Mathf.Min(DPS_Nick_Name.Length, DPS.Length, list.Count);

        for (int i = 0; i < count; i++)
        {
            var item = list[i];

            // ���� ��ҵ� null �˻�
            if (DPS_Nick_Name[i] != null)
                DPS_Nick_Name[i].text = string.IsNullOrEmpty(item.nickname) ? "-" : item.nickname;

            if (DPS[i] != null)
            {
                if (int.TryParse(item.score, out int tierIndex))
                {
                    DPS[i].text = $"{item.score}�ܰ�";
                }
                else
                {
                    DPS[i].text = "-";
                }
            }

        }

        // ���� UI ���� �ʱ�ȭ (�� ĭ���� ó��)
        for (int i = list.Count; i < Mathf.Min(DPS_Nick_Name.Length, DPS.Length); i++)
        {
            if (DPS_Nick_Name[i] != null) DPS_Nick_Name[i].text = "-";
            if (DPS[i] != null) DPS[i].text = "-";
        }

        Fix_UI.gameObject.SetActive(false); // ������� ������ ���� ������, ��ũ������ �����̹Ƿ�, ����

        DPS_Rank_Page.gameObject.SetActive(false);
    }
    public void SWITCH_DPS_Rank_Page()
    {
        Stage_Rank_Page.gameObject.SetActive(false);
        DPS_Rank_Page.gameObject.SetActive(true);
    }
    public void SWITCH_Stage_Rank_Page()
    {
        Stage_Rank_Page.gameObject.SetActive(true);
        DPS_Rank_Page.gameObject.SetActive(false);
    }
    public override void DisableOBJ()
    {
        base.DisableOBJ();
    }
}
