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
    [SerializeField]
    private GameObject Fix_UI; // 랭크페이지 점검중 오브젝트

    

    private void Start()
    {
        Set_User_Rank();
        
    }

    private void Set_User_Rank()
    {
        Fix_UI.gameObject.SetActive(true);

        var bro = Backend.Leaderboard.User.GetLeaderboard(Utils.LEADERBOARD_UUID);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError("리더보드 조회 실패 : " + bro.GetStatusCode());
            return;
        }

        var list = bro.GetUserLeaderboardList();

        if (list == null || list.Count == 0)
        {
            Debug.LogWarning("리더보드에 유저 정보가 없습니다.");
            return;
        }

        // Nick_Name 또는 Stage 배열이 비어있을 경우 대비
        if (Nick_Name == null || Stage == null)
        {
            Debug.LogError("Nick_Name 또는 Stage 배열이 초기화되지 않았습니다.");
            return;
        }

        int count = Mathf.Min(Nick_Name.Length, Stage.Length, list.Count);

        for (int i = 0; i < count; i++)
        {
            var item = list[i];

            // 개별 요소도 null 검사
            if (Nick_Name[i] != null)
                Nick_Name[i].text = string.IsNullOrEmpty(item.nickname) ? "-" : item.nickname;

            if (Stage[i] != null)
            {
                if (int.TryParse(item.score, out int tierIndex))
                {
                    Stage[i].text = Utils.Set_Rank_To_Tier(int.Parse(item.score));
                }
                else
                {
                    Stage[i].text = "-";
                }
            }
 
        }

        // 남은 UI 슬롯 초기화 (빈 칸으로 처리)
        for (int i = list.Count; i < Mathf.Min(Nick_Name.Length, Stage.Length); i++)
        {
            if (Nick_Name[i] != null) Nick_Name[i].text = "-";
            if (Stage[i] != null) Stage[i].text = "-";
        }

        Fix_UI.gameObject.SetActive(false); // 여기까지 로직이 진행 됐으면, 랭크페이지 정상이므로, 해제
    }
    public override void DisableOBJ()
    {
        base.DisableOBJ();
    }
}
