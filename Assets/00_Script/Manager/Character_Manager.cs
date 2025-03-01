using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character_Manager 
{
    /// <summary>
    /// 현재 배치 되어있는 영웅을 저장합니다.
    /// </summary>
    public Character_Holder[] Set_Character = new Character_Holder[6];


    public void Get_Character(int value, string character_name)
    {
        Character_Holder newChar = Base_Manager.Data.Data_Character_Dictionary[character_name];

        for(int i = 0; i<Set_Character.Length; i++)
        {
            if (Set_Character[i]!=null && Set_Character[i].Data.Character_EN_Name == newChar.Data.Character_EN_Name)
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("영웅을 선택하세요.");
                return;
            }
        }

        Set_Character[value] = newChar;
        Base_Canvas.instance.Get_Toast_Popup().Initialize("영웅 배치가 완료되었습니다. 다음 스테이지부터 출전합니다!");
       
    }

    public void Disable_Character(int value)
    {
        Set_Character[value] = null;       
    }

}
