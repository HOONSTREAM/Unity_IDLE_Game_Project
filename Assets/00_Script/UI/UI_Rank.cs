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
        Backend.Leaderboard.User.GetLeaderboards(bro => {
            foreach (BackEnd.Leaderboard.LeaderboardTableItem item in bro.GetLeaderboardTableList())
            {
                string uuid = item.uuid;
                Debug.Log(item.ToString());
            }
        });
    }
    public override void DisableOBJ()
    {
        base.DisableOBJ();
    }
}
