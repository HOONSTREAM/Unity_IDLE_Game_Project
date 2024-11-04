using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Utils 
{
    public static SpriteAtlas atlas = Resources.Load<SpriteAtlas>("Atlas");
    public static Stack<UI_Base> UI_Holder = new Stack<UI_Base>();
    public static Level_Design Data = Resources.Load<Level_Design>("Scriptable/Level_Design");

    public static void CloseAllPopupUI()
    {
        while(UI_Holder.Count > 0)
        {
            ClosePopupUI(); 
        }
    }
    public static void ClosePopupUI()
    {
        if(UI_Holder.Count == 0) { return; }

        UI_Base popup = UI_Holder.Peek(); // 스택에 마지막으로 들어온 값을 반환
        popup.DisableOBJ();
    }
    public static Sprite Get_Atlas(string temp)
    {
        return atlas.GetSprite(temp);
    }
    public static string String_Color_Rarity(Rarity rarity)
    {

        switch (rarity)
        {
            case Rarity.Common:
                return "<color=#FFFFFF>";
                
            case Rarity.UnCommon:
                return "<color=#00FF00>";

            case Rarity.Rare:
                return "<color=#00D8FF>";

            case Rarity.Epic:
                return "<color=#B750C3>";


            case Rarity.Legendary:
                return "<color=#FF9000>";

        }



        return "<color=#FFFFFF>";
    }


    /// <summary>
    /// 레벨디자인에 이용할 지수증가공식입니다.
    /// </summary>
    /// <param name="baseValue"></param>
    /// <param name="level"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double CalculateValue(float baseValue, int level, float value)
    {
        return baseValue * Mathf.Pow((level+1), value);
    }
    
    /// <summary>
    /// 레벨업에 필요한 골드를 충족했는지 검사합니다.
    /// </summary>
    /// <returns></returns>
    public static bool Check_Levelup_Gold(double value)
    {
        if (Base_Manager.Data.Player_Money >= value) return true;

        else return false;

    }
}
