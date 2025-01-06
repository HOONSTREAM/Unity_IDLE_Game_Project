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
                Debug.Log("Firebase �ʱ�ȭ�� �����Ͽ����ϴ�.");
            }
            else
            {
                Debug.Log("Firebase �ʱ�ȭ�� �����Ͽ����ϴ�.");
            }
        });


    }
  
}
