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
    public static int Dungeon_Enter_Type = 0;
    public static int Dungeon_Level = 0; // 최종 클리어한 레벨과 다르게, 유저가 지정한 레벨을 의미합니다.

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
                Debug.Log("Stage : Ready");
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
                Debug.Log("Stage : Play");
                M_PlayEvent?.Invoke();
                break;
            case Stage_State.Boss:
                Debug.Log("Stage : Boss");
                Count = 0; // 카운트초기화
                M_BossEvent?.Invoke();  
                break;
            case Stage_State.BossPlay:
                Debug.Log("Stage : BossPlay");
                M_BossPlayEvent?.Invoke();
                break;
            case Stage_State.Clear:
                Debug.Log("Stage : Clear");
                Base_Manager.instance.StopAllPoolCoroutines(); 
                Base_Manager.Pool.Clear_Pool(); // 풀링객체 초기화
                Data_Manager.Main_Players_Data.Player_Stage++;

                M_ClearEvent?.Invoke();
                break;
            case Stage_State.Dead:
                Debug.Log("Stage : Dead");
                isDead = true;               
                M_DeadEvent?.Invoke();
                break;
            case Stage_State.Dungeon:
                Dungeon_Enter_Type = Value;
                Debug.Log("Stage : Dungeon");
                isDungeon = true;
                DungeonCount = FULL_MAX_COUNT;
                M_DungeonEvent?.Invoke(Value);
                Base_Manager.instance.Coroutine_Action(2.0f, () => State_Change(Stage_State.Play));
                break;

            case Stage_State.Dungeon_Clear:
                Debug.Log("Stage : Dungeon_Clear");
                isDungeon = false;
                M_DungeonClearEvent?.Invoke(Value);               
                break;

            case Stage_State.Dungeon_Dead:
                Debug.Log("Stage : Dungeon_Dead");
                isDungeon = false;
                M_DungeonDeadEvent?.Invoke();              
                break;
        }
    }
}
