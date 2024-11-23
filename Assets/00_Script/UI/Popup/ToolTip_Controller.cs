using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTip_Controller : MonoBehaviour, IPointerDownHandler
{
    private Item_Scriptable item;

    public void Init(Item_Scriptable itemData)
    {
        item = itemData;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Base_Canvas.instance.Get_Item_Tooltip().Show_Item_ToolTip(item, eventData.position);
    }

}
 
