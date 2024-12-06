using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FireBase_Manager : MonoBehaviour
{
    public void GuestLogin()
    {

        if(auth.CurrentUser != null)
        {
            Debug.Log("기기에 로그인 된 상태입니다. 유저 ID : " + auth.CurrentUser.UserId);
            return;
        }

        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {

            if(task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("게스트 로그인에 실패하였습니다.");
                return;
            }


            FirebaseUser user = task.Result.User;
            Debug.Log("게스트 로그인에 성공하였습니다. 사용자 ID : " + user.UserId);


        });
    }
    
}
