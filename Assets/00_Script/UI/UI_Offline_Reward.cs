using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Offline_Reward : UI_Base
{
    [SerializeField]
    private TextMeshProUGUI Offline_Time;
    [SerializeField]
    private TextMeshProUGUI money_reward_value;

    private double _money_reward_value;

    public override bool Init()
    {
        return base.Init();
    }

    /// <summary>
    /// 유저가 실수로 오프라인 보상창을 종료해도, 자동으로 기본 보상을 획득하도록 합니다.
    /// </summary>
    public override void DisableOBJ()
    {
        base.DisableOBJ();
    }
}
