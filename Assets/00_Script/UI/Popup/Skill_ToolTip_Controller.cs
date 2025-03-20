using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Skill_ToolTip_Controller : MonoBehaviour, IPointerDownHandler
{
    
    public void Init()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Base_Canvas.instance.Get_Skill_Tooltip().Show_Skill_ToolTip(eventData.position);
    }

}
 
