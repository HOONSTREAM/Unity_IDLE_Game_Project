using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        var gameObject = Instantiate(Resources.Load<UI_Base>("UI/" + temp), transform);

        Utils.UI_Holder.Push(gameObject);

    }
}
