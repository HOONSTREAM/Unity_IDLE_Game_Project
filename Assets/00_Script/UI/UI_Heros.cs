using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Heros : UI_Base
{
    private void Start()
    {
        var Data = Resources.LoadAll<Character_Scriptable>("Scriptable");

        for(int i = 0; i < Data.Length; i++)
        {
            
        }
    }


}
