using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UI_SELECT_STAGE : UI_Base
{
    [SerializeField]
    private TextMeshProUGUI PLAYER_HIGH_STAGE;
    [SerializeField]
    private TMP_InputField STAGE_Input;
    [SerializeField]
    private TextMeshProUGUI STAGE_REWARD_TEXT;

    public override bool Init()
    {
        PLAYER_HIGH_STAGE.text = $"{Data_Manager.Main_Players_Data.Player_Max_Stage} 층";

        if (Data_Manager.Main_Players_Data.Player_Max_Stage < Stage_Manager.MAX_STAGE)
        {
            var playerData = Data_Manager.Main_Players_Data;
            int currentLevel = playerData.Player_Max_Stage;

            var claimedSet = new HashSet<int>();

            if (!string.IsNullOrEmpty(playerData.STAGE_REWARD))
            {
                string[] tokens = playerData.STAGE_REWARD.Split(',');
                foreach (var t in tokens)
                {
                    if (int.TryParse(t, out int claimed))
                        claimedSet.Add(claimed);
                }
            }

            int totalReward = 0;
            List<int> newlyClaimed = new List<int>();

            foreach (var row in CSV_Importer.STAGE_REWARD_Design)
            {
                int level = int.Parse(row["STAGE"].ToString());
                if (level > currentLevel) break;
                if (claimedSet.Contains(level)) continue;
                if (level % 1000 != 0) continue; // 1000단위 보상만 허용

                // 다이아 보상 값 읽기
                int reward = 0;
                if (int.TryParse(row["DIAMOND"].ToString(), out reward))
                {
                    totalReward += reward;
                    newlyClaimed.Add(level);
                }
            }

            if (newlyClaimed.Count == 0)
            {
                STAGE_REWARD_TEXT.text = "0";
            }

            STAGE_REWARD_TEXT.text = totalReward.ToString();
        }

        else
        {
            STAGE_REWARD_TEXT.text = "최고단계";            
        }

        return base.Init();
    }

    public void Get_All_Stage_Rewards()
    {
        var playerData = Data_Manager.Main_Players_Data;
        int currentLevel = playerData.Player_Max_Stage;

        if (playerData.Player_Max_Stage >= Stage_Manager.MAX_STAGE)
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("최고 스테이지에 달성하였습니다.");
            return;
        }

        // 수령 이력 파싱
        var claimedSet = new HashSet<int>();
        if (!string.IsNullOrEmpty(playerData.STAGE_REWARD))
        {
            string[] tokens = playerData.STAGE_REWARD.Split(',');
            foreach (var t in tokens)
            {
                if (int.TryParse(t, out int claimed))
                    claimedSet.Add(claimed);
            }
        }

        int totalReward = 0;
        List<int> newlyClaimed = new List<int>();

        foreach (var row in CSV_Importer.STAGE_REWARD_Design)
        {
            int level = int.Parse(row["STAGE"].ToString());
            if (level > currentLevel) break;
            if (claimedSet.Contains(level)) continue;
            if (level % 1000 != 0) continue; // 1000단위 보상만 허용

            // 다이아 보상 값 읽기
            int reward = 0;
            if (int.TryParse(row["DIAMOND"].ToString(), out reward))
            {
                totalReward += reward;
                newlyClaimed.Add(level);
            }
        }

        if (newlyClaimed.Count == 0)
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("수령할 수 있는 보상이 없습니다.");
            return;
        }

        // 다이아 지급
        playerData.DiaMond += totalReward;

        // 이력 업데이트
        claimedSet.UnionWith(newlyClaimed);
        playerData.STAGE_REWARD = string.Join(",", claimedSet.OrderBy(x => x));

        // 서버 저장

        Base_Manager.BACKEND.Log_Get_Dia("STAGE_REWARD");
        _ = Base_Manager.BACKEND.WriteData();

        // 알림 및 효과
        Base_Canvas.instance.Get_TOP_Popup().Initialize($"보상 수령 완료! (+ <color=#FFFF00>{totalReward}</color> 다이아)");
        Utils.SendSystemLikeMessage($"★ <color=#FFFF00>스테이지 보상 다이아몬드</color> {totalReward} 개를 수령했습니다!");
        Base_Manager.SOUND.Play(Sound.BGS, "Victory");
        Init();
    }


    public void Move_Stage_Button()
    {
        string inputText = STAGE_Input.text;

        // 입력이 비어 있거나 공백일 경우
        if (string.IsNullOrWhiteSpace(inputText))
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("층 수를 입력해주세요.");
            return;
        }

        // 숫자 파싱 실패 시
        if (!int.TryParse(inputText, out int move_Stage))
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("숫자만 입력해주세요.");
            return;
        }

        // 최고 층수보다 높은 층을 입력한 경우
        if (move_Stage > Data_Manager.Main_Players_Data.Player_Max_Stage)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("최고 층 보다 높이 이동할 수 없습니다.");
            return;
        }

        // 이동 처리
        Data_Manager.Main_Players_Data.Player_Stage = move_Stage;
      
        Main_UI.Instance.Main_UI_PlayerInfo_Text_Check();
        
        Main_UI.Instance.Set_Mode_Change_Idle_Mode();

        Base_Manager.instance.StopAllPoolCoroutines();
        Base_Manager.Pool.Clear_Pool();
        Base_Manager.Stage.State_Change(Stage_State.Ready);

        Base_Canvas.instance.Get_Toast_Popup().Initialize("스테이지 이동이 완료되었습니다.");
        Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

        Main_UI.Instance.Fake_Loading_Panel_Fade();

        DisableOBJ();
    }

    public override void DisableOBJ()
    {
        base.DisableOBJ();
    }

}
