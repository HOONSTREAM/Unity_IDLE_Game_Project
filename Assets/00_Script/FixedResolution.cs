using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����ػ� ó��
/// </summary>
public class FixedResolution : MonoBehaviour
{
    public int targetWidth = 1440; // �����ػ��� �ʺ�
    public int targetHeight = 2560; // �����ػ��� ����

    private void Start()
    {
        ApplyFixedResolution();
    }

    private void ApplyFixedResolution()
    {
        float targetAspect = (float)targetWidth / (float)targetHeight; // ��ǥ ȭ�� ����
        float windowAspect = (float)Screen.width / Screen.height; // ���� ȭ�� ����

        float scaleHeight = windowAspect / targetAspect;

        Camera mainCam = Camera.main;

        if(scaleHeight < 1.0f)
        {
            Rect rect = mainCam.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            mainCam.rect = rect;
        }

        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = mainCam.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            mainCam.rect = rect;
        }

        Screen.SetResolution(targetWidth, targetHeight, true);
    }

}
