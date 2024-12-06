using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class User
{
    public string userName;
    public int Stage;
}
public partial class FireBase_Manager 
{
    public void WriteData()
    {

        User user = new User();
        user.userName = currentUser.UserId;
        user.Stage = Base_Manager.Data.Player_Stage;

        string json = JsonUtility.ToJson(user);

        DB_reference.Child("USER").Child(user.userName).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("데이터 쓰기 성공하였습니다.");
            }
            else
            {
                Debug.LogError("데이터 쓰기 실패 : " + task.Exception.ToString());
            }

        });
    }

    public void ReadData()
    {
        DB_reference.Child("USER").Child(currentUser.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                User user = JsonUtility.FromJson<User>(snapshot.GetRawJsonValue());
                Debug.Log("사용자 이름 :" + user.userName + ", 현재 스테이지 : " + user.Stage);
            }

            else
            {
                Debug.LogError("데이터 읽기 실패 : " + task.Exception.ToString());
            }
        });

    } 

    
}
