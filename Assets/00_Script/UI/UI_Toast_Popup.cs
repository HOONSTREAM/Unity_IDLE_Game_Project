using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Toast_Popup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Toast_Popup_Text;

    public void Initialize(string temp)
    {
        Toast_Popup_Text.text = temp;
        Destroy(this.gameObject, 5.0f);
    }


}
