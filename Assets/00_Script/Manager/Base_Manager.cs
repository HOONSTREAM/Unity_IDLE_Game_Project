using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Manager : MonoBehaviour
{
    public static Base_Manager instance;


    private static Pool_Manager _pool = new Pool_Manager();
    private static Player_Manager _player = new Player_Manager();
    private static Stage_Manager _stage = new Stage_Manager();
    private static Data_Manager _data = new Data_Manager();
    private static Item_Manager _item = new Item_Manager();
    private static Character_Manager _character = new Character_Manager();
    private static Inventory_Manager _inventory = new Inventory_Manager();
    private static ADS_Manager _ads = new ADS_Manager();
    
 
    public static Pool_Manager Pool { get { return _pool; } }
    public static Player_Manager Player { get { return _player; } }
    public static Stage_Manager Stage { get { return _stage; } }
    public static Data_Manager Data { get { return _data; } }
    public static Item_Manager Item { get { return _item; } }
    public static Character_Manager Character { get { return _character; } }
    public static Inventory_Manager Inventory { get { return _inventory; } }
    public static ADS_Manager ADS { get { return _ads; } }

    public static bool is_Fast_Mode = false;

    private void Awake()
    {
        Init();
    }


    private void Update()
    {
        for (int i = 0; i < Data.Buff_Timers.Length; i++)
        {
            if (Data.Buff_Timers[i] >= 0.0f)
            {
                Data.Buff_Timers[i] -= Time.unscaledDeltaTime;
            }           
        }

        if(Data.buff_x2_speed > 0.0f)
        {
            Data.buff_x2_speed -= Time.unscaledDeltaTime;
        }
    }
    private void Init()
    {
        
        if(instance == null)
        {
            instance = this;
            Pool.Initialize(transform);
            ADS.Init();           
            Item.Init();
            Data.Init();
            StartCoroutine(Action_Coroutine(()=>Base_Manager.Stage.State_Change(Stage_State.Ready), 0.3f));
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    public GameObject Instantiate_Path(string path)
    {
        return Instantiate(Resources.Load<GameObject>(path));
    }

    public void Return_Pool(float timer, GameObject obj, string path)
    {
        StartCoroutine(Return_Pool_Coroutine(timer, obj, path));
    }

    IEnumerator Return_Pool_Coroutine(float timer, GameObject obj, string path)
    {
        yield return new WaitForSeconds(timer);

        if (Pool.m_pool_Dictionary.Count == 0 || !Pool.m_pool_Dictionary.ContainsKey(path))
        {
            yield break;
        }

        Pool.m_pool_Dictionary[path].Return(obj);
    }

    IEnumerator Action_Coroutine(Action action, float timer)
    {
        yield return new WaitForSeconds(timer);

        action?.Invoke();
    }
    
    public void Coroutine_Action(float timer, Action action)
    {
        StartCoroutine(Action_Coroutine(action, timer));
    }

    public void StopAllPoolCoroutines()
    {
        StopAllCoroutines(); // 현재 실행 중인 모든 코루틴 중지
    }

    


}
