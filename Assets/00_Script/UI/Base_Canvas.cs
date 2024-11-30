using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Base_Canvas : MonoBehaviour
{
    public static Base_Canvas instance = null;
    public Transform Coin;
    [SerializeField]
    private Transform LAYER;
    [SerializeField]
    private Button Hero_Button;
    [SerializeField]
    private Button Inventory_Button;
    [SerializeField]
    private Button Saving_Mode_Button;

    public Item_ToolTip item_tooltip = null;
    public UI_Base UI;
    public static bool isSavingMode = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        Hero_Button.onClick.AddListener(() => Get_UI("@Heros", true));
        Inventory_Button.onClick.AddListener(() => Get_UI("UI_INVENTORY"));
        Saving_Mode_Button.onClick.AddListener(() => {
            Get_UI("Saving_Mode");
            isSavingMode = true;
        });
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Utils.UI_Holder.Count > 0)
            {
                Utils.ClosePopupUI();
            }

            else
            {
                Debug.Log("게임종료 팝업");
            }
            
        }
    }

    public Transform Holder_Layer(int value)
    {
        return LAYER.GetChild(value);
    }

    public void All_Layer_Destroy()
    {
        for (int i = 0; i < GameObject.Find("Layer1").gameObject.transform.childCount; i++)
        {
            Destroy(GameObject.Find("Layer1").gameObject.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < GameObject.Find("Layer2").gameObject.transform.childCount; i++)
        {
            Destroy(GameObject.Find("Layer2").gameObject.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < GameObject.Find("Layer3").gameObject.transform.childCount; i++)
        {
            Destroy(GameObject.Find("Layer3").gameObject.transform.GetChild(i).gameObject);
        }

        return;
    }

    public void Get_UI(string temp, bool Fade = false)
    {
        if (Fade)
        {
            Main_UI.Instance.FadeInOut(false, true, () => GetPopupUI(temp));

            return;
        }

        GetPopupUI(temp);

    }

    private void GetPopupUI(string temp)
    {
        if(UI != null)
        {
            UI = null;
        }

        var go = Instantiate(Resources.Load<UI_Base>("UI/" + temp), transform);
        UI = go;
        Utils.UI_Holder.Push(go);

    }

    public Item_ToolTip Get_Item_Tooltip()
    {
        if(item_tooltip != null)
        {
            Destroy(item_tooltip.gameObject);
        }

        item_tooltip = Instantiate(Resources.Load<Item_ToolTip>("UI/Item_ToolTip"), transform);
       
        return item_tooltip;
    }
}
