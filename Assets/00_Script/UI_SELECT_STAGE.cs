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
        PLAYER_HIGH_STAGE.text = $"{Data_Manager.Main_Players_Data.Player_Max_Stage} ��";
        return base.Init();
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
