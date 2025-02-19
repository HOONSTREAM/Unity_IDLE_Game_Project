using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// �� ������Ʈ�� �����ϰ��ִ� ������Ʈ�� �ڵ����� ���ö������̼��� ����˴ϴ�.
/// </summary>
public class Auto_Localization : MonoBehaviour
{
    public string Local_Name;
    TextMeshProUGUI T;
    public string[] Semi_Data;

    private void Awake()
    {
        T = GetComponent<TextMeshProUGUI>();
        Set_LocalData();
    }

    public void Set_LocalData()
    {
        if (Local_Name != "")
        {
            string temp = "";
            if (Semi_Data.Length > 0)
                temp = string.Format(Localization_Manager.local_Data["UI/" + Local_Name].Get_Data(), Semi_Data);
            else temp = Localization_Manager.local_Data["UI/" + Local_Name].Get_Data();
            T.text = temp;
        }
    }
}
