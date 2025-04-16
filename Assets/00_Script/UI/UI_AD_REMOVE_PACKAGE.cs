using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AD_REMOVE_PACKAGE : UI_Base
{
    private void Start()
    {
        if (Data_Manager.Main_Players_Data.isBuyADPackage)
        {
            return;
        }
    }
}
