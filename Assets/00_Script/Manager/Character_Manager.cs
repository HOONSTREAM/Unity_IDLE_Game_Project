using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Manager 
{
    /// <summary>
    /// 현재 배치 되어있는 영웅을 저장합니다.
    /// </summary>
    public Character_Holder[] Set_Character = new Character_Holder[6];

    public void Get_Character(int value, string character_name)
    {
        Set_Character[value] = Base_Manager.Data.Data_Character_Dictionary[character_name];
    }
}
