using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage_Manager
{

   

    public static Stage_State M_State;
    public static int MaxCount = 3;
    public static int Count;
    public static int DungeonCount = 30;
    

    public static bool isDead = false;
    public static bool isDungeon = false; // 던전 진행중인지 검사
    public static bool isDungeon_Map_Change = false; // 맵이 변경되어 던전이 진행 되었으면 true, 끝나서 메인 맵으로 되돌아왔으면 false
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

    public const int MULTIPLE_REWARD_GOLD_DUNGEON = 10000;
    public const int MULTIPLE_REWARD_DIAMOND_DUNGEON = 50;


    public void State_Change(Stage_State state, int Value = 0)
    {

        if(SceneManager.GetActiveScene().name != "MainGame")
        {
            Debug.Log("현재 Scene이 MainGame이 아닙니다.");
            return;
        }

        M_State = state;
        
        switch (state)
        {
            case Stage_State.Ready:
                Debug.Log("Stage : Ready");
                Count = 0; // 카운트초기화
                MaxCount = int.Parse(CSV_Importer.Spawn_Design[Data_Manager.Main_Players_Data.Player_Stage]["MaxCount"].ToString());

                if(MaxCount > 30)
                {
                    MaxCount = FULL_MAX_COUNT;
                }

                GameObject.Find("@Character_Spawner").gameObject.GetComponent<Character_Spawner>().Set_Hero_Main_Game();
                Base_Manager.Data.Set_Player_ATK_HP();
                Spawner.m_players.RemoveAll(player => player == null || !player.gameObject.activeInHierarchy); // 레디상태마다, m_players를 정리합니다.        
                M_ReadyEvent?.Invoke();                
                Base_Manager.instance.Coroutine_Action(2.0f, () => State_Change(Stage_State.Play));
                break;
            case Stage_State.Play:
                Debug.Log("Stage : Play");
                M_PlayEvent?.Invoke();
                break;
            case Stage_State.Boss:
                Debug.Log("Stage : Boss");
                Spawner.m_players.RemoveAll(player => player == null || !player.gameObject.activeInHierarchy);
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
                Base_Manager.Pool.Clear_Pool();
                M_DeadEvent?.Invoke();
                break;
            case Stage_State.Dungeon:
                Dungeon_Enter_Type = Value;
                Debug.Log("Stage : Dungeon");
                Base_Manager.SOUND.Play(Sound.BGM, "Dungeon");
                isDungeon = true;
                DungeonCount = FULL_MAX_COUNT;
                M_DungeonEvent?.Invoke(Value);
                Base_Manager.instance.Coroutine_Action(2.0f, () => State_Change(Stage_State.Play));
                break;

            case Stage_State.Dungeon_Clear:
                Debug.Log("Stage : Dungeon_Clear");                   
                Base_Manager.SOUND.Play(Sound.BGM, "Main");                
                Base_Manager.Pool.Clear_Pool();
                M_DungeonClearEvent?.Invoke(Value);
                isDungeon = false;
                break;

            case Stage_State.Dungeon_Dead:
                Debug.Log("Stage : Dungeon_Dead");
                Base_Manager.Pool.Clear_Pool();
                Base_Manager.SOUND.Play(Sound.BGS, "Lose");
                Base_Manager.SOUND.Play(Sound.BGM, "Main");               
                M_DungeonDeadEvent?.Invoke();
                isDungeon = false;
                break;
        }
    }
}
