using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using UnityEngine;

public partial class FireBase_Manager
{
    private FirebaseAuth auth;
    private FirebaseUser currentUser;
    public void Init()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if(task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                currentUser = auth.CurrentUser;
                GuestLogin();
                Debug.Log("Firebase 초기화에 성공하였습니다.");
            }
            else
            {
                Debug.Log("Firebase 초기화에 실패하였습니다.");
            }

        });

    }
  
}
