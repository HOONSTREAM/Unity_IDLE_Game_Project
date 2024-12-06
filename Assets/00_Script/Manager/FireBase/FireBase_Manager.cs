using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

public partial class FireBase_Manager
{
    private FirebaseAuth auth;
    private FirebaseUser currentUser;
    private DatabaseReference DB_reference;
    public void Init()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if(task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                currentUser = auth.CurrentUser;
                DB_reference = FirebaseDatabase.DefaultInstance.RootReference;

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
