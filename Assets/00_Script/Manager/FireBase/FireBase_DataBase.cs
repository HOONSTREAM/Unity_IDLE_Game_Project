using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

public partial class FireBase_Manager 
{
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

    }

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
                

                Debug.Log("로드된 데이터: " + JsonConvert.SerializeObject(Base_Manager.Data.character_Holder));
 
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

                Debug.Log("로드된 데이터: " + JsonConvert.SerializeObject(Base_Manager.Data.Item_Holder));


                Base_Manager.Data.Init(); // TODO


            }

            else
            {
                Debug.LogError("데이터 읽기 실패 : " + task.Exception.ToString());
            }
        });

        #endregion

    }

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

}
