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

        // tableName에서 최대 10개의 타인이 등록한 row 불러오기
        bro = Backend.PlayerData.GetOtherData(tableName, otherOwnerIndate);
        // 불러오기에 실패할 경우
        if (bro.IsSuccess() == false)
        {
            Debug.Log("데이터 읽기 중에 문제가 발생했습니다 : " + bro.ToString());
        }
        // 불러오기에는 성공했으나 데이터가 존재하지 않는 경우
        if (bro.IsSuccess() && bro.FlattenRows().Count <= 0)
        {
            Debug.Log("데이터가 존재하지 않습니다");
        }
        // 1개 이상 데이터를 불러온 경우
        if (bro.FlattenRows().Count > 0)
        {
            string inDate = bro.FlattenRows()[0]["inDate"].ToString();
            int level = int.Parse(bro.FlattenRows()[0]["PLAYER_LEVEL"].ToString());

            
            User_Rank.text = level.ToString();
        }

        
    }
}
