using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd;
using UnityEngine;

public partial class BackEnd_Manager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        var bro = Backend.Initialize(); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            Debug.Log("뒤끝 서버 초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
        }
        else
        {
            Debug.LogError("뒤끝 서버 초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생 
        }      
    }

    private void Start()
    {     
        Loading_Scene.instance.LoadingMain();
    }

    private void OnDestroy()
    {
        if (Base_Manager.Get_MainGame_Start)
        {
            Debug.Log("게임을 정상적으로 종료하고, 데이터를 저장합니다.");           
            WriteData();
        }
    }

}
