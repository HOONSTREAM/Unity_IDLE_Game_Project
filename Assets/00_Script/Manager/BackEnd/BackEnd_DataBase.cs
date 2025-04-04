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
    /// ���� �����͸� �����մϴ�. BackendReturnObject�� ������ ����� ������� �ǹ��մϴ�.
    /// </summary>
    public async Task WriteData()
    {
       
        Debug.Log("WriteData �޼��� ȣ��, �����͸� ����մϴ�.");

            #region DEFAULT DATA

        if (Data_Manager.Main_Players_Data == null)
        {
            Debug.LogError("�����Ͱ� �������� �ʽ��ϴ�. Initialize ���� �����͸� �������ּ���.");
            return;
        }

        try
        {
            Param param = new Param();

            param.Add("ATK", Data_Manager.Main_Players_Data.ATK); // �÷��̾� ���ݷ�
            param.Add("HP", Data_Manager.Main_Players_Data.HP); // �÷��̾� ü��
            param.Add("PLAYER_MONEY", Data_Manager.Main_Players_Data.Player_Money); // �÷��̾� ��� ������
            param.Add("DIAMOND", Data_Manager.Main_Players_Data.DiaMond); //�÷��̾� ���̾Ƹ�� ������
            param.Add("PLAYER_LEVEL", Data_Manager.Main_Players_Data.Player_Level); // �÷��̾� ����
            param.Add("PLAYER_EXP", Data_Manager.Main_Players_Data.EXP); // �÷��̾� ����ġ
            param.Add("PLAYER_STAGE", Data_Manager.Main_Players_Data.Player_Stage); // �÷��̾� ��������
            param.Add("EXP_UPGRADE_COUNT", Data_Manager.Main_Players_Data.EXP_Upgrade_Count); // ������ ��ư ī��Ʈ
            param.Add("BUFF_TIMER", Data_Manager.Main_Players_Data.Buff_Timers); // �������
            param.Add("ADS_TIMER", Data_Manager.Main_Players_Data.ADS_Timer); // ��ȯ ���� ��û ��Ÿ��
            param.Add("SPEED", Data_Manager.Main_Players_Data.buff_x2_speed); // 2���       
            param.Add("QUEST_COUNT", Data_Manager.Main_Players_Data.Quest_Count); // ����Ʈ �ܰ�
            param.Add("HERO_SUMMON_COUNT", Data_Manager.Main_Players_Data.Hero_Summon_Count); // ������ȯ ī��Ʈ
            param.Add("HERO_PICKUP_COUNT", Data_Manager.Main_Players_Data.Hero_Pickup_Count); // ���� Ȯ����ȯ ī��Ʈ
            param.Add("RELIC_SUMMON_COUNT", Data_Manager.Main_Players_Data.Relic_Summon_Count); // ������ȯ Ƚ�� ī��Ʈ
            param.Add("RELIC_PICKUP_COUNT", Data_Manager.Main_Players_Data.Relic_Pickup_Count); // Ȯ����ȯ ī��Ʈ
            param.Add("START_DATE", Data_Manager.Main_Players_Data.StartDate); // ���� ���۽ð�
            param.Add("END_DATE", Utils.Get_Server_Time()); // �����ð�        
            param.Add("DAILY_ENTER_KEY", Data_Manager.Main_Players_Data.Daily_Enter_Key); // ���ϸ� ���� ���� ����Ű
            param.Add("USER_KEY_ASSETS", Data_Manager.Main_Players_Data.User_Key_Assets); // ������ ���ų�, �߰��� ���޹��� ���� ����Ű
            param.Add("DUNGEON_CLEAR_LEVEL", Data_Manager.Main_Players_Data.Dungeon_Clear_Level); // ���� ���� Ŭ���� ���� (�迭)


            param.Add("isBUY_AD_Package", Data_Manager.Main_Players_Data.isBuyADPackage); // �������� ��Ű�� ���ſ���
            param.Add("ADS_HERO_SUMMON_COUNT", Data_Manager.Main_Players_Data.ADS_Hero_Summon_Count); // �����û ������ȯ ī��Ʈ
            param.Add("ADS_RELIC_SUMMON_COUNT", Data_Manager.Main_Players_Data.ADS_Relic_Summon_Count); // �����û ������ȯ ī��Ʈ
            param.Add("EVENT_PUSH_ALARM", Data_Manager.Main_Players_Data.Event_Push_Alarm_Agree); // Ǫ�þ˶� ���ǿ���

            #region ��������Ʈ ���� ������
            param.Add("Daily_Attendance", Data_Manager.Main_Players_Data.Daily_Attendance); // ��������Ʈ �⼮
            param.Add("Daily_Levelup", Data_Manager.Main_Players_Data.Levelup); //��������Ʈ ������ ���൵ (24�� ���� �ʱ�ȭ)
            param.Add("Daily_Summon", Data_Manager.Main_Players_Data.Summon); //��������Ʈ ������ȯ ���൵ (24�� ���� �ʱ�ȭ)
            param.Add("Daily_Relic", Data_Manager.Main_Players_Data.Relic); //��������Ʈ ������ȯ ���൵ (24�� ���� �ʱ�ȭ)
            param.Add("Daily_Dungeon_Gold", Data_Manager.Main_Players_Data.Dungeon_Gold); //��� ���� Ŭ���� Ƚ�� (24�� ���� �ʱ�ȭ)
            param.Add("Daily_Dungeon_Dia", Data_Manager.Main_Players_Data.Dungeon_Dia); //���̾� ���� Ŭ���� Ƚ�� (24�� ���� �ʱ�ȭ)

            param.Add("Daily_Attendance_Clear", Data_Manager.Main_Players_Data.Daily_Attendance_Clear); // �⼮üũ Ŭ���� ����
            param.Add("Daily_Levelup_Clear", Data_Manager.Main_Players_Data.Level_up_Clear); // ������ Ŭ���� ����
            param.Add("Daily_Summon_Clear", Data_Manager.Main_Players_Data.Summon_Clear); // ������ȯ Ŭ���� ����
            param.Add("Daily_Relic_Clear", Data_Manager.Main_Players_Data.Relic_Clear); // ������ȯ Ŭ���� ����
            param.Add("Daily_Dungeon_Gold_Clear", Data_Manager.Main_Players_Data.Dungeon_Gold_Clear); // ������ Ŭ���� ����
            param.Add("Daily_Dungeon_Dia_Clear", Data_Manager.Main_Players_Data.Dungeon_Dia_Clear); // ���̾ƴ��� Ŭ���� ����
            param.Add("Fast_Mode", Data_Manager.Main_Players_Data.isFastMode); // �н�Ʈ��� Ȱ��ȭ ����
            #endregion

            var bro = Backend.GameData.Get("USER", new Where());

            if (bro.IsSuccess())
            {
                var inDate = bro.GetInDate();

                Backend.Leaderboard.User.UpdateMyDataAndRefreshLeaderboard(Utils.LEADERBOARD_UUID, "USER", inDate, param, (callback) =>
                {
                    if (callback.IsSuccess())
                    {
                        Debug.Log("���� �⺻ ������ ������Ʈ �� �������� ���ſ� �����Ͽ����ϴ�.");
                    }
                    else
                    {
                        Debug.LogWarning("���� �⺻ ������ ������Ʈ �� �������� ���ſ� �����Ͽ����ϴ�.");

                        Backend.Leaderboard.User.GetMyLeaderboard(Utils.LEADERBOARD_UUID, callback => 
                        {
                            if (!callback.IsSuccess())
                            {
                                Debug.Log("�������忡 ��ϵǾ� �����ʽ��ϴ�. ���� �� �� ���� �õ��մϴ�.");

                                var bro = Backend.GameData.GetMyData("USER", new Where());
                                string inDate = bro.GetInDate();
                                var _bro = Backend.URank.User.UpdateUserScore(Utils.LEADERBOARD_UUID, "USER", inDate, param); // �������忡 ���������͸� ����մϴ�.

                                if (_bro.IsSuccess())
                                {
                                    Debug.Log($"�������� ��Ͽ� �����Ͽ����ϴ�. : {callback}");

                                }
                                else
                                {
                                    Debug.LogError($"�������� ��Ͽ� �����Ͽ����ϴ�. : {callback}");
                                    //TODO : �ϴ� ������Ʈ��.
                                }
                            }

                            else
                            {
                                Debug.LogError($"�������� �̵�� ���� ��, �ٸ� ������ ������ �ֽ��ϴ�. : {callback}");
                                return;
                            }


                        });
                     }
                });

            }

            else
            {
                return;
            }

            await Task.Yield();

            #endregion

            #region CHARACTER DATA

            Param character_param = new Param();
            string char_Json_Data = JsonConvert.SerializeObject(Base_Manager.Data.character_Holder);
            character_param.Add("character", char_Json_Data);


            Backend.GameData.Update("CHARACTER", new Where(), character_param, (callback) =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("���� ���� ���� ������ ������Ʈ�� �����Ͽ����ϴ�.");
                }
                else
                {
                    Debug.LogError("���� ���� ���� ������ ������Ʈ�� �����Ͽ����ϴ�.");
                }
            });

            await Task.Yield();

            #endregion

            #region ITEM_DATA
            Param item_param = new Param();
            string Json_item_Data = JsonConvert.SerializeObject(Base_Manager.Data.Item_Holder);
            item_param.Add("Item", Json_item_Data);

            Backend.GameData.Update("ITEM", new Where(), item_param, (callback) =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("���� �κ��丮 ���� ������ ������Ʈ�� �����Ͽ����ϴ�.");
                }
                else
                {
                    Debug.LogError("���� �κ��丮 ���� ������ ������Ʈ�� �����Ͽ����ϴ�.");
                }
            });


            #endregion

            #region SMELT_DATA
            Param smelt_param = new Param();
            string Json_Smelt_Data = JsonConvert.SerializeObject(Base_Manager.Data.User_Main_Data_Smelt_Array);
            smelt_param.Add("Smelt", Json_Smelt_Data);


            Backend.GameData.Update("SMELT", new Where(), smelt_param, (callback) =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("���� ���� ������ ������Ʈ�� �����Ͽ����ϴ�.");
                }
                else
                {
                    Debug.LogError("���� ���� ������ ������Ʈ�� �����Ͽ����ϴ�.");
                }
            });

            await Task.Yield();

            #endregion

            #region PLAYER_SET_HERO_DATA
            Param Player_Set_Hero_param = new Param();
            string Json_Player_Set_Hero_data = JsonConvert.SerializeObject(Base_Manager.Character.Set_Character);
            Player_Set_Hero_param.Add("Player_Set_Hero", Json_Player_Set_Hero_data);

            Debug.Log("���� ���� ��ġ �����͸� �����մϴ�");


            Backend.GameData.Update("PLAYER_SET_HERO", new Where(), Player_Set_Hero_param, (callback) =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("���� ���� ��ġ ������ ������Ʈ�� �����Ͽ����ϴ�.");
                }
                else
                {
                    Debug.LogError("���� ���� ��ġ ������ ������Ʈ�� �����Ͽ����ϴ�.");
                }
            });

            await Task.Yield();


            #endregion

            #region PLAYER_SET_RELIC_DATA
            Param Player_Set_Relic_Param = new Param();
            string Json_Player_Set_Relic_data = JsonConvert.SerializeObject(Base_Manager.Data.Main_Set_Item);
            Player_Set_Relic_Param.Add("Player_Set_Relic", Json_Player_Set_Relic_data);

            Debug.Log("���� ���� ��ġ �����͸� �����մϴ�");

            Backend.GameData.Update("PLAYER_SET_RELIC", new Where(), Player_Set_Relic_Param, (callback) =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("���� ���� ��ġ ������ ������Ʈ�� �����Ͽ����ϴ�.");
                    Relic_Manager.instance.Initalize(); // ������ ������ ��Ƽ��ȿ���� �����ŵ�ϴ�.
                }
                else
                {
                    Debug.LogError("���� ���� ��ġ ������ ������Ʈ�� �����Ͽ����ϴ�.");
                }
            });

            await Task.Yield();

            #endregion
        }

        catch (Exception e)
        {
            Debug.LogError($"[ERROR] WriteDataAsync() ���� �� ���� �߻�: {e.Message}");
        }
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
             
                data.ATK = double.Parse(gameDataJson[0]["ATK"].ToString());
                data.HP = double.Parse(gameDataJson[0]["HP"].ToString());
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

                }

                Data_Manager.Main_Players_Data = data;

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
