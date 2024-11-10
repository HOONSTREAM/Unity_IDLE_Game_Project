using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Heros : UI_Base
{
    public Transform Content;
    public GameObject Parts;

    private Dictionary<string, Character_Scriptable> _dict = new Dictionary<string, Character_Scriptable>();

    public override bool Init()
    {
        Main_UI.Instance.FadeInOut(true, true, null);


        var Data = Base_Manager.Data.Data_Character_Dictionary;

        foreach (var data in Data)
        {
            _dict.Add(data.Value.Data.M_Character_Name, data.Value.Data);
        }
      

        var sort_dict = _dict.OrderByDescending(x => x.Value.Rarity);


        foreach (var data in sort_dict)
        {
            var Object = Instantiate(Parts, Content).GetComponent<UI_Heros_Parts>(); // Content를 부모오브젝트로 해서 Parts를 생성
            Object.Init(data.Value);
        }

        return base.Init();
    }

    public override void DisableOBJ()
    {
        Main_UI.Instance.FadeInOut(false, true, () =>
        {
            Main_UI.Instance.FadeInOut(true, false, null);
            base.DisableOBJ();
        });

    }

}
