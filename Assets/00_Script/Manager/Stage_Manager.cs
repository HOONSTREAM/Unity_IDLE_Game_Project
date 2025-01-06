using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Stage_Manager
{ 
    public static Stage_State M_State;
    public static int MaxCount = 3;
    public static int Count;
    public static int DungeonCount = 30;
    

    public static bool isDead = false;
    public static bool isDungeon = false;

    public  OnReadyEvent M_ReadyEvent;
    public  OnPlayEvent M_PlayEvent;
    public  OnBossEvent M_BossEvent;
    public  OnBossPlayEvent M_BossPlayEvent;
    public  OnClearEvent M_ClearEvent;
    public  OnDeadEvent M_DeadEvent;
    public  OnDungeonEvent M_DungeonEvent;
    public  OnDungeonClearEvent M_DungeonClearEvent;
    public  OnDungeonDeadEvent M_DungeonDeadEvent;

    private const int FULL_MAX_COUNT = 30;
  
    public void State_Change(Stage_State state, int Value = 0)
    {
        M_State = state;
        switch(state)
        {
            case Stage_State.Ready:

                MaxCount = int.Parse(CSV_Importer.Spawn_Design[Data_Manager.Main_Players_Data.Player_Stage]["MaxCount"].ToString());

                if(MaxCount > 30)
                {
                    MaxCount = FULL_MAX_COUNT;
                }

                Base_Manager.Data.Set_Player_ATK_HP();

                M_ReadyEvent?.Invoke();               
                Base_Manager.instance.Coroutine_Action(2.0f, () => State_Change(Stage_State.Play));
                break;
            case Stage_State.Play:
               
                M_PlayEvent?.Invoke();
                break;
            case Stage_State.Boss:
               
                Count = 0; // 카운트초기화
                M_BossEvent?.Invoke();  
                break;
            case Stage_State.BossPlay:
              
                M_BossPlayEvent?.Invoke();
                break;
            case Stage_State.Clear:
                
                Base_Manager.instance.StopAllPoolCoroutines(); 
                Base_Manager.Pool.Clear_Pool(); // 풀링객체 초기화
                Data_Manager.Main_Players_Data.Player_Stage++;

                M_ClearEvent?.Invoke();
                break;
            case Stage_State.Dead:
               
                isDead = true;               
                M_DeadEvent?.Invoke();
                break;
            case Stage_State.Dungeon:
                isDungeon = true;
                DungeonCount = FULL_MAX_COUNT;
                M_DungeonEvent?.Invoke(Value);
                Base_Manager.instance.Coroutine_Action(2.0f, () => State_Change(Stage_State.Play));
                break;

            case Stage_State.Dungeon_Clear:
                isDungeon = false;
                M_DungeonClearEvent?.Invoke(Value);               
                break;

            case Stage_State.Dungeon_Dead:               
                M_DungeonDeadEvent?.Invoke();              
                break;
        }
    }
}
