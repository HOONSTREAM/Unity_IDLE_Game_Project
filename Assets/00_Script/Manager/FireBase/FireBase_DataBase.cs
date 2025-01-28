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
    /// 유저 데이터를 저장합니다.
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
            Debug.Log("종료시간 : " + data.EndDate);
        }

        string DefalutJson = JsonUtility.ToJson(data);

        DB_reference.Child("USER").Child(currentUser.UserId).Child("DATA").SetRawJsonValueAsync(DefalutJson).ContinueWithOnMainThread(task => 
        {
            if (task.IsCompleted)
            {
                Debug.Log("Child.DATA 데이터 쓰기 성공하였습니다.");
            }
            else
            {
                Debug.LogError("데이터 쓰기 실패 : " + task.Exception.ToString());
            }

        });
        #endregion

        #region CHARACTER DATA

        string Character_Json = JsonConvert.SerializeObject(Base_Manager.Data.character_Holder);

        DB_reference.Child("USER").Child(currentUser.UserId).Child("CHARACTER").SetRawJsonValueAsync(Character_Json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log(" Child.character 데이터 쓰기 성공하였습니다.");              
            }
            else
            {
                Debug.LogError("데이터 쓰기 실패 : " + task.Exception.ToString());
            }

        });
        #endregion

        #region ITEM_DATA
        string Item_Json = JsonConvert.SerializeObject(Base_Manager.Data.Item_Holder);

        DB_reference.Child("USER").Child(currentUser.UserId).Child("ITEM").SetRawJsonValueAsync(Item_Json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log(" Child.Item 데이터 쓰기 성공하였습니다.");
            }
            else
            {
                Debug.LogError("데이터 쓰기 실패 : " + task.Exception.ToString());
            }

        });

        #endregion

        #region SMELT_DATA
        string smelt_json = JsonConvert.SerializeObject(Base_Manager.Data.User_Main_Data_Smelt_Array);

        DB_reference.Child("USER").Child(currentUser.UserId).Child("SMELT").SetRawJsonValueAsync(smelt_json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log(" Child.smelt 데이터 쓰기 성공하였습니다.");
            }
            else
            {
                Debug.LogError("데이터 쓰기 실패 : " + task.Exception.ToString());
            }

        });

        #endregion

    }
    /// <summary>
    /// 유저 데이터를 불러옵니다.
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
                    /*data.EndDate가 null 또는 빈 문자열일때,
                    DateTime.Parse가 호출되면 FormatException이 발생.
                    메서드가 예외를 처리하지 않을 경우 메서드가 중도종료됨..*/

                    data.EndDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    Debug.Log("EndDate가 없어서 기본값으로 설정: " + data.EndDate);
                }

                DateTime startDate = DateTime.Parse(data.StartDate);
                DateTime endDate = DateTime.Parse(data.EndDate);

                if(Get_Date_Dungeon_Item(startDate,endDate))
                {
                    data.Daily_Enter_Key[0] = 2;
                    data.Daily_Enter_Key[1] = 2;
                }
                
                Data_Manager.Main_Players_Data = data;
                Debug.Log("시작시간 : " + data.StartDate);
                Loading_Scene.instance.LoadingMain();
            }

            else
            {
                Debug.LogError("데이터 읽기 실패 : " + task.Exception.ToString());
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
                Debug.LogError("데이터 읽기 실패 : " + task.Exception.ToString());
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
                
                Debug.Log("로드된 인벤토리 데이터: " + JsonConvert.SerializeObject(Base_Manager.Data.Item_Holder));

            }

            else
            {
                Debug.LogError("데이터 읽기 실패 : " + task.Exception.ToString());
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
                    Debug.Log("로드된 각인 데이터: " + JsonConvert.SerializeObject(Base_Manager.Data.User_Main_Data_Smelt_Array[i].smelt_holder.ToString()));
                }

            }

            else
            {
                Debug.LogError("데이터 읽기 실패 : " + task.Exception.ToString());
            }
        });

        #endregion

    }
    /// <summary>
    /// 날짜가 자정이 지났는지 확인하고, 데일리 입장권을 지급할 지 판단합니다.
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
    /// 로드된 Character_Holder를 이용하여, Data_Character_Dictionary에 매핑합니다.
    /// </summary>
    private void Set_Character_Data_Dictionary()
    {
        // Scriptable 파일과 매칭하여 Data_Character_Dictionary에 저장
        Base_Manager.Data.Data_Character_Dictionary.Clear(); // 기존 데이터를 초기화

        var scriptableCharacters = Resources.LoadAll<Character_Scriptable>("Scriptable/Character");

        foreach (var characterScriptable in scriptableCharacters)
        {
            if (Base_Manager.Data.character_Holder.ContainsKey(characterScriptable.M_Character_Name))
            {
                // 캐릭터 데이터를 Scriptable과 매칭
                var holderData = Base_Manager.Data.character_Holder[characterScriptable.M_Character_Name];

                var characterHolder = new Character_Holder
                {
                    Data = characterScriptable,
                    holder = holderData
                };

                Base_Manager.Data.Data_Character_Dictionary[characterScriptable.M_Character_Name] = characterHolder;

                Debug.Log($"캐릭터 추가됨: {characterScriptable.M_Character_Name}");
            }

            Debug.Log("로드된 Character_Holder 데이터: " + JsonConvert.SerializeObject(Base_Manager.Data.character_Holder));
            Debug.Log("로드된 캐릭터 딕셔너리 : " + JsonConvert.SerializeObject(Base_Manager.Data.Data_Character_Dictionary));
            
        }
    }

}
