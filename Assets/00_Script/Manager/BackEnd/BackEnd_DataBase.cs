using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using BackEnd;
using BackEnd.BackndNewtonsoft.Json.Linq;

public partial class BackEnd_Manager : MonoBehaviour
{
    /// <summary>
    /// ���� �����͸� �����մϴ�.
    /// </summary>
    public void WriteData()
    {
        Debug.Log("�����͸� ����մϴ�.");
        #region DEFAULT DATA

        if (Data_Manager.Main_Players_Data == null)
        {
            Debug.LogError("�����Ͱ� �������� �ʽ��ϴ�. Initialize Ȥ�� Get�� ���� �����͸� �������ּ���.");
            return;
        }
        if (string.IsNullOrEmpty(Data_Manager.Main_Players_Data.StartDate))
        {
            /*data.EndDate�� null �Ǵ� �� ���ڿ��϶�,
            DateTime.Parse�� ȣ��Ǹ� FormatException�� �߻�.
            �޼��尡 ���ܸ� ó������ ���� ��� �޼��尡 �ߵ������..*/

            Data_Manager.Main_Players_Data.StartDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Debug.Log("StartDate�� ��� �⺻������ ����: " + Data_Manager.Main_Players_Data.StartDate);
        }

        if (string.IsNullOrEmpty(Data_Manager.Main_Players_Data.EndDate))
        {
            /*data.EndDate�� null �Ǵ� �� ���ڿ��϶�,
            DateTime.Parse�� ȣ��Ǹ� FormatException�� �߻�.
            �޼��尡 ���ܸ� ó������ ���� ��� �޼��尡 �ߵ������..*/

            Data_Manager.Main_Players_Data.EndDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Debug.Log("EndDate�� ��� �⺻������ ����: " + Data_Manager.Main_Players_Data.EndDate);
        }
        DateTime LastDate = DateTime.Parse(Data_Manager.Main_Players_Data.EndDate);
        Data_Manager.Main_Players_Data.EndDate = DateTime.Now.ToString();

        if (Get_Date_Dungeon_Item(LastDate, DateTime.Now))
        {
            Data_Manager.Main_Players_Data.Daily_Enter_Key[0] = 2;
            Data_Manager.Main_Players_Data.Daily_Enter_Key[1] = 2;
        }
        Debug.Log("����ð� : " + Data_Manager.Main_Players_Data.EndDate);

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

        BackendReturnObject bro = null;

        Debug.Log("���� �����͸� �����մϴ�");

        bro = Backend.GameData.Update("USER", new Where(), param);

        if (bro.IsSuccess())
        {
            Debug.Log("������ ������ �����߽��ϴ�. : " + bro);
        }
        else
        {
            Debug.LogError("������ ������ �����߽��ϴ�. : " + bro);
        }

        //string DefalutJson = JsonUtility.ToJson(data);

        //DB_reference.Child("USER").Child(currentUser.UserId).Child("DATA").SetRawJsonValueAsync(DefalutJson).ContinueWithOnMainThread(task =>
        //{
        //    if (task.IsCompleted)
        //    {
        //        Debug.Log("Child.DATA ������ ���� �����Ͽ����ϴ�.");
        //    }
        //    else
        //    {
        //        Debug.LogError("������ ���� ���� : " + task.Exception.ToString());
        //    }

        //});
        #endregion

        #region CHARACTER DATA

        string Character_Json = JsonConvert.SerializeObject(Base_Manager.Data.character_Holder);

        //DB_reference.Child("USER").Child(currentUser.UserId).Child("CHARACTER").SetRawJsonValueAsync(Character_Json).ContinueWithOnMainThread(task =>
        //{
        //    if (task.IsCompleted)
        //    {
        //        Debug.Log(" Child.character ������ ���� �����Ͽ����ϴ�.");
        //    }
        //    else
        //    {
        //        Debug.LogError("������ ���� ���� : " + task.Exception.ToString());
        //    }

        //});
        #endregion

        #region ITEM_DATA
        string Item_Json = JsonConvert.SerializeObject(Base_Manager.Data.Item_Holder);

        //DB_reference.Child("USER").Child(currentUser.UserId).Child("ITEM").SetRawJsonValueAsync(Item_Json).ContinueWithOnMainThread(task =>
        //{
        //    if (task.IsCompleted)
        //    {
        //        Debug.Log(" Child.Item ������ ���� �����Ͽ����ϴ�.");
        //    }
        //    else
        //    {
        //        Debug.LogError("������ ���� ���� : " + task.Exception.ToString());
        //    }

        //});

        #endregion

        #region SMELT_DATA
        string smelt_json = JsonConvert.SerializeObject(Base_Manager.Data.User_Main_Data_Smelt_Array);

        //DB_reference.Child("USER").Child(currentUser.UserId).Child("SMELT").SetRawJsonValueAsync(smelt_json).ContinueWithOnMainThread(task =>
        //{
        //    if (task.IsCompleted)
        //    {
        //        Debug.Log(" Child.smelt ������ ���� �����Ͽ����ϴ�.");
        //    }
        //    else
        //    {
        //        Debug.LogError("������ ���� ���� : " + task.Exception.ToString());
        //    }

        //});

        #endregion

    }
    /// <summary>
    /// ���� �����͸� �ҷ��ɴϴ�.
    /// </summary>
    public void ReadData()
    {
        #region Default_Data
        //DB_reference.Child("USER").Child(currentUser.UserId).Child("DATA").GetValueAsync().ContinueWithOnMainThread(task =>
        //{
        //    if (task.IsCompleted)
        //    {
        //        DataSnapshot snapshot = task.Result;

        //        var Default_Data = JsonUtility.FromJson<Data>(snapshot.GetRawJsonValue());
        //        Data data = new Data();
        //        if (Default_Data != null)
        //        {
        //            data = Default_Data;
        //        }
        //        data.StartDate = DateTime.Now.ToString();

        //        if (string.IsNullOrEmpty(data.EndDate))
        //        {
        //            /*data.EndDate�� null �Ǵ� �� ���ڿ��϶�,
        //            DateTime.Parse�� ȣ��Ǹ� FormatException�� �߻�.
        //            �޼��尡 ���ܸ� ó������ ���� ��� �޼��尡 �ߵ������..*/

        //            data.EndDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //            Debug.Log("EndDate�� ��� �⺻������ ����: " + data.EndDate);
        //        }

        //        DateTime startDate = DateTime.Parse(data.StartDate);
        //        DateTime endDate = DateTime.Parse(data.EndDate);

        //        if (Get_Date_Dungeon_Item(startDate, endDate))
        //        {
        //            data.Daily_Enter_Key[0] = 2;
        //            data.Daily_Enter_Key[1] = 2;
        //        }

        //        Data_Manager.Main_Players_Data = data;
        //        Debug.Log("���۽ð� : " + data.StartDate);
        //        Loading_Scene.instance.LoadingMain();
        //    }

        //    else
        //    {
        //        Debug.LogError("������ �б� ���� : " + task.Exception.ToString());
        //    }
        //});
        #endregion

        #region Character_Data

        //DB_reference.Child("USER").Child(currentUser.UserId).Child("CHARACTER").GetValueAsync().ContinueWithOnMainThread(task =>
        //{
        //    if (task.IsCompleted)
        //    {
        //        DataSnapshot snapshot = task.Result;

        //        var data = JsonConvert.DeserializeObject<Dictionary<string, Holder>>(snapshot.GetRawJsonValue());

        //        Base_Manager.Data.character_Holder = data;
        //        Base_Manager.Data.Init();
        //        Set_Character_Data_Dictionary();


        //    }

        //    else
        //    {
        //        Debug.LogError("������ �б� ���� : " + task.Exception.ToString());
        //    }
        //});

        #endregion

        #region Item_Data

        //DB_reference.Child("USER").Child(currentUser.UserId).Child("ITEM").GetValueAsync().ContinueWithOnMainThread(task =>
        //{
        //    if (task.IsCompleted)
        //    {
        //        DataSnapshot snapshot = task.Result;

        //        var data = JsonConvert.DeserializeObject<Dictionary<string, Holder>>(snapshot.GetRawJsonValue());

        //        Base_Manager.Data.Item_Holder = data;

        //    }

        //    else
        //    {
        //        Debug.LogError("������ �б� ���� : " + task.Exception.ToString());
        //    }
        //});

        #endregion

        #region Smelt_Data

        //DB_reference.Child("USER").Child(currentUser.UserId).Child("SMELT").GetValueAsync().ContinueWithOnMainThread(task =>
        //{
        //    if (task.IsCompleted)
        //    {
        //        DataSnapshot snapshot = task.Result;

        //        var data = JsonConvert.DeserializeObject<List<Smelt_Holder>>(snapshot.GetRawJsonValue());

        //        Base_Manager.Data.User_Main_Data_Smelt_Array = data;
        //        Base_Manager.Data.Init();

        //    }

        //    else
        //    {
        //        Debug.LogError("������ �б� ���� : " + task.Exception.ToString());
        //    }
        //});

        #endregion

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
                data.Buff_Level= int.Parse(gameDataJson[0]["BUFF_LEVEL"].ToString());
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
              
                Data_Manager.Main_Players_Data = data;

                Base_Manager.Data.Init();

                Debug.Log("USER ���̺� �����͸� ���������� �ҷ��� �����͸� ������Ʈ �Ͽ����ϴ�.");

            }
        }
        else
        {
            Debug.LogError("������ ��ȸ�� �����߽��ϴ�. : " + bro);
        }

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
