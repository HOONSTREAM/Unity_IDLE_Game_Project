using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostData 
{
    public string Title; // ���� ����
    public string content; // ���� ����
    public string inDate; // ������ Indate;
    public string expiration_Date; // ������ ������

    public bool isCanReceive = false; // ���� �ִ� �������� ������ �� �ִ����� ���� ����

    // <������ �̸�, ������ ����>

    public Dictionary<string, int> post_reward = new Dictionary<string, int>();

    public override string ToString()
    {
        string result = string.Empty;
        result += $"tltle : {Title}\n";
        result += $"content : {content}\n";
        result += $"inDate : {inDate}\n";

        if (isCanReceive)
        {
            result += $"���� ������ : \n";

            foreach(string itemKey in post_reward.Keys)
            {
                result += $"itemKey : {itemKey} : {post_reward[itemKey]}��\n";
            }
        }

        else
        {
            result += "�������� �ʴ� ������ �Դϴ�.";
        }

        return result;
    }
}
