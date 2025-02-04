using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static I_Hero_Effect;

public class I_Hero_Effect : MonoBehaviour
{
    public interface IHeroEffect
    {
        public string Get_Effect_Name();
        Holding_Effect_Type Get_Effect_Type();
        double ApplyEffect(Character_Scriptable data);
    }
}
