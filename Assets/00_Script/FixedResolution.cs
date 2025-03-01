using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 고정해상도 처리
/// </summary>
public class FixedResolution : MonoBehaviour
{
    public int targetWidth = 1440; // 고정해상도의 너비
    public int targetHeight = 2560; // 고정해상도의 높이

    private void Start()
    {
        ApplyFixedResolution();
    }

    private void ApplyFixedResolution()
    {
        float targetAspect = (float)targetWidth / (float)targetHeight; // 목표 화면 비율
        float windowAspect = (float)Screen.width / Screen.height; // 현재 화면 비율

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
