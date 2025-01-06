using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FireBase_Manager
{
    public void GuestLogin()
    {

        if(auth.CurrentUser != null)
        {
            
            Debug.Log("��⿡ �α��� �� �����Դϴ�. ���� ID : " + auth.CurrentUser.UserId);
            ReadData();
            return;
        }

        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {

            if(task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("�Խ�Ʈ �α��ο� �����Ͽ����ϴ�.");
                return;
            }


            FirebaseUser user = task.Result.User;
            Debug.Log("�Խ�Ʈ �α��ο� �����Ͽ����ϴ�. ����� ID : " + user.UserId);
            ReadData();


        });
    }
    
}
