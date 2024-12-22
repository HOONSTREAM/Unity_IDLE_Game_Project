using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_TOP_POPUP : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI top_popup_text;

    public void Initialize(string temp)
    {
        top_popup_text.text = temp;
        Destroy(this.gameObject, 2.0f);
    }

}
