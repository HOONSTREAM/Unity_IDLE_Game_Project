using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd;
using UnityEngine;

public partial class BackEnd_Manager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        var bro = Backend.Initialize(); // �ڳ� �ʱ�ȭ

        // �ڳ� �ʱ�ȭ�� ���� ���䰪
        if (bro.IsSuccess())
        {
            Debug.Log("�ڳ� ���� �ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 204 Success
        }
        else
        {
            Debug.LogError("�ڳ� ���� �ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 400�� ���� �߻� 
        }      
    }

    private void Start()
    {     
        Loading_Scene.instance.LoadingMain();

        #region ���� �ڵ鷯
        if (Backend.IsInitialized)
        {
            Backend.ErrorHandler.OnMaintenanceError = () => {
                Base_Canvas.instance.Get_MainGame_Error_UI().Initialize("���� ������ �����մϴ�. ������ �����մϴ�.");
                Application.Quit();
            };
            Backend.ErrorHandler.OnTooManyRequestError = () => {
                
                
            };
            Backend.ErrorHandler.OnTooManyRequestByLocalError = () => {
                
                
            };
            Backend.ErrorHandler.OnOtherDeviceLoginDetectedError = () => {
                Base_Canvas.instance.Get_MainGame_Error_UI().Initialize("���������� �����Ǿ����ϴ�. ������ �����մϴ�.");
                Log_Try_Multi_Connection("��� �������� �õ�");
                Application.Quit();
            };
        }
        #endregion
    }



    #region Log ���
    public void Log_HeroSummon(Character_Scriptable hero, int Pickup_Count)
    {
        Param param = new Param();
        param.Add("HeroName", hero.Character_EN_Name);
        param.Add("PickupCount", Pickup_Count);
        param.Add("Total_Hero_Card", Base_Manager.Data.character_Holder[hero.Character_EN_Name].Hero_Card_Amount);
        param.Add("Total_Hero_Level", Base_Manager.Data.character_Holder[hero.Character_EN_Name].Hero_Level);
        param.Add("Action", "Summon_Hero");
        param.Add("Time", Utils.Get_Server_Time().ToString("yyyy-MM-dd HH:mm:ss"));

        Backend.GameLog.InsertLogV2("Summon_Log", param, (callback) =>
        {

        });
    }
    public void Log_RelicSummon(Item_Scriptable relic, int Pickup_Count)
    {
        Param param = new Param();
        param.Add("HeroName", relic.Item_Name);
        param.Add("PickupCount", Pickup_Count);
        param.Add("Total_Hero_Card", Base_Manager.Data.Item_Holder[relic.name].Hero_Card_Amount);
        param.Add("Total_Hero_Level", Base_Manager.Data.Item_Holder[relic.name].Hero_Level);
        param.Add("Action", "Summon_Relic");
        param.Add("Time", Utils.Get_Server_Time().ToString("yyyy-MM-dd HH:mm:ss"));

        Backend.GameLog.InsertLogV2("Summon_RELIC_Log", param, (callback) =>
        {

        });
    }
    public void Log_Try_Smelt()
    {
        Param param = new Param();
        
        param.Add("Action", "Smelt");
        param.Add("User_Total_Steel", Base_Manager.Data.Item_Holder["Steel"].Hero_Card_Amount);
        param.Add("Time", Utils.Get_Server_Time().ToString("yyyy-MM-dd HH:mm:ss"));

        Backend.GameLog.InsertLogV2("User_Smelt_Log", param, (callback) =>
        {

        });
    }
    public void Log_Try_Crack_IAP(string id, string result)
    {
        Param param = new Param();

        param.Add("Action", "Try_IAP_Fail_Check_CRACK_OR_NOT");
        param.Add("ProductID", id);
        param.Add("Result", result);
        param.Add("Time", Utils.Get_Server_Time().ToString("yyyy-MM-dd HH:mm:ss"));

        Backend.GameLog.InsertLogV2("User_Fail_IAP", param, (callback) =>
        {

        });
    }
    
    public void Log_Get_Dia(string Action)
    {
        Param param = new Param();

        param.Add("Action", Action);
        param.Add("DIAMOND", Data_Manager.Main_Players_Data.DiaMond);

        if (Action == "Combination_Dia")
        {
            param.Add("Meat", Base_Manager.Data.Item_Holder["Meat"].Hero_Card_Amount);
        }

        if(Action == "DPS_Dungeon")
        {
            param.Add("DPS_LEVEL", Data_Manager.Main_Players_Data.USER_DPS_LEVEL);
        }

        if(Action == "STAGE_REWARD")
        {
            
        }
        
        param.Add("Time", Utils.Get_Server_Time().ToString("yyyy-MM-dd HH:mm:ss"));

        Backend.GameLog.InsertLogV2("User_Get_Dia_Log", param, (callback) =>
        {

        });
    }
    public void Log_Get_Combination_Hondon_Ball(string Action)
    {
        Param param = new Param();

        param.Add("Action", Action);
        param.Add("Hondon_Potion", Base_Manager.Data.Item_Holder["Hondon_Potion"].Hero_Card_Amount);
        param.Add("Hondon_Ball", Base_Manager.Data.Item_Holder["Hondon_Ball"].Hero_Card_Amount);
        param.Add("Time", Utils.Get_Server_Time().ToString("yyyy-MM-dd HH:mm:ss"));

        Backend.GameLog.InsertLogV2("User_Hondon_Ball_Comb", param, (callback) =>
        {

        });
    }
    public void Log_Try_Multi_Connection(string Action)
    {
        Param param = new Param();

        param.Add("Action", Action);       
        param.Add("Time", Utils.Get_Server_Time().ToString("yyyy-MM-dd HH:mm:ss"));

        Backend.GameLog.InsertLogV2("User_Try_Multi_Connection", param, (callback) =>
        {

        });
    }
    public void Log_Clear_Dungeon(int Dungeon_Type)
    {
        Param param = new Param();

        param.Add("Action", "Dungeon");
        param.Add("Dungeon_Type", Dungeon_Type);
        if(Dungeon_Type != 2)
        {
            param.Add("Clear_Level", Data_Manager.Main_Players_Data.Dungeon_Clear_Level[Dungeon_Type]);
        }      
        param.Add("Time", Utils.Get_Server_Time().ToString("yyyy-MM-dd HH:mm:ss"));

        Backend.GameLog.InsertLogV2("User_Clear_Dungeon_Log", param, (callback) =>
        {

        });
    }
    public void Log_Hero_Upgrade(Character_Scriptable hero, Holder holder)
    {
        Param param = new Param();
        param.Add("HeroName", hero.Character_EN_Name);
        param.Add("Hero_Level", holder.Hero_Level);
        param.Add("Hero_Card_Amount", holder.Hero_Card_Amount);       
        param.Add("Action", "Upgrade_Hero");
        param.Add("Time", Utils.Get_Server_Time().ToString("yyyy-MM-dd HH:mm:ss"));

        Backend.GameLog.InsertLogV2("Upgrade_Hero_Log", param, (callback) =>
        {

        });
    }
    #endregion

}
