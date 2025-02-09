using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logo_FadeOut : MonoBehaviour
{
    public Image targetChildImage; // 알파값을 조정할 자식 Image

    public float fadeDuration = 1f; // 페이드 아웃 지속 시간

    public void StartFadeOut()
    {
        if (targetChildImage != null)
        {
            StartCoroutine(FadeOutCoroutine());
        }
        else
        {
            Debug.LogError("targetChildImage가 설정되지 않았습니다!");
        }
    }

    private IEnumerator FadeOutCoroutine()
    {
        yield return new WaitForSeconds(1.0f);

        float elapsedTime = 0f;
        Color initialColor = targetChildImage.color; // 원래 색상 저장

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            targetChildImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 마지막으로 알파값 0으로 설정
        targetChildImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        yield return new WaitForSeconds(1.0f); // 1초 대기 후 부모 오브젝트 비활성화
        gameObject.SetActive(false);
    }

}
