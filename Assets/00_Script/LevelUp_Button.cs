using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class LevelUp_Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Image Main_Exp_Silder;
    [SerializeField]
    private TextMeshProUGUI Exp_Text, ATK_Text, HP_Text, Gold_Text, Get_Exp_Text;

    private bool isPush = false;
    private float Touch_Timer = default;
    private Coroutine coroutine;

    private void Update()
    {
        if (isPush)
        {
            Touch_Timer += Time.deltaTime;

            if(Touch_Timer >= 0.01f)
            {
                Touch_Timer = 0.0f;
                Exp_Up();            
            }
        }
    }
    public void Exp_Up()
    {
        transform.DORewind();
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.25f);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Exp_Up();
        coroutine = StartCoroutine(Push_Coroutine());

    }

    

    public void OnPointerUp(PointerEventData eventData)
    {     
        isPush = false;
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        Touch_Timer = 0.0f;
    }

    IEnumerator Push_Coroutine()
    {
        yield return new WaitForSeconds(1.0f);
        isPush = true;
    }
}
