using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Relic_ToolTip_Controller : MonoBehaviour, IPointerDownHandler
{
    private Item_Scriptable item;

    public void Init(Item_Scriptable itemdata)
    {
        item = itemdata;
    }

    public void OnPointerDown(PointerEventData eventData)
    {        
        Base_Canvas.instance.Get_Relic_Tooltip().Show_Relic_ToolTip(item, eventData.position);
    }

}
 
