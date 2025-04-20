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

    public void Get_IAP_Product(string purchase_name)
    {
        Base_Manager.IAP.Purchase(purchase_name);
    }

}
