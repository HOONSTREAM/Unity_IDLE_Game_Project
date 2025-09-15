using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UI_Rank : UI_Base
{
    [Header("Dynamic List")]
    [SerializeField] private Transform contentParent_Stage;   // ScrollRect Content
    [SerializeField] private Transform contentParent_DPS;   // ScrollRect Content
    [SerializeField] private Rank rankItemPrefab;   // 프리팹 (위에서 만든 RankItem)

    [Header("Options")]
    [SerializeField] private int maxEntries;    // 최대 노출 개수

    // 간단한 풀(재사용) 매 프레임 Instantiate를 피함
    private readonly List<Rank> _pool = new List<Rank>();
    private readonly List<Rank> _pool_dps = new List<Rank>();

    [SerializeField]
    private GameObject Fix_UI; // 랭크페이지 점검중 오브젝트
 
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

    private void ClearOrResizePool(int neededCount)
    {
        // 부족하면 생성해서 풀 채우기
        while (_pool.Count < neededCount)
        {
            var item = Instantiate(rankItemPrefab, contentParent_Stage);
            item.gameObject.SetActive(false);
            _pool.Add(item);
        }

        // 넘치는 애들은 끄기만
        for (int i = neededCount; i < _pool.Count; i++)
            _pool[i].gameObject.SetActive(false);
    }

    private void ClearOrResizePool_DPS(int neededCount)
    {
        // 부족하면 생성해서 풀 채우기
        while (_pool_dps.Count < neededCount)
        {
            var item = Instantiate(rankItemPrefab, contentParent_DPS);
            item.gameObject.SetActive(false);
            _pool_dps.Add(item);
        }

        // 넘치는 애들은 끄기만
        for (int i = neededCount; i < _pool.Count; i++)
            _pool_dps[i].gameObject.SetActive(false);
    }


    private void Set_User_Rank()
    {
        Fix_UI.gameObject.SetActive(true);

        var bro = Backend.Leaderboard.User.GetLeaderboard(Utils.LEADERBOARD_UUID, 100);
        if (bro.IsSuccess() == false)
        {
            Debug.LogError("리더보드 조회 실패 : " + bro.GetStatusCode());
            return;
        }

      

        var list = bro.GetUserLeaderboardList();
        if (list == null || list.Count == 0)
        {
            Debug.LogWarning("리더보드에 유저 정보가 없습니다.");
            // 모두 숨김
            ClearOrResizePool(0);
            return;
        }

        // 보여줄 개수 결정
        int showCount = Mathf.Min(maxEntries, list.Count);

        // 풀 준비
        ClearOrResizePool(showCount);

        // 데이터 바인딩
        for (int i = 0; i < showCount; i++)
        {
            var item = list[i];

            // 점수 파싱 (RP는 숫자 문자열로 온다고 가정)
            long rpValue = 0;
            if (!long.TryParse(item.score, out rpValue))
                rpValue = 0;

            var view = _pool[i];
            view.Bind(i + 1, item.nickname, rpValue, true);
            view.gameObject.SetActive(true);
        }

        Fix_UI.gameObject.SetActive(false); // 여기까지 로직이 진행 됐으면, 랭크페이지 정상이므로, 해제

        // ContentSizeFitter/VerticalLayoutGroup이 달려있다면 자동으로 사이즈 갱신됩니다.
        // 필요 시 LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentParent);
    }
   
    private void Set_User_Rank_DPS()
    {
        Fix_UI.gameObject.SetActive(true);

        var bro = Backend.Leaderboard.User.GetLeaderboard(Utils.DPS_LEADERBOARD_UUID, 100);
        if (bro.IsSuccess() == false)
        {
            Debug.LogError("리더보드 조회 실패 : " + bro.GetStatusCode());
            return;
        }


        var list = bro.GetUserLeaderboardList();
        if (list == null || list.Count == 0)
        {
            Debug.LogWarning("리더보드에 유저 정보가 없습니다.");
            // 모두 숨김
            ClearOrResizePool_DPS(0);
            return;
        }

        // 보여줄 개수 결정
        int showCount = Mathf.Min(maxEntries, list.Count);

        // 풀 준비
        ClearOrResizePool_DPS(showCount);

        // 데이터 바인딩
        for (int i = 0; i < showCount; i++)
        {
            var item = list[i];

            // 점수 파싱 (RP는 숫자 문자열로 온다고 가정)
            long rpValue = 0;
            if (!long.TryParse(item.score, out rpValue))
                rpValue = 0;

            var view = _pool_dps[i];
            view.Bind(i + 1, item.nickname, rpValue, false);
            view.gameObject.SetActive(true);
        }

        Fix_UI.gameObject.SetActive(false); // 여기까지 로직이 진행 됐으면, 랭크페이지 정상이므로, 해제

        // ContentSizeFitter/VerticalLayoutGroup이 달려있다면 자동으로 사이즈 갱신됩니다.
        // 필요 시 LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentParent);
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
