using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speech_Character : MonoBehaviour
{
    [SerializeField] private Bubble_Speech_Script buble_chat;
    [SerializeField] private string speech_Types;
    [SerializeField] private Camera cam; 

    public void Init()
    {
        StopAllCoroutines();
        StartCoroutine(Speech_Coroutine());
    }

    public void DisableCoroutine()
    {
        StopAllCoroutines();
    }

    IEnumerator Speech_Coroutine()
    {

        var go = Instantiate(buble_chat, Base_Canvas.instance.transform);
        go.transform.SetSiblingIndex(0);
        go.Init(transform, speech_Types, cam);

        yield return new WaitForSeconds(2.0f);
      
    }
}
