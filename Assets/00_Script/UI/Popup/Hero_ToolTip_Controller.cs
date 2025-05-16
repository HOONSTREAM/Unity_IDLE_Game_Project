using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hero_ToolTip_Controller : MonoBehaviour, IPointerDownHandler
{
    private Character_Scriptable hero;

    public void Init(Character_Scriptable herodata)
    {
        hero = herodata;
    }

    public void OnPointerDown(PointerEventData eventData)
    {        
        Base_Canvas.instance.Get_Hero_Tooltip().Show_Hero_ToolTip(hero, eventData.position);
    }

}
 
