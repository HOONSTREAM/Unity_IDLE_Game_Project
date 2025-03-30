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
    private Image ImageItemIcon; // ���� ���� �� ������ �����ܿ� ����� �̹��� 
    [SerializeField]
    private TextMeshProUGUI Item_Count; // ���� ���� �� ������ ����
    [SerializeField]
    private TextMeshProUGUI Text_Title; // ���� ����
    [SerializeField]
    private TextMeshProUGUI Text_Content; // ���� ����
    [SerializeField]
    private TextMeshProUGUI Text_Expiration_Date; // ���� ��¥
    [SerializeField]
    private Button button_Receive; // ���� ��ư

    private BackEnd_PostSystem backend_postsystem;
    private PostData postData;
    private UI_PostBox PostBoxUI;


    public void SetUp(BackEnd_PostSystem postsystem, UI_PostBox postbox, PostData postData)
    {
        button_Receive.onClick.AddListener(OnClickPostReceive);

        backend_postsystem = postsystem;
        PostBoxUI = postbox;
        this.postData = postData;

        // ���� ����� ���� ����
        Text_Title.text = postData.Title;
        Text_Content.text = postData.content;

        // ù ��° ������ ������ ���� ���
        foreach( string ItemKey in postData.post_reward.Keys)
        {
            if (ItemKey.Equals("Dia"))
            {
                ImageItemIcon.sprite = Utils.Get_Atlas("Dia");
            }
            //TODO :  �ϴܿ� ������ ������ �߰��Ǹ� else if�� ������ �߰�

            Item_Count.text = postData.post_reward[ItemKey].ToString();

            break;
        }

        // �����ð� �޾ƿ���

        Backend.Utils.GetServerTime(callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"���� �ð� �ҷ����⿡ ���� �߽��ϴ� :{callback}");
                return;
            }

            // JSON ������ �Ľ� ����
            try
            {
                // ���� ���� �ð�
                string server_Time = callback.GetFlattenJSON()["utcTime"].ToString();

                // ���� ������� ���� �ð� = ���� ����ð� - ���� �����ð�
                TimeSpan timespan = DateTime.Parse(postData.expiration_Date) - DateTime.Parse(server_Time);

                Text_Expiration_Date.text = $"{timespan.TotalHours:F0}�ð� �� ����";
            }

            catch(System.Exception e)
            {
                Debug.LogError(e);
            }
          
        });

    }

    private void OnClickPostReceive()
    {
        // ���� ���� UI ������Ʈ ����
        PostBoxUI.DestroyPost(gameObject);
        backend_postsystem.PostReceive(PostType.Admin, postData.inDate);
    }
    
}
