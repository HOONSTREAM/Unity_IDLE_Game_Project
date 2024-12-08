using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class User
{
    public string userName;
    public int Stage;
}
public partial class FireBase_Manager 
{
    public void WriteData()
    {
        Data data = new Data();
        if(Data_Manager.Main_Players_Data != null)
        {
            data = Data_Manager.Main_Players_Data;
        }

        string DefalutJson = JsonUtility.ToJson(data);
        string Character_Json = JsonConvert.SerializeObject(Base_Manager.Data.character_Holder);


         
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
    }

    public void ReadData()
    {
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
                Data_Manager.Main_Players_Data = data;
                Base_Manager.Data.Init();
                Loading_Scene.instance.LoadingMain();
            }

            else
            {
                Debug.LogError("데이터 읽기 실패 : " + task.Exception.ToString());
            }
        });

    } 

    
}
