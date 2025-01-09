using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Status : UI_Base
{
    public override void DisableOBJ()
    {
        Main_UI.Instance.Layer_Check(-1); // ��ư�� �ٽ� ���� ũ��� �ǵ����ϴ�.

        Main_UI.Instance.FadeInOut(false, true, () =>
        {
            Main_UI.Instance.FadeInOut(true, false, null);
            base.DisableOBJ();
        });

    }
}
