using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChatLIst_Parts : MonoBehaviour
{
    [SerializeField]
    private Button User_Info_Button;
    [SerializeField]
    private GameObject User_Info;
    [SerializeField]
    private TextMeshProUGUI User_Rank;
    [SerializeField]
    private TextMeshProUGUI Nick_Name;

    public void Get_User_Info()
    {
        User_Info.gameObject.SetActive(true);

        var userBro = Backend.Social.GetUserInfoByNickName("Vscode");
        string otherOwnerIndate = userBro.GetReturnValuetoJSON()["row"]["inDate"].ToString();
        string tableName = "USER";
        BackendReturnObject bro = null;

        // tableName���� �ִ� 10���� Ÿ���� ����� row �ҷ�����
        bro = Backend.PlayerData.GetOtherData(tableName, otherOwnerIndate);
        // �ҷ����⿡ ������ ���
        if (bro.IsSuccess() == false)
        {
            Debug.Log("������ �б� �߿� ������ �߻��߽��ϴ� : " + bro.ToString());
        }
        // �ҷ����⿡�� ���������� �����Ͱ� �������� �ʴ� ���
        if (bro.IsSuccess() && bro.FlattenRows().Count <= 0)
        {
            Debug.Log("�����Ͱ� �������� �ʽ��ϴ�");
        }
        // 1�� �̻� �����͸� �ҷ��� ���
        if (bro.FlattenRows().Count > 0)
        {
            string inDate = bro.FlattenRows()[0]["inDate"].ToString();
            int level = int.Parse(bro.FlattenRows()[0]["PLAYER_LEVEL"].ToString());

            
            User_Rank.text = level.ToString();
        }

        
    }
}
