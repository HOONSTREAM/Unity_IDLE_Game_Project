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
    [SerializeField]
    private TextMeshProUGUI Now_Player_Stage;

    public TextMeshProUGUI Gold_Text;

    public Dictionary<string, Item_Holder> saving_item_Dict = new Dictionary<string, Item_Holder>(); // ���̺��� �����϶�, ������ȹ�淮�� �ӽ÷� �����մϴ�.
    public Dictionary<string, UI_Inventory_Parts> item_parts_saving_mode = new Dictionary<string, UI_Inventory_Parts>();


    public static OnSavingMode onSaving;
    Vector2 Off_Saving_Mode_Start_Pos, Off_Saving_Mode_End_Pos;
    private Camera _camera;

    public override bool Init()
    {
        _camera = Camera.main;
        _camera.enabled = false;
        onSaving?.Invoke();

        Base_Manager.SOUND._audioSource[0].volume = 0.0f;
        Base_Manager.SOUND._audioSource[1].volume = 0.0f;

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

        Time_Text.text = System.DateTime.Now.ToString("HH:mm:ss"); //�ڵ����ð�����, �������κ��� ����ϸ� ���׷� �ǿ�� �� �ִ�.
        Now_Player_Stage.text = $"<color=#FFFF00>{Data_Manager.Main_Players_Data.Player_Stage}</color>�� ������...";
        Gold_Text.text = $"<color=#FFFF00>{StringMethod.ToCurrencyString(Data_Manager.Main_Players_Data.Player_Money)}</color>��� ȹ����...";

        if (Input.GetMouseButtonDown(0))
        {
            Off_Saving_Mode_Start_Pos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            if (Off_Saving_Mode_Start_Pos == Vector2.zero) return;

            Off_Saving_Mode_End_Pos = Input.mousePosition;
            float distance = Vector2.Distance(Off_Saving_Mode_End_Pos, Off_Saving_Mode_Start_Pos);

            float alpha = Mathf.Clamp01(1.0f - (distance / (Screen.width / 2))); // 1���� 0���� ����
            Color currentColor = Off_Saving_Image.color;
            Off_Saving_Image.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);

            if (distance >= Screen.width / 2)
            {

                DisableOBJ();

                Base_Manager.SOUND._audioSource[0].volume = Base_Manager.SOUND.BGMValue;
                Base_Manager.SOUND._audioSource[1].volume = Base_Manager.SOUND.BGSValue;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Off_Saving_Mode_Start_Pos = Vector2.zero;
            Off_Saving_Mode_End_Pos = Vector2.zero;
            Color currentColor = Off_Saving_Image.color;
            Off_Saving_Image.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1.0f); // ���İ� ����
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
            item_parts_saving_mode[item.name].Init(item.name, saving_item_Dict[item.name].holder);
            return;
        }

        Item_Holder items = new Item_Holder {Data = item, holder = new Holder()};
        items.holder.Hero_Card_Amount = 1;
        saving_item_Dict.Add(item.name, items);


        var go = Instantiate(item_parts, Content);
        item_parts_saving_mode.Add(item.name, go);
        go.Init(items.Data.name, saving_item_Dict[item.name].holder);
    }
}
