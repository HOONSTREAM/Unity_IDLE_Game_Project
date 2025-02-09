using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logo_FadeOut : MonoBehaviour
{
    public Image targetChildImage; // ���İ��� ������ �ڽ� Image

    public float fadeDuration = 1f; // ���̵� �ƿ� ���� �ð�

    public void StartFadeOut()
    {
        if (targetChildImage != null)
        {
            StartCoroutine(FadeOutCoroutine());
        }
        else
        {
            Debug.LogError("targetChildImage�� �������� �ʾҽ��ϴ�!");
        }
    }

    private IEnumerator FadeOutCoroutine()
    {
        yield return new WaitForSeconds(1.0f);

        float elapsedTime = 0f;
        Color initialColor = targetChildImage.color; // ���� ���� ����

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            targetChildImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���������� ���İ� 0���� ����
        targetChildImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        yield return new WaitForSeconds(1.0f); // 1�� ��� �� �θ� ������Ʈ ��Ȱ��ȭ
        gameObject.SetActive(false);
    }

}
