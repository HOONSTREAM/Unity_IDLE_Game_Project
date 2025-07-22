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
        PLAYER_HIGH_STAGE.text = $"{Data_Manager.Main_Players_Data.Player_Max_Stage} ��";

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
                if (level % 1000 != 0) continue; // 1000���� ���� ���

                // ���̾� ���� �� �б�
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
            STAGE_REWARD_TEXT.text = "�ְ�ܰ�";            
        }

        return base.Init();
    }

    public void Get_All_Stage_Rewards()
    {
        var playerData = Data_Manager.Main_Players_Data;
        int currentLevel = playerData.Player_Max_Stage;

        if (playerData.Player_Max_Stage >= Stage_Manager.MAX_STAGE)
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("�ְ� ���������� �޼��Ͽ����ϴ�.");
            return;
        }

        // ���� �̷� �Ľ�
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
            if (level % 1000 != 0) continue; // 1000���� ���� ���

            // ���̾� ���� �� �б�
            int reward = 0;
            if (int.TryParse(row["DIAMOND"].ToString(), out reward))
            {
                totalReward += reward;
                newlyClaimed.Add(level);
            }
        }

        if (newlyClaimed.Count == 0)
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("������ �� �ִ� ������ �����ϴ�.");
            return;
        }

        // ���̾� ����
        playerData.DiaMond += totalReward;

        // �̷� ������Ʈ
        claimedSet.UnionWith(newlyClaimed);
        playerData.STAGE_REWARD = string.Join(",", claimedSet.OrderBy(x => x));

        // ���� ����

        Base_Manager.BACKEND.Log_Get_Dia("STAGE_REWARD");
        _ = Base_Manager.BACKEND.WriteData();

        // �˸� �� ȿ��
        Base_Canvas.instance.Get_TOP_Popup().Initialize($"���� ���� �Ϸ�! (+ <color=#FFFF00>{totalReward}</color> ���̾�)");
        Utils.SendSystemLikeMessage($"�� <color=#FFFF00>�������� ���� ���̾Ƹ��</color> {totalReward} ���� �����߽��ϴ�!");
        Base_Manager.SOUND.Play(Sound.BGS, "Victory");
        Init();
    }


    public void Move_Stage_Button()
    {
        string inputText = STAGE_Input.text;

        // �Է��� ��� �ְų� ������ ���
        if (string.IsNullOrWhiteSpace(inputText))
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("�� ���� �Է����ּ���.");
            return;
        }

        // ���� �Ľ� ���� ��
        if (!int.TryParse(inputText, out int move_Stage))
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("���ڸ� �Է����ּ���.");
            return;
        }

        // �ְ� �������� ���� ���� �Է��� ���
        if (move_Stage > Data_Manager.Main_Players_Data.Player_Max_Stage)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("�ְ� �� ���� ���� �̵��� �� �����ϴ�.");
            return;
        }

        // �̵� ó��
        Data_Manager.Main_Players_Data.Player_Stage = move_Stage;
      
        Main_UI.Instance.Main_UI_PlayerInfo_Text_Check();
        
        Main_UI.Instance.Set_Mode_Change_Idle_Mode();

        Base_Manager.instance.StopAllPoolCoroutines();
        Base_Manager.Pool.Clear_Pool();
        Base_Manager.Stage.State_Change(Stage_State.Ready);

        Base_Canvas.instance.Get_Toast_Popup().Initialize("�������� �̵��� �Ϸ�Ǿ����ϴ�.");
        Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

        Main_UI.Instance.Fake_Loading_Panel_Fade();

        DisableOBJ();
    }

    public override void DisableOBJ()
    {
        base.DisableOBJ();
    }

}
