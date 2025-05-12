using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_Inventory_Parts : MonoBehaviour
{
    [SerializeField]
    private Image Rarity_Image, IconImage;
    [SerializeField]
    private TextMeshProUGUI Count_Text;


    public void Init(string name, Holder holder)
    {        
        Item_Scriptable scriptable = Base_Manager.Data.Data_Item_Dictionary[name];
        Rarity_Image.sprite = Utils.Get_Atlas(scriptable.rarity.ToString());
        IconImage.sprite = Utils.Get_Atlas(scriptable.name);
        Count_Text.text = holder.Hero_Card_Amount.ToString();


        GetComponent<ToolTip_Controller>().Init(scriptable);
    }

    /// <summary>
    /// 인벤토리 파츠 소환에 애니메이션을 더합니다.
    /// </summary>
    /// <param name="delay"></param>
    public void PlayAppearAnimation(float delay = 0f)
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = gameObject.AddComponent<CanvasGroup>();
        }

        cg.alpha = 0;
        transform.localScale = Vector3.one * 0.8f;

        StartCoroutine(AppearRoutine(cg, delay));
    }
    private IEnumerator AppearRoutine(CanvasGroup cg, float delay)
    {
        yield return new WaitForSeconds(delay);

        float time = 0f;
        float duration = 0.3f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            cg.alpha = Mathf.Lerp(0, 1, t);
            transform.localScale = Vector3.Lerp(Vector3.one * 0.8f, Vector3.one, t);

            yield return null;
        }

        cg.alpha = 1f;
        transform.localScale = Vector3.one;
    }

}
