using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bubble_Speech_Script : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI SpeechText;
    [SerializeField] private Transform m_Transform;
    private Vector3 m_Pos = new Vector3(0.0f, 2.5f, 0.0f);
    private Camera cam;
    string Speechs;

    public void Init(Transform transform, string methods, Camera camera)
    {
        cam = camera;
        Speechs = methods;
        m_Transform = transform;
        StartCoroutine(SpeechTextCoroutine());
    }

    private void LateUpdate()
    {
        if (cam != null)
            transform.position = cam.WorldToScreenPoint(m_Transform.position + m_Pos);
    }

    IEnumerator SpeechTextCoroutine()
    {
        string temp = Speechs;
        SpeechText.text = temp;

        float current = 0;
        float percent = 0;
        float start = 0.0f;
        float end = 1.0f;
        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.3f;
            float LerpScale = Mathf.Lerp(start, end, percent);
            transform.localScale = new Vector3(LerpScale, LerpScale, LerpScale);
            yield return null;
        }

        yield return new WaitForSeconds(2.0f);
        current = 0;
        percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.3f;
            float LerpScale = Mathf.Lerp(end, start, percent);
            transform.localScale = new Vector3(LerpScale, LerpScale, LerpScale);
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
