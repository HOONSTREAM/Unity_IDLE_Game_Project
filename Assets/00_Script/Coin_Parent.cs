using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Coin_Parent : MonoBehaviour
{
    private Vector3 target;
    private Camera cam;
    RectTransform[] childs = new RectTransform[5];

    [Range(0.0f, 500.0f)]
    [SerializeField]
    private float Distance_Range, Speed;



    private void Awake()
    {
        cam = Camera.main;
        for(int i =0; i<childs.Length; i++)
        {
            childs[i] = transform.GetChild(i).GetComponent<RectTransform>();
        }

       
    }

    private void OnSave()
    {
        Data_Manager.Main_Players_Data.Player_Money += Utils.Data.stageData.Get_DROP_MONEY();

        if (Distance_Boolean_World(0.5f))
        {
            Base_Manager.Pool.m_pool_Dictionary["COIN_PARENT"].Return(this.gameObject);
        }

    }

    private void OnDisable()
    {
        Saving_Mode.onSaving -= OnSave;
    }

    public void Init(Vector3 pos, Coin_Type type = Coin_Type.Gold, int reward_value = 0)
    {
        Saving_Mode.onSaving += OnSave;

        if (Base_Canvas.isSavingMode)
        {
            return;
        }


        target = pos;
        transform.position = cam.WorldToScreenPoint(pos);
        for(int i = 0; i<childs.Length;i++)
        {
            childs[i].GetComponent<Image>().sprite = Utils.Get_Atlas(type.ToString());
            childs[i].anchoredPosition = Vector2.zero;
        }

        switch (type)
        {
            case Coin_Type.Gold:
                Data_Manager.Main_Players_Data.Player_Money += Utils.Data.stageData.Get_DROP_MONEY();
                break;
            case Coin_Type.Dia:
                Data_Manager.Main_Players_Data.DiaMond += reward_value;
                break;

        }
        
        transform.parent = Base_Canvas.instance.Holder_Layer(0);

        
        StartCoroutine(Coin_Effect());
    }

    IEnumerator Coin_Effect()
    {
        Vector2[] RandomPos = new Vector2[childs.Length];
        for(int i = 0; i < childs.Length; i++)
        {
            RandomPos[i] = new Vector2(target.x, target.y) + Random.insideUnitCircle * Random.Range(-Distance_Range, Distance_Range);
        }

        while (true)
        {
            for(int i = 0; i<childs.Length;i++)
            {
                RectTransform rect = childs[i];

                rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, RandomPos[i], Time.deltaTime * Speed);
            }

            if (Distance_Boolean(RandomPos, 0.5f))
            {
                break;
            }

            yield return null;  // 한번의 프레임 대기
        }

        yield return new WaitForSeconds(0.3f);

        while (true)
        {
            for(int i = 0; i<childs.Length;i++)
            {
                RectTransform rect = childs[i];
                rect.position = Vector2.MoveTowards(rect.position, Base_Canvas.instance.Coin.position, Time.deltaTime * (Speed * 10.0f));
            }

            if (Distance_Boolean_World(0.5f))
            {
                Base_Manager.Pool.m_pool_Dictionary["COIN_PARENT"].Return(this.gameObject);
                break;
            }
            yield return null;
        }

        Main_UI.Instance.Level_Text_Check();
    }

    private bool Distance_Boolean(Vector2[] end, float range)
    {
        for(int i = 0; i < childs.Length ; i++)
        {
            float distance = Vector2.Distance(childs[i].anchoredPosition, end[i]);

            if(distance > range)
            {
                return false;
            }
            
        }

        return true;
    }

    private bool Distance_Boolean_World(float Range)
    {
        for(int i = 0;i < childs.Length ; i++)
        {
            float distance = Vector2.Distance(childs[i].position, Base_Canvas.instance.Coin.position);
            if(distance > Range)
            {
                return false;
            }
        }

        return true;
    }

}
