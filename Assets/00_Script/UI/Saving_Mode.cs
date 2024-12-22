using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public delegate void OnSavingMode();
public class Saving_Mode : UI_Base
{
    [SerializeField]
    private TextMeshProUGUI Battery_Text;
    [SerializeField]
    private TextMeshProUGUI Time_Text;
    [SerializeField]
    private Image Battery_Fill_Image;
    [SerializeField]
    private Transform Content;
    [SerializeField]
    private UI_Inventory_Parts item_parts;
    [SerializeField]
    private Image Off_Saving_Image;

    public Dictionary<string, Item_Holder> saving_item_Dict = new Dictionary<string, Item_Holder>();
    public Dictionary<string, UI_Inventory_Parts> item_parts_saving_mode = new Dictionary<string, UI_Inventory_Parts>();


    public static OnSavingMode onSaving;
    Vector2 Off_Saving_Mode_Start_Pos, Off_Saving_Mode_End_Pos;
    private Camera _camera;

    public override bool Init()
    {
        _camera = Camera.main;
        _camera.enabled = false;
        onSaving?.Invoke();
        return base.Init();
    }

    public override void DisableOBJ()
    {
        Base_Canvas.isSavingMode = false;
        _camera.enabled = true;
        base.DisableOBJ();
    }
    private void Update()
    {
        Battery_Text.text = (SystemInfo.batteryLevel * 100.0f).ToString() + "%";
        Battery_Fill_Image.fillAmount = SystemInfo.batteryLevel;

        Time_Text.text = System.DateTime.Now.ToString("HH:mm:ss"); //핸드폰시간기준, 오프라인보상에 사용하면 버그로 악용될 수 있다.



        if (Input.GetMouseButtonDown(0))
        {
            Off_Saving_Mode_Start_Pos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Off_Saving_Mode_End_Pos = Input.mousePosition;
            float distance = Vector2.Distance(Off_Saving_Mode_End_Pos, Off_Saving_Mode_Start_Pos);

            float alpha = Mathf.Clamp01(1.0f - (distance / (Screen.width / 2))); // 1에서 0으로 감소
            Color currentColor = Off_Saving_Image.color;
            Off_Saving_Image.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);

            if (distance >= Screen.width / 2)
            {

                DisableOBJ();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Off_Saving_Mode_Start_Pos = Vector2.zero;
            Off_Saving_Mode_End_Pos = Vector2.zero;
            Color currentColor = Off_Saving_Image.color;
            Off_Saving_Image.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1.0f); // 알파값 복원
        }
    

    }

    IEnumerator OFF_Saving_Mode_Coroutine()
    {
        yield return new WaitForSeconds(2.0f);
        
    }

    public void Get_Item_Saving_Mode(Item_Scriptable item)
    {
        if (saving_item_Dict.ContainsKey(item.name))
        {
            saving_item_Dict[item.name].holder.Hero_Card_Amount++;
            item_parts_saving_mode[item.name].Init(item.name);
            return;
        }

        Item_Holder items = new Item_Holder {Data = item, holder = new Holder()};
        items.holder.Hero_Card_Amount = 1;
        saving_item_Dict.Add(item.name, items);


        var go = Instantiate(item_parts, Content);
        item_parts_saving_mode.Add(item.name, go);
        go.Init(items.Data.name);
    }
}
