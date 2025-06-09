using TMPro;
using UnityEngine;

public class UI_SELECT_STAGE : UI_Base
{
    [SerializeField]
    private TextMeshProUGUI PLAYER_HIGH_STAGE;
    [SerializeField]
    private TMP_InputField STAGE_Input;

    public override bool Init()
    {
        PLAYER_HIGH_STAGE.text = $"{Data_Manager.Main_Players_Data.Player_Max_Stage} 층";
        return base.Init();
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
