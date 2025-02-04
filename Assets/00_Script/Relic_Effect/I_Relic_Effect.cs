using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static I_Relic_Effect;

public class I_Relic_Effect : MonoBehaviour
{
    public interface IRelicEffect
    {
        public string Get_Effect_Name();
        Holding_Effect_Type Get_Effect_Type();
        double ApplyEffect(Item_Scriptable data);
    }
}
