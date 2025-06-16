using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Status_ToolTip_Controller : MonoBehaviour, IPointerDownHandler
{
    private Status_Item_Scriptable item;

    public void Init(Status_Item_Scriptable itemData)
    {
        item = itemData;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Data_Manager.Main_Players_Data.Player_Max_Stage < 2000)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize($"스테이지 {Utils.ENHANCEMENT_DUNGEON_FIRST_HARD}층 이상부터 해금됩니다.");
            return;
        }

        Base_Canvas.instance.Get_Status_Item_Tooltip().Show_Status_Item_ToolTip(eventData.position, item);
    }
    
}
 
