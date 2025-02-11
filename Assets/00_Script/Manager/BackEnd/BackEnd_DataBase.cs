using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using BackEnd;
using BackEnd.BackndNewtonsoft.Json.Linq;
using LitJson;

public partial class BackEnd_Manager : MonoBehaviour
{
    /// <summary>
    /// ���� �����͸� �����մϴ�. BackendReturnObject�� ������ ����� ������� �ǹ��մϴ�.
    /// </summary>
    public void WriteData()
    {
        Debug.Log("WriteData �޼��� ȣ��, �����͸� ����մϴ�.");
        #region DEFAULT DATA

        if (Data_Manager.Main_Players_Data == null)
        {
            Debug.LogError("�����Ͱ� �������� �ʽ��ϴ�. Initialize ���� �����͸� �������ּ���.");
            return;
        }
        
        Param param = new Param();

        param.Add("NICK_NAME", Data_Manager.Main_Players_Data.Nick_Name);
        param.Add("ATK", Data_Manager.Main_Players_Data.ATK);
        param.Add("HP", Data_Manager.Main_Players_Data.HP);
        param.Add("PLAYER_MONEY", Data_Manager.Main_Players_Data.Player_Money);
        param.Add("DIAMOND", Data_Manager.Main_Players_Data.DiaMond);
        param.Add("PLAYER_LEVEL", Data_Manager.Main_Players_Data.Player_Level);
        param.Add("PLAYER_EXP", Data_Manager.Main_Players_Data.EXP);
        param.Add("PLAYER_STAGE", Data_Manager.Main_Players_Data.Player_Stage);
        param.Add("EXP_UPGRADE_COUNT", Data_Manager.Main_Players_Data.EXP_Upgrade_Count);
        param.Add("BUFF_TIMER", Data_Manager.Main_Players_Data.Buff_Timers);
        param.Add("SPEED", Data_Manager.Main_Players_Data.buff_x2_speed);
        param.Add("BUFF_LEVEL", Data_Manager.Main_Players_Data.Buff_Level);
        param.Add("BUFF_LEVEL_COUNT", Data_Manager.Main_Players_Data.Buff_Level_Count);
        param.Add("QUEST_COUNT", Data_Manager.Main_Players_Data.Quest_Count);
        param.Add("HERO_SUMMON_COUNT", Data_Manager.Main_Players_Data.Hero_Summon_Count);
        param.Add("HERO_PICKUP_COUNT", Data_Manager.Main_Players_Data.Hero_Pickup_Count);
        param.Add("RELIC_SUMMON_COUNT", Data_Manager.Main_Players_Data.Relic_Summon_Count);
        param.Add("RELIC_PICKUP_COUNT", Data_Manager.Main_Players_Data.Relic_Pickup_Count);
        param.Add("START_DATE", Data_Manager.Main_Players_Data.StartDate);
        param.Add("END_DATE", DateTime.Now.ToString());
        param.Add("DAILY_ENTER_KEY", Data_Manager.Main_Players_Data.Daily_Enter_Key);
        param.Add("USER_KEY_ASSETS", Data_Manager.Main_Players_Data.User_Key_Assets);
        param.Add("DUNGEON_CLEAR_LEVEL", Data_Manager.Main_Players_Data.Dungeon_Clear_Level);
        
        Debug.Log("���� �⺻ �����͸� �����մϴ�");

        var bro = Backend.GameData.Update("USER", new Where(), param);

        if (bro.IsSuccess())
        {
            Debug.Log("���� �⺻ ������ ������ �����߽��ϴ�. : " + bro);
        }
        else
        {
            Debug.LogError("���� �⺻ ������ ������ �����߽��ϴ�. : " + bro);
        }

        
        #endregion

        #region CHARACTER DATA

        Param character_param = new Param();
        string char_Json_Data = JsonConvert.SerializeObject(Base_Manager.Data.character_Holder);
        character_param.Add("character", char_Json_Data);

        Debug.Log("���� ���� �����͸� �����մϴ�");

        var character_bro = Backend.GameData.Update("CHARACTER", new Where(), character_param);

        if (character_bro.IsSuccess())
        {
            Debug.Log("���� ���� ������ ������ �����߽��ϴ�. : " + character_bro);
        }
        else
        {
            Debug.LogError("���� ���� ������ ������ �����߽��ϴ�. : " + character_bro);
        }
        #endregion

        #region ITEM_DATA
        Param item_param = new Param();
        string Json_item_Data = JsonConvert.SerializeObject(Base_Manager.Data.Item_Holder);
        item_param.Add("Item", Json_item_Data);

        Debug.Log("�κ��丮 �����͸� �����մϴ�");

        var item_bro = Backend.GameData.Update("ITEM", new Where(), item_param);

        if (item_bro.IsSuccess())
        {
            Debug.Log("���� �κ��丮 ������ ������ �����߽��ϴ�. : " + item_bro);
        }
        else
        {
            Debug.LogError("���� �κ��丮 ������ ������ �����߽��ϴ�. : " + item_bro);
        }
        #endregion

        #region SMELT_DATA
        Param smelt_param = new Param();
        string Json_Smelt_Data = JsonConvert.SerializeObject(Base_Manager.Data.User_Main_Data_Smelt_Array); 
        smelt_param.Add("Smelt", Json_Smelt_Data);
     
        Debug.Log("���� ���� �����͸� �����մϴ�");

        var smelt_bro = Backend.GameData.Update("SMELT", new Where(), smelt_param);

        if (smelt_bro.IsSuccess())
        {
            Debug.Log("���� ���� ���� ������ ������ �����߽��ϴ�. : " + smelt_bro);
        }
        else
        {
            Debug.LogError("���� ���� ���� ������ ������ �����߽��ϴ�. : " + smelt_bro);
        }
        #endregion
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

                data.Nick_Name = gameDataJson[0]["NICK_NAME"].ToString();
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

                data.buff_x2_speed = float.Parse(gameDataJson[0]["SPEED"].ToString());
                data.Buff_Level = int.Parse(gameDataJson[0]["BUFF_LEVEL"].ToString());
                data.Buff_Level_Count = int.Parse(gameDataJson[0]["BUFF_LEVEL_COUNT"].ToString());
                data.Quest_Count = int.Parse(gameDataJson[0]["QUEST_COUNT"].ToString());


                data.Hero_Summon_Count = int.Parse(gameDataJson[0]["HERO_SUMMON_COUNT"].ToString());
                data.Hero_Pickup_Count = int.Parse(gameDataJson[0]["HERO_PICKUP_COUNT"].ToString());
                data.Relic_Summon_Count = int.Parse(gameDataJson[0]["RELIC_SUMMON_COUNT"].ToString());
                data.Relic_Pickup_Count = int.Parse(gameDataJson[0]["RELIC_PICKUP_COUNT"].ToString());

                data.StartDate = DateTime.Now.ToString();
                data.EndDate = gameDataJson[0]["END_DATE"].ToString();
                

                data.Daily_Enter_Key[0] = int.Parse(gameDataJson[0]["DAILY_ENTER_KEY"][0].ToString());
                data.Daily_Enter_Key[1] = int.Parse(gameDataJson[0]["DAILY_ENTER_KEY"][1].ToString());

                data.User_Key_Assets[0] = int.Parse(gameDataJson[0]["USER_KEY_ASSETS"][0].ToString());
                data.User_Key_Assets[1] = int.Parse(gameDataJson[0]["USER_KEY_ASSETS"][1].ToString());

                data.Dungeon_Clear_Level[0] = int.Parse(gameDataJson[0]["DUNGEON_CLEAR_LEVEL"][0].ToString());
                data.Dungeon_Clear_Level[1] = int.Parse(gameDataJson[0]["DUNGEON_CLEAR_LEVEL"][1].ToString());


                //if (string.IsNullOrEmpty(data.StartDate))
                //{
                //    /*data.EndDate�� null �Ǵ� �� ���ڿ��϶�,
                //    DateTime.Parse�� ȣ��Ǹ� FormatException�� �߻�.
                //    �޼��尡 ���ܸ� ó������ ���� ��� �޼��尡 �ߵ������..*/

                //    Data_Manager.Main_Players_Data.StartDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //    Debug.Log("StartDate�� ��� �⺻������ ����: " + data.StartDate);
                //}

                //if (string.IsNullOrEmpty(data.EndDate))
                //{
                //    /*data.EndDate�� null �Ǵ� �� ���ڿ��϶�,
                //    DateTime.Parse�� ȣ��Ǹ� FormatException�� �߻�.
                //    �޼��尡 ���ܸ� ó������ ���� ��� �޼��尡 �ߵ������..*/

                //    Data_Manager.Main_Players_Data.EndDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //    Debug.Log("EndDate�� ��� �⺻������ ����: " + data.EndDate);
                //}

                DateTime startDate = DateTime.Parse(data.StartDate);
                DateTime endDate = DateTime.Parse(data.EndDate);

                if (Get_Date_Dungeon_Item(startDate, endDate))
                {
                    data.Daily_Enter_Key[0] = 2;
                    data.Daily_Enter_Key[1] = 2;
                }

                Data_Manager.Main_Players_Data = data;

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
            if (Base_Manager.Data.character_Holder.ContainsKey(characterScriptable.M_Character_Name))
            {
                // ĳ���� �����͸� Scriptable�� ��Ī
                var holderData = Base_Manager.Data.character_Holder[characterScriptable.M_Character_Name];

                var characterHolder = new Character_Holder
                {
                    Data = characterScriptable,
                    holder = holderData
                };

                Base_Manager.Data.Data_Character_Dictionary[characterScriptable.M_Character_Name] = characterHolder;

            }

        }
    }

}
