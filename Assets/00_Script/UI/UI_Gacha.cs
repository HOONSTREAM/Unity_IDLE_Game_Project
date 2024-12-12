using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Gacha : UI_Base
{

    [SerializeField]
    private Image Gacha_Hero_Parts;
    public Transform Content;
    [SerializeField]
    private GameObject Rare_Particle;
    public override bool Init()
    {
        return base.Init();
    }

    public void Get_Gacha_Hero(int Hero_Amount_Value)
    {
        StartCoroutine(GaCha_Coroutine(Hero_Amount_Value));
    }

    IEnumerator GaCha_Coroutine(int Hero_Amount_Value)
    {
        for (int i = 0; i < Hero_Amount_Value; i++)
        {
            Rarity rarity = Rarity.Common;
            float R_Percentage = 0.0f;
            float Percentage = Random.Range(0.0f, 100.0f);
            var go = Instantiate(Gacha_Hero_Parts, Content);
            yield return new WaitForSeconds(0.1f);

            for(int j = 0; j < 5; j++)
            {
                R_Percentage += Utils.Gacha_Percentage[j];
                if (Percentage <= R_Percentage)
                {
                    rarity = (Rarity)j;                  
                    break;
                }
            }

            Debug.Log(rarity);

            if(rarity >= Rarity.Epic)
            {
                Rare_Particle.gameObject.SetActive(true);
            }
        }
    }

}
