using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Canvas : MonoBehaviour
{
    public static Base_Canvas instance = null;
    public Transform Coin;
    [SerializeField]
    private Transform LAYER;
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

    public void Get_UI(string temp)
    {
        var gameObject = Instantiate(Resources.Load<UI_Base>("UI/" + temp), transform);

        Utils.UI_Holder.Push(gameObject);
    }
}
