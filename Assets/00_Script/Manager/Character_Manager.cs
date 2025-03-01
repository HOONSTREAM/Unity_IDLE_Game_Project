using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character_Manager 
{
    /// <summary>
    /// ���� ��ġ �Ǿ��ִ� ������ �����մϴ�.
    /// </summary>
    public Character_Holder[] Set_Character = new Character_Holder[6];


    public void Get_Character(int value, string character_name)
    {
        Character_Holder newChar = Base_Manager.Data.Data_Character_Dictionary[character_name];

        for(int i = 0; i<Set_Character.Length; i++)
        {
            if (Set_Character[i]!=null && Set_Character[i].Data.Character_EN_Name == newChar.Data.Character_EN_Name)
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("������ �����ϼ���.");
                return;
            }
        }

        Set_Character[value] = newChar;
        Base_Canvas.instance.Get_Toast_Popup().Initialize("���� ��ġ�� �Ϸ�Ǿ����ϴ�. ���� ������������ �����մϴ�!");
       
    }

    public void Disable_Character(int value)
    {
        Set_Character[value] = null;       
    }

}
