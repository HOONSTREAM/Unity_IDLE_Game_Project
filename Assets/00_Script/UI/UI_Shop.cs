using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Shop : UI_Base
{
    public override void DisableOBJ()
    {
        Main_UI.Instance.Layer_Check(-1);
        base.DisableOBJ();
    }
}
