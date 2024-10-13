using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static string String_Color_Rarity(Rarity rarity)
    {

        switch (rarity)
        {
            case Rarity.UnCommon:
                return "<color=#FFFFFF>";
                

            case Rarity.Common:
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
}
