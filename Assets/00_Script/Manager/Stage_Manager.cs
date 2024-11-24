using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public delegate void OnReadyEvent();
public delegate void OnPlayEvent();
public delegate void OnBossEvent();
public delegate void OnBossPlayEvent();
public delegate void OnClearEvent();
public delegate void OnDeadEvent();

public class Stage_Manager
{ 
    public static Stage_State M_State;
    public static int MaxCount = 3;
    public static int Count;
    

    public static bool isDead = false;

    public  OnReadyEvent M_ReadyEvent;
    public  OnPlayEvent M_PlayEvent;
    public  OnBossEvent M_BossEvent;
    public  OnBossPlayEvent M_BossPlayEvent;
    public  OnClearEvent M_ClearEvent;
    public  OnDeadEvent M_DeadEvent;

  
  
    public void State_Change(Stage_State state)
    {
        M_State = state;
        switch(state)
        {
            case Stage_State.Ready:
                MaxCount = int.Parse(CSV_Importer.Spawn_Design[Base_Manager.Data.Player_Stage]["MaxCount"].ToString());
                Debug.Log("Ready");
                M_ReadyEvent?.Invoke();
                //Base_Manager.Pool.Clear_Pool(); // 풀링된 오브젝트의 과다생성을 방지하기 위해 한번 초기화
                Base_Manager.instance.Coroutine_Action(2.0f, () => State_Change(Stage_State.Play));
                break;
            case Stage_State.Play:
                Debug.Log("Play");
                M_PlayEvent?.Invoke();
                break;
            case Stage_State.Boss:
                Debug.Log("Boss");
                Count = 0; // 카운트초기화
                M_BossEvent?.Invoke();  
                break;
            case Stage_State.BossPlay:
                Debug.Log("BossPlay");
                M_BossPlayEvent?.Invoke();
                break;
            case Stage_State.Clear:
                Debug.Log("Clear");                
                Base_Manager.Data.Player_Stage++;

                M_ClearEvent?.Invoke();
                break;
            case Stage_State.Dead:
                Debug.Log("Dead");
                isDead = true;               
                M_DeadEvent?.Invoke();
                break;
        }
    }
}
