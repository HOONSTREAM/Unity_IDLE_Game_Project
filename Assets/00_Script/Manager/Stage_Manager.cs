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
    

    public static bool isDead = false;

    public  OnReadyEvent M_ReadyEvent;
    public  OnPlayEvent M_PlayEvent;
    public  OnBossEvent M_BossEvent;
    public  OnBossPlayEvent M_BossPlayEvent;
    public  OnClearEvent M_ClearEvent;
    public  OnDeadEvent M_DeadEvent;
    public  OnDungeonEvent M_DungeonEvent;

  
  
    public void State_Change(Stage_State state, int Value = 0)
    {
        M_State = state;
        switch(state)
        {
            case Stage_State.Ready:

                MaxCount = int.Parse(CSV_Importer.Spawn_Design[Data_Manager.Main_Players_Data.Player_Stage]["MaxCount"].ToString());

                Base_Manager.Data.Set_Player_ATK_HP();

                M_ReadyEvent?.Invoke();               
                Base_Manager.instance.Coroutine_Action(2.0f, () => State_Change(Stage_State.Play));
                break;
            case Stage_State.Play:
               
                M_PlayEvent?.Invoke();
                break;
            case Stage_State.Boss:
               
                Count = 0; // ī��Ʈ�ʱ�ȭ
                M_BossEvent?.Invoke();  
                break;
            case Stage_State.BossPlay:
              
                M_BossPlayEvent?.Invoke();
                break;
            case Stage_State.Clear:
                
                Base_Manager.instance.StopAllPoolCoroutines(); 
                Base_Manager.Pool.Clear_Pool(); // Ǯ����ü �ʱ�ȭ
                Data_Manager.Main_Players_Data.Player_Stage++;

                M_ClearEvent?.Invoke();
                break;
            case Stage_State.Dead:
               
                isDead = true;               
                M_DeadEvent?.Invoke();
                break;
            case Stage_State.Dungeon:
                M_DungeonEvent?.Invoke(Value);
                Base_Manager.instance.Coroutine_Action(2.0f, () => State_Change(Stage_State.Play));
                break;
        }
    }
}
