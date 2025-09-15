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
    [SerializeField] private Rank rankItemPrefab;   // ������ (������ ���� RankItem)

    [Header("Options")]
    [SerializeField] private int maxEntries;    // �ִ� ���� ����

    // ������ Ǯ(����) �� ������ Instantiate�� ����
    private readonly List<Rank> _pool = new List<Rank>();
    private readonly List<Rank> _pool_dps = new List<Rank>();

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

    private void ClearOrResizePool(int neededCount)
    {
        // �����ϸ� �����ؼ� Ǯ ä���
        while (_pool.Count < neededCount)
        {
            var item = Instantiate(rankItemPrefab, contentParent_Stage);
            item.gameObject.SetActive(false);
            _pool.Add(item);
        }

        // ��ġ�� �ֵ��� ���⸸
        for (int i = neededCount; i < _pool.Count; i++)
            _pool[i].gameObject.SetActive(false);
    }

    private void ClearOrResizePool_DPS(int neededCount)
    {
        // �����ϸ� �����ؼ� Ǯ ä���
        while (_pool_dps.Count < neededCount)
        {
            var item = Instantiate(rankItemPrefab, contentParent_DPS);
            item.gameObject.SetActive(false);
            _pool_dps.Add(item);
        }

        // ��ġ�� �ֵ��� ���⸸
        for (int i = neededCount; i < _pool.Count; i++)
            _pool_dps[i].gameObject.SetActive(false);
    }


    private void Set_User_Rank()
    {
        Fix_UI.gameObject.SetActive(true);

        var bro = Backend.Leaderboard.User.GetLeaderboard(Utils.LEADERBOARD_UUID, 100);
        if (bro.IsSuccess() == false)
        {
            Debug.LogError("�������� ��ȸ ���� : " + bro.GetStatusCode());
            return;
        }

      

        var list = bro.GetUserLeaderboardList();
        if (list == null || list.Count == 0)
        {
            Debug.LogWarning("�������忡 ���� ������ �����ϴ�.");
            // ��� ����
            ClearOrResizePool(0);
            return;
        }

        // ������ ���� ����
        int showCount = Mathf.Min(maxEntries, list.Count);

        // Ǯ �غ�
        ClearOrResizePool(showCount);

        // ������ ���ε�
        for (int i = 0; i < showCount; i++)
        {
            var item = list[i];

            // ���� �Ľ� (RP�� ���� ���ڿ��� �´ٰ� ����)
            long rpValue = 0;
            if (!long.TryParse(item.score, out rpValue))
                rpValue = 0;

            var view = _pool[i];
            view.Bind(i + 1, item.nickname, rpValue, true);
            view.gameObject.SetActive(true);
        }

        Fix_UI.gameObject.SetActive(false); // ������� ������ ���� ������, ��ũ������ �����̹Ƿ�, ����

        // ContentSizeFitter/VerticalLayoutGroup�� �޷��ִٸ� �ڵ����� ������ ���ŵ˴ϴ�.
        // �ʿ� �� LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentParent);
    }
   
    private void Set_User_Rank_DPS()
    {
        Fix_UI.gameObject.SetActive(true);

        var bro = Backend.Leaderboard.User.GetLeaderboard(Utils.DPS_LEADERBOARD_UUID, 100);
        if (bro.IsSuccess() == false)
        {
            Debug.LogError("�������� ��ȸ ���� : " + bro.GetStatusCode());
            return;
        }


        var list = bro.GetUserLeaderboardList();
        if (list == null || list.Count == 0)
        {
            Debug.LogWarning("�������忡 ���� ������ �����ϴ�.");
            // ��� ����
            ClearOrResizePool_DPS(0);
            return;
        }

        // ������ ���� ����
        int showCount = Mathf.Min(maxEntries, list.Count);

        // Ǯ �غ�
        ClearOrResizePool_DPS(showCount);

        // ������ ���ε�
        for (int i = 0; i < showCount; i++)
        {
            var item = list[i];

            // ���� �Ľ� (RP�� ���� ���ڿ��� �´ٰ� ����)
            long rpValue = 0;
            if (!long.TryParse(item.score, out rpValue))
                rpValue = 0;

            var view = _pool_dps[i];
            view.Bind(i + 1, item.nickname, rpValue, false);
            view.gameObject.SetActive(true);
        }

        Fix_UI.gameObject.SetActive(false); // ������� ������ ���� ������, ��ũ������ �����̹Ƿ�, ����

        // ContentSizeFitter/VerticalLayoutGroup�� �޷��ִٸ� �ڵ����� ������ ���ŵ˴ϴ�.
        // �ʿ� �� LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentParent);
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
