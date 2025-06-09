using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Status_ToolTip_Controller : MonoBehaviour, IPointerDownHandler
{
    
    public void Init()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {       
        Base_Canvas.instance.Get_Status_Item_Tooltip().Show_Status_Item_ToolTip(eventData.position);
    }
    
}
 
