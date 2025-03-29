using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostData 
{
    public string Title; // 우편 제목
    public string content; // 우편 내용
    public string inDate; // 우편의 Indate;
    public string expiration_Date; // 우편의 만료일

    public bool isCanReceive = false; // 우편에 있는 아이템을 수령할 수 있는지에 대한 여부

    // <아이템 이름, 아이템 갯수>

    public Dictionary<string, int> post_reward = new Dictionary<string, int>();

    public override string ToString()
    {
        string result = string.Empty;
        result += $"tltle : {Title}\n";
        result += $"content : {content}\n";
        result += $"inDate : {inDate}\n";

        if (isCanReceive)
        {
            result += $"우편 아이템 : \n";

            foreach(string itemKey in post_reward.Keys)
            {
                result += $"itemKey : {itemKey} : {post_reward[itemKey]}개\n";
            }
        }

        else
        {
            result += "지원하지 않는 아이템 입니다.";
        }

        return result;
    }
}
