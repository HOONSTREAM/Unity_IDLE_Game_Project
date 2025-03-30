using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BackEnd;
using System;
using UnityEngine.UI;

public class UI_Post_Parts : MonoBehaviour
{
    [SerializeField]
    private Image ImageItemIcon; // 우편에 포함 된 아이템 아이콘에 출력할 이미지 
    [SerializeField]
    private TextMeshProUGUI Item_Count; // 우편에 포함 된 아이템 갯수
    [SerializeField]
    private TextMeshProUGUI Text_Title; // 우편 제목
    [SerializeField]
    private TextMeshProUGUI Text_Content; // 우편 내용
    [SerializeField]
    private TextMeshProUGUI Text_Expiration_Date; // 만기 날짜
    [SerializeField]
    private Button button_Receive; // 수령 버튼

    private BackEnd_PostSystem backend_postsystem;
    private PostData postData;
    private UI_PostBox PostBoxUI;


    public void SetUp(BackEnd_PostSystem postsystem, UI_PostBox postbox, PostData postData)
    {
        button_Receive.onClick.AddListener(OnClickPostReceive);

        backend_postsystem = postsystem;
        PostBoxUI = postbox;
        this.postData = postData;

        // 우편 제목과 내용 설정
        Text_Title.text = postData.Title;
        Text_Content.text = postData.content;

        // 첫 번째 아이템 정보를 우편에 출력
        foreach( string ItemKey in postData.post_reward.Keys)
        {
            if (ItemKey.Equals("Dia"))
            {
                ImageItemIcon.sprite = Utils.Get_Atlas("Dia");
            }
            //TODO :  하단에 아이템 종류가 추가되면 else if로 아이템 추가

            Item_Count.text = postData.post_reward[ItemKey].ToString();

            break;
        }

        // 서버시간 받아오기

        Backend.Utils.GetServerTime(callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"서버 시간 불러오기에 실패 했습니다 :{callback}");
                return;
            }

            // JSON 데이터 파싱 성공
            try
            {
                // 현재 서버 시간
                string server_Time = callback.GetFlattenJSON()["utcTime"].ToString();

                // 우편 만료까지 남은 시간 = 우편 만료시간 - 현재 서버시간
                TimeSpan timespan = DateTime.Parse(postData.expiration_Date) - DateTime.Parse(server_Time);

                Text_Expiration_Date.text = $"{timespan.TotalHours:F0}시간 후 만료";
            }

            catch(System.Exception e)
            {
                Debug.LogError(e);
            }
          
        });

    }

    private void OnClickPostReceive()
    {
        // 현재 우편 UI 오브젝트 삭제
        PostBoxUI.DestroyPost(gameObject);
        backend_postsystem.PostReceive(PostType.Admin, postData.inDate);
    }
    
}
