using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Rank : UI_Base
{
    [SerializeField]
    private TextMeshProUGUI[] Nick_Name;
    [SerializeField]
    private TextMeshProUGUI[] Stage;

    

    private void Start()
    {
        Set_User_Rank();

    }

    private void Set_User_Rank()
    {
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
                Stage[i].text = !string.IsNullOrEmpty(item.score) ? $"{item.score}��" : "- ��";

            Debug.Log($"{item.rank}�� : {item.nickname}, {item.score}��");
        }

        // ���� UI ���� �ʱ�ȭ (�� ĭ���� ó��)
        for (int i = list.Count; i < Mathf.Min(Nick_Name.Length, Stage.Length); i++)
        {
            if (Nick_Name[i] != null) Nick_Name[i].text = "-";
            if (Stage[i] != null) Stage[i].text = "-";
        }
    }
    public override void DisableOBJ()
    {
        base.DisableOBJ();
    }
}
