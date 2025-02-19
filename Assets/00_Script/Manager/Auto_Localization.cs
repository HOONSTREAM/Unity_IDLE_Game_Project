using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 이 컴포넌트를 보유하고있는 오브젝트는 자동으로 로컬라이제이션이 진행됩니다.
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
