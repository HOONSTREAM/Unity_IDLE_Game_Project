using UnityEngine;

public class Auto_Save_Mode_Controller : MonoBehaviour
{
    private float idleTimer = 0f;
    private float idleThreshold = 60f; // 1��

    private bool isSleepMode = false;

    private void Update()
    {
        if (IsUserActive())
        {
            isSleepMode = false;
            idleTimer = 0f;
        }

        else
        {
            idleTimer += Time.unscaledDeltaTime;

            if (idleTimer >= idleThreshold && !isSleepMode)
            {
                EnterSleepMode();
            }
        }
    }

    private bool IsUserActive()
    {
        // PC: ���콺 �̵�, Ŭ�� / �����: ��ġ
        return Input.GetMouseButtonDown(0) || Input.touchCount > 0;
    }

    private void EnterSleepMode()
    {
        isSleepMode = true;
        Debug.Log("���� ��� ����");
        Base_Canvas.instance.Get_UI("Saving_Mode");
        Base_Canvas.isSavingMode = true;
    }

}
