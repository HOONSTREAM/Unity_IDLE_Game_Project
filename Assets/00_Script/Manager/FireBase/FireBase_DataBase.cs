using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

public partial class FireBase_Manager 
{
    /// <summary>
    /// ���� �����͸� �����մϴ�.
    /// </summary>
    public void WriteData()
    {
        #region DEFAULT DATA

        Data data = new Data();

        if (Data_Manager.Main_Players_Data != null)
        {
            data = Data_Manager.Main_Players_Data;
            DateTime LastDate = DateTime.Parse(data.EndDate);
            data.EndDate = DateTime.Now.ToString();

            if (Get_Date_Dungeon_Item(LastDate, DateTime.Now))
            {
                data.Daily_Enter_Key[0] = 2;
                data.Daily_Enter_Key[1] = 2;
            }
            Debug.Log("����ð� : " + data.EndDate);
        }

        string DefalutJson = JsonUtility.ToJson(data);

        DB_reference.Child("USER").Child(currentUser.UserId).Child("DATA").SetRawJsonValueAsync(DefalutJson).ContinueWithOnMainThread(task => 
        {
            if (task.IsCompleted)
            {
                Debug.Log("Child.DATA ������ ���� �����Ͽ����ϴ�.");
            }
            else
            {
                Debug.LogError("������ ���� ���� : " + task.Exception.ToString());
            }

        });
        #endregion

        #region CHARACTER DATA

        string Character_Json = JsonConvert.SerializeObject(Base_Manager.Data.character_Holder);

        DB_reference.Child("USER").Child(currentUser.UserId).Child("CHARACTER").SetRawJsonValueAsync(Character_Json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log(" Child.character ������ ���� �����Ͽ����ϴ�.");              
            }
            else
            {
                Debug.LogError("������ ���� ���� : " + task.Exception.ToString());
            }

        });
        #endregion

        #region ITEM_DATA
        string Item_Json = JsonConvert.SerializeObject(Base_Manager.Data.Item_Holder);

        DB_reference.Child("USER").Child(currentUser.UserId).Child("ITEM").SetRawJsonValueAsync(Item_Json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log(" Child.Item ������ ���� �����Ͽ����ϴ�.");
            }
            else
            {
                Debug.LogError("������ ���� ���� : " + task.Exception.ToString());
            }

        });

        #endregion

        #region SMELT_DATA
        string smelt_json = JsonConvert.SerializeObject(Base_Manager.Data.User_Main_Data_Smelt_Array);

        DB_reference.Child("USER").Child(currentUser.UserId).Child("SMELT").SetRawJsonValueAsync(smelt_json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log(" Child.smelt ������ ���� �����Ͽ����ϴ�.");
            }
            else
            {
                Debug.LogError("������ ���� ���� : " + task.Exception.ToString());
            }

        });

        #endregion

    }
    /// <summary>
    /// ���� �����͸� �ҷ��ɴϴ�.
    /// </summary>
    public void ReadData()
    {
        #region Default_Data
        DB_reference.Child("USER").Child(currentUser.UserId).Child("DATA").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsCompleted)
            {            
                DataSnapshot snapshot = task.Result;

                var Default_Data = JsonUtility.FromJson<Data>(snapshot.GetRawJsonValue());
                Data data = new Data();
                if(Default_Data != null)
                {
                    data = Default_Data;                     
                }
                data.StartDate = DateTime.Now.ToString();

                if (string.IsNullOrEmpty(data.EndDate))
                {
                    /*data.EndDate�� null �Ǵ� �� ���ڿ��϶�,
                    DateTime.Parse�� ȣ��Ǹ� FormatException�� �߻�.
                    �޼��尡 ���ܸ� ó������ ���� ��� �޼��尡 �ߵ������..*/

                    data.EndDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    Debug.Log("EndDate�� ��� �⺻������ ����: " + data.EndDate);
                }

                DateTime startDate = DateTime.Parse(data.StartDate);
                DateTime endDate = DateTime.Parse(data.EndDate);

                if(Get_Date_Dungeon_Item(startDate,endDate))
                {
                    data.Daily_Enter_Key[0] = 2;
                    data.Daily_Enter_Key[1] = 2;
                }
                
                Data_Manager.Main_Players_Data = data;
                Debug.Log("���۽ð� : " + data.StartDate);
                Loading_Scene.instance.LoadingMain();
            }

            else
            {
                Debug.LogError("������ �б� ���� : " + task.Exception.ToString());
            }
        });
        #endregion

        #region Character_Data

        DB_reference.Child("USER").Child(currentUser.UserId).Child("CHARACTER").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {             
                DataSnapshot snapshot = task.Result;

                var data = JsonConvert.DeserializeObject<Dictionary<string, Holder>>(snapshot.GetRawJsonValue());
               
                Base_Manager.Data.character_Holder = data;
                Base_Manager.Data.Init();
                Set_Character_Data_Dictionary();

                
            }

            else
            {
                Debug.LogError("������ �б� ���� : " + task.Exception.ToString());
            }
        });

        #endregion

        #region Item_Data

        DB_reference.Child("USER").Child(currentUser.UserId).Child("ITEM").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {             
                DataSnapshot snapshot = task.Result;

                var data = JsonConvert.DeserializeObject<Dictionary<string, Holder>>(snapshot.GetRawJsonValue());

                Base_Manager.Data.Item_Holder = data;
                
                Debug.Log("�ε�� �κ��丮 ������: " + JsonConvert.SerializeObject(Base_Manager.Data.Item_Holder));

            }

            else
            {
                Debug.LogError("������ �б� ���� : " + task.Exception.ToString());
            }
        });

        #endregion

        #region Smelt_Data

        DB_reference.Child("USER").Child(currentUser.UserId).Child("SMELT").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                var data = JsonConvert.DeserializeObject<List<Smelt_Holder>>(snapshot.GetRawJsonValue());

                Base_Manager.Data.User_Main_Data_Smelt_Array = data;
                Base_Manager.Data.Init();

                for(int i = 0; i < 5 ; i++)
                {
                    Debug.Log("�ε�� ���� ������: " + JsonConvert.SerializeObject(Base_Manager.Data.User_Main_Data_Smelt_Array[i].smelt_holder.ToString()));
                }

            }

            else
            {
                Debug.LogError("������ �б� ���� : " + task.Exception.ToString());
            }
        });

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
        if(startdate.Day !=  enddate.Day)
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

                Debug.Log($"ĳ���� �߰���: {characterScriptable.M_Character_Name}");
            }

            Debug.Log("�ε�� Character_Holder ������: " + JsonConvert.SerializeObject(Base_Manager.Data.character_Holder));
            Debug.Log("�ε�� ĳ���� ��ųʸ� : " + JsonConvert.SerializeObject(Base_Manager.Data.Data_Character_Dictionary));
            
        }
    }

}
