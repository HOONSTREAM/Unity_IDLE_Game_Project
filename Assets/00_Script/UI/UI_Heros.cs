using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Heros : UI_Base
{
    public Transform Content;
    public GameObject Parts;

    private Dictionary<string, Character_Scriptable> _dict = new Dictionary<string, Character_Scriptable>();
    private void Start()
    {
        var Data = Resources.LoadAll<Character_Scriptable>("Scriptable");

        for(int i = 0; i < Data.Length; i++)
        {
            _dict.Add(Data[i].M_Character_Name, Data[i]);

        }

        var sort_dict = _dict.OrderByDescending(x => x.Value.Rarity);


        foreach(var data in sort_dict)
        {
            var Object = Instantiate(Parts, Content).GetComponent<UI_Heros_Parts>(); // Content를 부모오브젝트로 해서 Parts를 생성
            Object.Init(data.Value);
        }
    }


}
