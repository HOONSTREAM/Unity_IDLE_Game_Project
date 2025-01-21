using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Manager 
{
    /// <summary>
    /// ���� ��ġ �Ǿ��ִ� ������ �����մϴ�.
    /// </summary>
    public Character_Holder[] Set_Character = new Character_Holder[6];

    public void Get_Character(int value, string character_name)
    {
        Set_Character[value] = Base_Manager.Data.Data_Character_Dictionary[character_name];
    }

    public void Disable_Character(int value)
    {
        Set_Character[value] = null;       
    }

}
