using TMPro;
using UnityEngine;

public class MainGame_Error_UI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI top_popup_text;

    public void Initialize(string temp)
    {
        top_popup_text.text = temp;
        Destroy(this.gameObject, 2.0f);
    }
}
