using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using BackEnd;
using BackEnd.BackndNewtonsoft.Json.Linq;
using LitJson;
using System.Threading.Tasks;

public partial class BackEnd_Manager : MonoBehaviour
{
    /// <summary>
    /// ���� �����͸� �����մϴ�. 00��~01�� ���̴� �����ð� üũ�Ͽ�, �������� ���� ����� �����մϴ�.
    /// </summary>
    /// <returns></returns>
    public async Task WriteData()
    {
        Debug.Log("WriteData �޼��� ȣ��, �����͸� ����մϴ�.");

        if (Data_Manager.Main_Players_Data == null)
        {
            Debug.LogError("�����Ͱ� �������� �ʽ��ϴ�. Initialize ���� �����͸� �������ּ���.");
            return;
        }

        try
        {
            DateTime now = Utils.Get_Server_Time();
            bool isMidnightRange = now.Hour == 0;

            await SaveDefaultData(isMidnightRange);
            await SaveCharacterData();
            await SaveItemData();
            await SaveSmeltData();
            await SaveSetHeroData();
            await SaveSetRelicData();
        }
        catch (Exception e)
        {
            Debug.LogError($"[ERROR] WriteData ���� �� ���� �߻�: {e.Message}");
        }
    } 
    private async Task SaveDefaultData(bool skipLeaderboard)
    {
        Param param = new Param();

        var data = Data_Manager.Main_Players_Data;
        param.Add("ATK", data.ATK);
        param.Add("HP", data.HP);
        param.Add("PLAYER_TIER", (int)data.Player_Tier);
        param.Add("PLAYER_MONEY", data.Player_Money);
        param.Add("DIAMOND", data.DiaMond);
        param.Add("PLAYER_LEVEL", data.Player_Level);
        param.Add("PLAYER_EXP", data.EXP);
        param.Add("PLAYER_STAGE", data.Player_Stage);
        param.Add("EXP_UPGRADE_COUNT", data.EXP_Upgrade_Count);
        param.Add("BUFF_TIMER", data.Buff_Timers);
        param.Add("ADS_TIMER", data.ADS_Timer);
        param.Add("SPEED", data.buff_x2_speed);
        param.Add("QUEST_COUNT", data.Quest_Count);
        param.Add("HERO_SUMMON_COUNT", data.Hero_Summon_Count);
        param.Add("HERO_PICKUP_COUNT", data.Hero_Pickup_Count);
        param.Add("RELIC_SUMMON_COUNT", data.Relic_Summon_Count);
        param.Add("RELIC_PICKUP_COUNT", data.Relic_Pickup_Count);
        param.Add("START_DATE", data.StartDate);
        param.Add("END_DATE", Utils.Get_Server_Time());
        param.Add("DAILY_ENTER_KEY", data.Daily_Enter_Key);
        param.Add("USER_KEY_ASSETS", data.User_Key_Assets);
        param.Add("DUNGEON_CLEAR_LEVEL", data.Dungeon_Clear_Level);
        param.Add("isBUY_AD_Package", data.isBuyADPackage);
        param.Add("isBUY_LAUNCH_EVENT", data.isBuyLAUNCH_EVENT);
        param.Add("isBUY_TODAY_Package", data.isBuyTodayPackage);
        param.Add("isBUY_STRONG_Package", data.isBuySTRONGPackage);
        param.Add("ADS_HERO_SUMMON_COUNT", data.ADS_Hero_Summon_Count);
        param.Add("ADS_RELIC_SUMMON_COUNT", data.ADS_Relic_Summon_Count);

        data.Event_Push_Alarm_Agree = Utils.is_push_alarm_agree;

        param.Add("EVENT_PUSH_ALARM", data.Event_Push_Alarm_Agree);

        param.Add("Daily_Attendance", data.Daily_Attendance);
        param.Add("Daily_Levelup", data.Levelup);
        param.Add("Daily_Summon", data.Summon);
        param.Add("Daily_Relic", data.Relic);
        param.Add("Daily_Dungeon_Gold", data.Dungeon_Gold);
        param.Add("Daily_Dungeon_Dia", data.Dungeon_Dia);
        param.Add("Daily_Attendance_Clear", data.Daily_Attendance_Clear);
        param.Add("Daily_Levelup_Clear", data.Level_up_Clear);
        param.Add("Daily_Summon_Clear", data.Summon_Clear);
        param.Add("Daily_Relic_Clear", data.Relic_Clear);
        param.Add("Daily_Dungeon_Gold_Clear", data.Dungeon_Gold_Clear);
        param.Add("Daily_Dungeon_Dia_Clear", data.Dungeon_Dia_Clear);
        param.Add("Fast_Mode", data.isFastMode);

        data.Season = Utils.TIER_SEASON;

        param.Add("SEASON", data.Season);
        
        var bro = Backend.GameData.Get("USER", new Where());
        
        if (!bro.IsSuccess()) return;

        string inDate = bro.GetInDate();

        
        if (skipLeaderboard)
        {
            Backend.GameData.Update("USER", new Where(), param, callback =>
            {
                Debug.Log(callback.IsSuccess() ? "������ ���� (�������� ����) ����" : "������ ���� ����");
            });
        }
        else
        {
            Backend.Leaderboard.User.UpdateMyDataAndRefreshLeaderboard(Utils.LEADERBOARD_UUID, "USER", inDate, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("��������� ������ ���� ����");
                }
                else
                {
                    Debug.LogWarning("�������� ���� ����, ��� ���� Ȯ�� �� ó��");
                    TryReRegisterLeaderboard(param);
                }
            });
        }

        await Task.Yield();
    }
    private void TryReRegisterLeaderboard(Param param)
    {
        Backend.Leaderboard.User.GetMyLeaderboard(Utils.LEADERBOARD_UUID, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.Log("�������� �̵�� ����, ��� �õ�");
                var bro = Backend.GameData.GetMyData("USER", new Where());
                string inDate = bro.GetInDate();
                var reg = Backend.URank.User.UpdateUserScore(Utils.LEADERBOARD_UUID, "USER", inDate, param);

                if (reg.IsSuccess())
                {
                    Debug.Log("�������� ��� ����");
                }
                else
                {
                    Debug.LogError("�������� ��� ����, �Ϲ� ������ ���� ����");
                    Backend.GameData.Update("USER", new Where(), param, fallback =>
                    {
                        Debug.Log($"�Ϲ� ���� ���: {fallback}");
                    });
                }
            }
        });
    }
    private async Task SaveCharacterData()
    {
        Param character_param = new Param();
        string json = JsonConvert.SerializeObject(Base_Manager.Data.character_Holder);
        character_param.Add("character", json);

        Backend.GameData.Update("CHARACTER", new Where(), character_param, callback =>
        {
            Debug.Log(callback.IsSuccess() ? "���� ������ ���� ����" : "���� ������ ���� ����");
        });

        await Task.Yield();
    }
    private async Task SaveItemData()
    {
        Param param = new Param();
        string json = JsonConvert.SerializeObject(Base_Manager.Data.Item_Holder);
        param.Add("Item", json);

        Backend.GameData.Update("ITEM", new Where(), param, callback =>
        {
            Debug.Log(callback.IsSuccess() ? "������ ���� ����" : "������ ���� ����");
        });

        await Task.Yield();
    }
    private async Task SaveSmeltData()
    {
        Param param = new Param();
        string json = JsonConvert.SerializeObject(Base_Manager.Data.User_Main_Data_Smelt_Array);
        param.Add("Smelt", json);

        Backend.GameData.Update("SMELT", new Where(), param, callback =>
        {
            Debug.Log(callback.IsSuccess() ? "���� ���� ����" : "���� ���� ����");
        });

        await Task.Yield();
    }
    private async Task SaveSetHeroData()
    {
        Param param = new Param();
        string json = JsonConvert.SerializeObject(Base_Manager.Character.Set_Character);
        param.Add("Player_Set_Hero", json);

        Backend.GameData.Update("PLAYER_SET_HERO", new Where(), param, callback =>
        {
            Debug.Log(callback.IsSuccess() ? "���� ��ġ ���� ����" : "���� ��ġ ���� ����");
        });

        await Task.Yield();
    }
    private async Task SaveSetRelicData()
    {
        Param param = new Param();
        string json = JsonConvert.SerializeObject(Base_Manager.Data.Main_Set_Item);
        param.Add("Player_Set_Relic", json);

        Backend.GameData.Update("PLAYER_SET_RELIC", new Where(), param, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("���� ��ġ ���� ����");

                if (Relic_Manager.instance != null)
                {
                    Relic_Manager.instance.Initalize();
                }
                else
                {
                    Debug.LogWarning("Relic_Manager �ν��Ͻ��� �������� �ʾ� �ʱ�ȭ �ǳʶ�");
                }
            }
            else
            {
                Debug.LogError("���� ��ġ ���� ����");
            }
        });

        await Task.Yield();
    }

    /// <summary>
    /// ���� �����͸� �ҷ��ɴϴ�.
    /// </summary>
    public void ReadData()
    {
        Debug.Log("ReadData �޼��� ȣ��, �����͸� �ҷ��ɴϴ�.");
        #region DEFAULT DATA
        Debug.Log("'USER' ���̺��� �����͸� ��ȸ�ϴ� �Լ��� ȣ���մϴ�.");
        var bro = Backend.GameData.GetMyData("USER", new Where());
        if (bro.IsSuccess())
        {
            Debug.Log("������ ��ȸ�� �����߽��ϴ�. : " + bro);

            // Json���� ���ϵ� �����͸� �޾ƿɴϴ�.
            LitJson.JsonData gameDataJson = bro.FlattenRows();

            // �޾ƿ� �������� ������ 0�̶�� �����Ͱ� �������� �ʴ� ���Դϴ�.
            if (gameDataJson.Count <= 0)
            {
                Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            }
            else
            {
                Data data = new Data();

                #region �ű� Į�� ������
               
                if (!gameDataJson[0].ContainsKey("SEASON"))
                {
                    Debug.Log("SEASON �÷��� ���� ���� �߰��մϴ�.");
                    Param param = new Param();
                    param.Add("SEASON", data.Season);

                    var bro_Get_Table_USER = Backend.GameData.Get("USER", new Where());

                    if (!bro_Get_Table_USER.IsSuccess()) return;

                    string inDate = bro_Get_Table_USER.GetInDate();

                    DateTime now = Utils.Get_Server_Time();
                    bool isMidnightRange = now.Hour == 0;

                    if (isMidnightRange)
                    {
                        Backend.GameData.Update("USER", new Where(), param, callback =>
                        {
                            Debug.Log(callback.IsSuccess() ? "������ ���� (�������� ����) ����" : "������ ���� ����");
                        });
                    }
                    else
                    {
                        Backend.Leaderboard.User.UpdateMyDataAndRefreshLeaderboard(Utils.LEADERBOARD_UUID, "USER", inDate, param, callback =>
                        {
                            if (callback.IsSuccess())
                            {
                                Debug.Log("��������� ������ ���� ����");
                            }
                            else
                            {
                                Debug.LogWarning("�������� ���� ����, ��� ���� Ȯ�� �� ó��");
                                TryReRegisterLeaderboard(param);
                            }
                        });
                    }                   
                }
                else
                {
                    Debug.Log("SEASON �÷��� �����մϴ�.");
                    data.Season = int.Parse(gameDataJson[0]["SEASON"].ToString());
                }

                #endregion

                data.ATK = double.Parse(gameDataJson[0]["ATK"].ToString());
                data.HP = double.Parse(gameDataJson[0]["HP"].ToString());
                int tier_number = int.Parse(gameDataJson[0]["PLAYER_TIER"].ToString());
                data.Player_Money = double.Parse(gameDataJson[0]["PLAYER_MONEY"].ToString());
                data.DiaMond = int.Parse(gameDataJson[0]["DIAMOND"].ToString());
                data.Player_Level = int.Parse(gameDataJson[0]["PLAYER_LEVEL"].ToString());
                data.EXP = double.Parse(gameDataJson[0]["PLAYER_EXP"].ToString());
                data.Player_Stage = int.Parse(gameDataJson[0]["PLAYER_STAGE"].ToString());
                data.EXP_Upgrade_Count = int.Parse(gameDataJson[0]["EXP_UPGRADE_COUNT"].ToString());

                data.Buff_Timers[0] = float.Parse(gameDataJson[0]["BUFF_TIMER"][0].ToString());
                data.Buff_Timers[1] = float.Parse(gameDataJson[0]["BUFF_TIMER"][1].ToString());
                data.Buff_Timers[2] = float.Parse(gameDataJson[0]["BUFF_TIMER"][2].ToString());

                data.ADS_Timer[0] = float.Parse(gameDataJson[0]["ADS_TIMER"][0].ToString());
                data.ADS_Timer[1] = float.Parse(gameDataJson[0]["ADS_TIMER"][1].ToString());

                data.buff_x2_speed = float.Parse(gameDataJson[0]["SPEED"].ToString());            
                data.Quest_Count = int.Parse(gameDataJson[0]["QUEST_COUNT"].ToString());


                data.Hero_Summon_Count = int.Parse(gameDataJson[0]["HERO_SUMMON_COUNT"].ToString());
                data.Hero_Pickup_Count = int.Parse(gameDataJson[0]["HERO_PICKUP_COUNT"].ToString());
                data.Relic_Summon_Count = int.Parse(gameDataJson[0]["RELIC_SUMMON_COUNT"].ToString());
                data.Relic_Pickup_Count = int.Parse(gameDataJson[0]["RELIC_PICKUP_COUNT"].ToString());


                data.EndDate = DateTime.Parse(gameDataJson[0]["END_DATE"].ToString());
                data.StartDate = Utils.Get_Server_Time();

                data.Daily_Enter_Key[0] = int.Parse(gameDataJson[0]["DAILY_ENTER_KEY"][0].ToString());
                data.Daily_Enter_Key[1] = int.Parse(gameDataJson[0]["DAILY_ENTER_KEY"][1].ToString());

                data.User_Key_Assets[0] = int.Parse(gameDataJson[0]["USER_KEY_ASSETS"][0].ToString());
                data.User_Key_Assets[1] = int.Parse(gameDataJson[0]["USER_KEY_ASSETS"][1].ToString());

                data.Dungeon_Clear_Level[0] = int.Parse(gameDataJson[0]["DUNGEON_CLEAR_LEVEL"][0].ToString());
                data.Dungeon_Clear_Level[1] = int.Parse(gameDataJson[0]["DUNGEON_CLEAR_LEVEL"][1].ToString());

                DateTime startDate = data.StartDate;
                DateTime endDate = data.EndDate;


                data.isBuyADPackage = bool.Parse(gameDataJson[0]["isBUY_AD_Package"].ToString());
                data.isBuyLAUNCH_EVENT = bool.Parse(gameDataJson[0]["isBUY_LAUNCH_EVENT"].ToString());
                data.isBuySTRONGPackage = bool.Parse(gameDataJson[0]["isBUY_STRONG_Package"].ToString());
                data.isBuyTodayPackage = bool.Parse(gameDataJson[0]["isBUY_TODAY_Package"].ToString());
                data.Event_Push_Alarm_Agree = bool.Parse(gameDataJson[0]["EVENT_PUSH_ALARM"].ToString());
                data.Daily_Attendance = int.Parse(gameDataJson[0]["Daily_Attendance"].ToString());
                data.Levelup = int.Parse(gameDataJson[0]["Daily_Levelup"].ToString());
                data.Summon = int.Parse(gameDataJson[0]["Daily_Summon"].ToString());
                data.Relic = int.Parse(gameDataJson[0]["Daily_Relic"].ToString());
                data.Dungeon_Gold = int.Parse(gameDataJson[0]["Daily_Dungeon_Gold"].ToString());
                data.Dungeon_Dia = int.Parse(gameDataJson[0]["Daily_Dungeon_Dia"].ToString());

                data.Daily_Attendance_Clear = bool.Parse(gameDataJson[0]["Daily_Attendance_Clear"].ToString());
                data.Level_up_Clear = bool.Parse(gameDataJson[0]["Daily_Levelup_Clear"].ToString());
                data.Summon_Clear = bool.Parse(gameDataJson[0]["Daily_Summon_Clear"].ToString());
                data.Relic_Clear = bool.Parse(gameDataJson[0]["Daily_Relic_Clear"].ToString());
                data.Dungeon_Gold_Clear = bool.Parse(gameDataJson[0]["Daily_Dungeon_Gold_Clear"].ToString());
                data.Dungeon_Dia_Clear = bool.Parse(gameDataJson[0]["Daily_Dungeon_Dia_Clear"].ToString());
                data.ADS_Hero_Summon_Count = int.Parse(gameDataJson[0]["ADS_HERO_SUMMON_COUNT"].ToString());
                data.ADS_Relic_Summon_Count = int.Parse(gameDataJson[0]["ADS_RELIC_SUMMON_COUNT"].ToString());
                data.isFastMode = bool.Parse(gameDataJson[0]["Fast_Mode"].ToString());
                


                if (Get_Date_Dungeon_Item(startDate, endDate))
                {
                    //���� ���� ����� �ʱ�ȭ
                    data.Daily_Enter_Key[0] = 2;
                    data.Daily_Enter_Key[1] = 2;

                    //���� ����Ʈ �ʱ�ȭ
                    data.Daily_Attendance = 1;
                    data.Levelup = 0;
                    data.Summon = 0;
                    data.Dungeon_Dia = 0;
                    data.Dungeon_Gold = 0;
                    data.Relic = 0;

                    data.Daily_Attendance_Clear = false;
                    data.Level_up_Clear = false;
                    data.Summon_Clear = false;
                    data.Dungeon_Dia_Clear = false;
                    data.Dungeon_Gold_Clear = false;
                    data.Relic_Clear = false;

                    data.ADS_Hero_Summon_Count = 0;
                    data.ADS_Relic_Summon_Count = 0;

                    data.isBuyTodayPackage = false;
                    data.isBuySTRONGPackage = false;

                }

                Data_Manager.Main_Players_Data = data;
                Data_Manager.Main_Players_Data.Player_Tier = (Player_Tier)tier_number;

                #region ������ ���� Ƽ�� �ʱ�ȭ
                if (Utils.TIER_SEASON >= data.Season)
                {
                    data.Player_Tier = Player_Tier.Tier_Beginner;
                    data.Season = Utils.TIER_SEASON;
                }
                #endregion

                Utils.Calculate_ADS_Timer(); // �������� �ð���ŭ ���� �� �ð� ����
                Utils.Calculate_ADS_Buff_Timer(); // �������� �ð���ŭ ������� �ð� ����
                Utils.Calculate_ADS_X2_SPEED_Timer(); // �������� �ð���ŭ ���� 2��� �ð� ����


                Base_Manager.Data.Init();

                Debug.Log("USER ���̺� �����͸� ���������� �ҷ��� �����͸� ������Ʈ �Ͽ����ϴ�.");

            }
        }
        else
        {
            Debug.LogError("������ ��ȸ�� �����߽��ϴ�. : " + bro);
        }

        #endregion

        #region CHARACTER DATA
        Debug.Log("'CHARACTER' ���̺��� �����͸� ��ȸ�ϴ� �Լ��� ȣ���մϴ�.");
        var char_bro = Backend.GameData.GetMyData("CHARACTER", new Where());

        if (char_bro.Rows().Count > 0)
        {

            var rows = BackendReturnObject.Flatten(char_bro.Rows());

            foreach (JsonData row in rows)
            {
                if (row.ContainsKey("character"))
                {
                    string charJsonData = row["character"].ToString();
                    Dictionary<string, JsonData> characterData = JsonConvert.DeserializeObject<Dictionary<string, JsonData>>(charJsonData);


                    foreach (var dict in characterData.Keys)
                    {
                        int hero_level = characterData[dict].ContainsKey("Hero_Level")
                           ? int.Parse(characterData[dict]["Hero_Level"].ToString()) : 0;

                        // 'Hero_Card_Amount' �� �������� (���� ����)
                        int hero_card_amount = characterData[dict].ContainsKey("Hero_Card_Amount")
                            ? int.Parse(characterData[dict]["Hero_Card_Amount"].ToString()) : 0;

                        // Holder ��ü ���� �� Dictionary�� �߰�

                        Holder holder = new Holder { Hero_Level = hero_level, Hero_Card_Amount = hero_card_amount };

                        Base_Manager.Data.character_Holder[dict] = holder;
                    }
                }
                
            }

            Set_Character_Data_Dictionary();
            Base_Manager.Data.Init();

            Debug.Log("���� ���� ������ �ҷ����⿡ �����Ͽ����ϴ�.");
        }
        else
        {
            Debug.LogError("���� ���� ������ ��ȸ�� �����߽��ϴ�. : " + char_bro);
        }
        #endregion

        #region ITEM_DATA
        Debug.Log("'ITEM' ���̺��� �����͸� ��ȸ�ϴ� �Լ��� ȣ���մϴ�.");
        var item_bro = Backend.GameData.GetMyData("ITEM", new Where());

        if (item_bro.Rows().Count > 0)
        {

            var rows = BackendReturnObject.Flatten(item_bro.Rows());

            foreach (JsonData row in rows)
            {
                if (row.ContainsKey("Item"))
                {
                    string charJsonData = row["Item"].ToString();
                    Dictionary<string, JsonData> characterData = JsonConvert.DeserializeObject<Dictionary<string, JsonData>>(charJsonData);


                    foreach (var dict in characterData.Keys)
                    {
                        int Item_level = characterData[dict].ContainsKey("Hero_Level")
                           ? int.Parse(characterData[dict]["Hero_Level"].ToString()) : 0;

                        // 'Hero_Card_Amount' �� �������� (���� ����)
                        int Item_card_amount = characterData[dict].ContainsKey("Hero_Card_Amount")
                            ? int.Parse(characterData[dict]["Hero_Card_Amount"].ToString()) : 0;

                        // Holder ��ü ���� �� Dictionary�� �߰�

                        Holder holder = new Holder { Hero_Level = Item_level, Hero_Card_Amount = Item_card_amount };

                        Base_Manager.Data.Item_Holder[dict] = holder;
                    }
                }

            }
        
            Base_Manager.Data.Init();

            Debug.Log("�κ��丮 ������ �ҷ����⿡ �����Ͽ����ϴ�.");
        }
        else
        {
            Debug.LogError("�κ��丮 ������ ��ȸ�� �����߽��ϴ�. : " + item_bro);
        }

        #endregion

        #region SMELT_DATA

        Debug.Log("'SMELT' ���̺��� �����͸� ��ȸ�ϴ� �Լ��� ȣ���մϴ�.");
        var smelt_bro = Backend.GameData.GetMyData("SMELT", new Where());

        if (smelt_bro.Rows().Count > 0)
        {

            var rows = BackendReturnObject.Flatten(smelt_bro.Rows());

            foreach (JsonData row in rows)
            {
                if (row.ContainsKey("Smelt"))
                {
                    string Smelt_Json_Data = row["Smelt"].ToString();
                    List<Smelt_Holder> smeltList = JsonConvert.DeserializeObject<List<Smelt_Holder>>(Smelt_Json_Data);
                    Base_Manager.Data.User_Main_Data_Smelt_Array = smeltList;                 
                }

                else
                {
                    Debug.LogWarning("'Smelt' �����Ͱ� �ùٸ� JSON ������ �ƴմϴ�.");
                }

            }

            Base_Manager.Data.Init();

            Debug.Log("���� ���� ������ �ҷ����⿡ �����Ͽ����ϴ�.");
        }
        else
        {
            Debug.LogError("���� ���� ������ ��ȸ�� �����߽��ϴ�. : " + smelt_bro);
        }
        #endregion

        #region PLAYER_SET_HERO_DATA

        Debug.Log("'PLAYER_SET_HERO' ���̺��� �����͸� ��ȸ�ϴ� �Լ��� ȣ���մϴ�.");
        var player_set_hero_bro = Backend.GameData.GetMyData("PLAYER_SET_HERO", new Where());

        if (player_set_hero_bro.Rows().Count > 0)
        {

            var rows = BackendReturnObject.Flatten(player_set_hero_bro.Rows());

            foreach (JsonData row in rows)
            {
                if (row.ContainsKey("Player_Set_Hero"))
                {
                    string Player_Set_Hero_Data = row["Player_Set_Hero"].ToString();

                    if (string.IsNullOrEmpty(Player_Set_Hero_Data))
                    {
                        Debug.LogError("���� ��ġ �����Ͱ� ����ֽ��ϴ�.");
                        return;
                    }
                    Character_Holder[] char_holder = JsonConvert.DeserializeObject<Character_Holder[]>(Player_Set_Hero_Data);
                    Base_Manager.Character.Set_Character = char_holder;
                }

                else
                {
                    Debug.LogWarning(" 'Player_Set_Hero' �����Ͱ� �ùٸ� JSON ������ �ƴմϴ�.");
                }

            }


            Base_Manager.Data.Init();
            
            Debug.Log("���� ��ġ ������ �ҷ����⿡ �����Ͽ����ϴ�.");
        }
        else
        {
            Debug.LogError("���� ��ġ ������ ��ȸ�� �����߽��ϴ�. : " + player_set_hero_bro);
        }
        #endregion

        #region PLAYER_SET_RELIC_DATA

        Debug.Log("'PLAYER_SET_RELIC' ���̺��� �����͸� ��ȸ�ϴ� �Լ��� ȣ���մϴ�.");
        var player_set_relic_bro = Backend.GameData.GetMyData("PLAYER_SET_RELIC", new Where());

        if (player_set_relic_bro.Rows().Count > 0)
        {

            var rows = BackendReturnObject.Flatten(player_set_relic_bro.Rows());

            foreach (JsonData row in rows)
            {
                if (row.ContainsKey("Player_Set_Relic"))
                {
                    string Player_Set_Relic_Data = row["Player_Set_Relic"].ToString();

                    if (string.IsNullOrEmpty(Player_Set_Relic_Data))
                    {
                        Debug.LogError("���� ��ġ �����Ͱ� ����ֽ��ϴ�.");
                        return;
                    }
                    Item_Scriptable[] relic_holder = JsonConvert.DeserializeObject<Item_Scriptable[]>(Player_Set_Relic_Data);
                    Base_Manager.Data.Main_Set_Item = relic_holder;
                }

                else
                {
                    Debug.LogWarning(" 'Player_Set_Relic' �����Ͱ� �ùٸ� JSON ������ �ƴմϴ�.");
                }

            }


            Base_Manager.Data.Init();

            Debug.Log("���� ��ġ ������ �ҷ����⿡ �����Ͽ����ϴ�.");
        }
        else
        {
            Debug.LogError("���� ��ġ ������ ��ȸ�� �����߽��ϴ�. : " + player_set_relic_bro);
        }
        #endregion
    }
    /// <summary>
    /// ��¥�� ������ �������� Ȯ���ϰ�, ���ϸ� ������� ������ �� �Ǵ��մϴ�.
    /// </summary>
    /// <param name="startdate"></param>
    /// <param name="enddate"></param>
    /// <returns></returns>
    private bool Get_Date_Dungeon_Item(DateTime startdate, DateTime enddate)
    {
        if (startdate.Day != enddate.Day)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
 
    /// <summary>
    /// �ε�� Character_Holder�� �̿��Ͽ�, Data_Character_Dictionary�� �����մϴ�.
    /// </summary>
    private void Set_Character_Data_Dictionary()
    {
        // Scriptable ���ϰ� ��Ī�Ͽ� Data_Character_Dictionary�� ����
        Base_Manager.Data.Data_Character_Dictionary.Clear(); // ���� �����͸� �ʱ�ȭ

        var scriptableCharacters = Resources.LoadAll<Character_Scriptable>("Scriptable/Character");

        foreach (var characterScriptable in scriptableCharacters)
        {
            if (Base_Manager.Data.character_Holder.ContainsKey(characterScriptable.Character_EN_Name))
            {
                // ĳ���� �����͸� Scriptable�� ��Ī
                var holderData = Base_Manager.Data.character_Holder[characterScriptable.Character_EN_Name];

                var characterHolder = new Character_Holder
                {
                    Data = characterScriptable,
                    holder = holderData
                };

                Base_Manager.Data.Data_Character_Dictionary[characterScriptable.Character_EN_Name] = characterHolder;

            }

        }
    }

}
