using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    private static BackEnd_Manager _backEnd = new BackEnd_Manager();
    private static Sound_Manager _sound = new Sound_Manager();
    private static Localization_Manager _local = new Localization_Manager();
    private static Daily_Quest_Manager _daily = new Daily_Quest_Manager();
      
 
    public static Pool_Manager Pool { get { return _pool; } }
    public static Player_Manager Player { get { return _player; } }
    public static Stage_Manager Stage { get { return _stage; } }
    public static Data_Manager Data { get { return _data; } }
    public static Item_Manager Item { get { return _item; } }
    public static Character_Manager Character { get { return _character; } }
    public static Inventory_Manager Inventory { get { return _inventory; } }
    public static ADS_Manager ADS { get { return _ads; } }
    public static BackEnd_Manager BACKEND {  get { return _backEnd; } }
    public static Sound_Manager SOUND { get { return _sound; } }
    public static Localization_Manager LOCAL { get { return _local; } }
    public static Daily_Quest_Manager DAILY { get { return _daily; } }

 
    public static bool Get_MainGame_Start = false;
    private float Save_Interval = 10.0f;
    private float Save_Time = 0.0f;

    private void Awake()
    {
        Init();        
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if(Get_MainGame_Start == false)
        {
            return;
        }

        Save_Time += Time.unscaledDeltaTime;

        if(Save_Time >= Save_Interval)
        {
            Save_Time = 0.0f;
            _=Base_Manager.BACKEND.WriteData();
        }
      
        if (Data_Manager.Main_Players_Data.buff_x2_speed > 0.0f)
        {
            Data_Manager.Main_Players_Data.buff_x2_speed -= Time.unscaledDeltaTime;
        }

        Update_ADS_Buffs();
        Update_ADS_Timer();
       
    }
    private void Init()
    {
        
        if(instance == null)
        {
            instance = this;
            Pool.Initialize(transform);
            ADS.Init();
            Data.Init();
            SOUND.Init();
            DAILY.Init();            
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 광고버프 타이머를 측정합니다.
    /// </summary>
    private void Update_ADS_Buffs()
    {
        bool hasActiveBuff = false;

        for (int i = 0; i < Data_Manager.Main_Players_Data.Buff_Timers.Length; i++)
        {
            if (Data_Manager.Main_Players_Data.Buff_Timers[i] > 0.0f)
            {
                Data_Manager.Main_Players_Data.Buff_Timers[i] = Mathf.Max(0,
                    Data_Manager.Main_Players_Data.Buff_Timers[i] - Time.unscaledDeltaTime);
                hasActiveBuff = true;
            }
        }

        if (!hasActiveBuff) return; // 모든 버프가 0이면 더 이상 실행하지 않음
    }

    /// <summary>
    /// 유물 및 영웅 광고소환에 대한 타이머를 측정합니다.
    /// </summary>
    private void Update_ADS_Timer()
    {
        bool hasActiveADS = false;

        for (int i = 0; i < Data_Manager.Main_Players_Data.ADS_Timer.Length; i++)
        {
            if (Data_Manager.Main_Players_Data.ADS_Timer[i] > 0.0f)
            {
                Data_Manager.Main_Players_Data.ADS_Timer[i] = Mathf.Max(0,
                    Data_Manager.Main_Players_Data.ADS_Timer[i] - Time.unscaledDeltaTime);
                hasActiveADS = true;
            }
        }

        if (!hasActiveADS) return;
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

    /// <summary>
    /// 백그라운드에 간 시간을 계산하여, 오프라인 보상을 계산하여 지급합니다.
    /// </summary>
    /// <param name="pause"></param>
    private void OnApplicationPause(bool pause)
    {
        if(pause == true)
        {
            Data_Manager.Main_Players_Data.EndDate = Utils.Get_Server_Time();
        }

        else
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("앱으로 다시 돌아왔습니다.");

            Data_Manager.Main_Players_Data.StartDate = Utils.Get_Server_Time();

            if (Utils.Offline_Timer_Check() >= 300.0d)
            {
                Base_Canvas.instance.Get_UI("OFFLINE_REWARD");
                Base_Manager.SOUND.Play(Sound.BGS, "OFFLINE");
            }
        }

        _=Base_Manager.BACKEND.WriteData();
    }

    /// <summary>
    /// 어플리케이션 종료 시, 게임을 저장합니다.
    /// </summary>
    private void OnApplicationQuit()
    {
        if (Base_Manager.Get_MainGame_Start)
        {
            Debug.Log("게임을 정상적으로 종료하고, 데이터를 저장합니다.");
            _ = Base_Manager.BACKEND.WriteData();
        }
    }




}
