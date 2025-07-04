using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage_Manager
{

    private Coroutine bossTimerCoroutine;
    private const float BOSS_TIME_LIMIT = 30.0f;

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

    public const int MULTIPLE_REWARD_GOLD_DUNGEON = 500000;
    public const int MULTIPLE_REWARD_DIAMOND_DUNGEON = 20;
    public const int MAX_STAGE = 599999;

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

                if (bossTimerCoroutine != null)
                    Base_Manager.instance.StopCoroutine(bossTimerCoroutine);

                // 새 코루틴 시작
                bossTimerCoroutine = Base_Manager.instance.StartCoroutine(BossTimer());
                break;

            case Stage_State.Clear:

                Debug.Log("Stage : Clear");

                if (!Base_Canvas.isSavingMode)
                {
                    Base_Manager.SOUND.Play(Sound.BGS, "Clear");
                    Base_Canvas.instance.Get_Stage_Clear_Popup().Initialize("스테이지 클리어");
                }
                
                
                Base_Manager.instance.StopAllPoolCoroutines(); 
                Base_Manager.Pool.Clear_Pool(); // 풀링객체 초기화
                Data_Manager.Main_Players_Data.Player_Stage++;

                if(Data_Manager.Main_Players_Data.Player_Max_Stage < Data_Manager.Main_Players_Data.Player_Stage)
                {
                    Data_Manager.Main_Players_Data.Player_Max_Stage = Data_Manager.Main_Players_Data.Player_Stage;
                }
                

                if (bossTimerCoroutine != null)
                {
                    Base_Manager.instance.StopCoroutine(bossTimerCoroutine);
                    bossTimerCoroutine = null;
                }

                if (Base_Manager.Item.Set_Item_Check("GOLD_REWARD"))
                {
                    var value = "GOLD_REWARD";
                    var effect_value = float.Parse(CSV_Importer.RELIC_GOLD_REWARD_Design[Base_Manager.Data.Item_Holder[value].Hero_Level]["effect_percent"].ToString());
                    Data_Manager.Main_Players_Data.Player_Money += effect_value;

                    Debug.Log($"{StringMethod.ToCurrencyString(effect_value)}골드 만큼 GOLD_REWARD 유물을 장착하여 지급함.");
                }

                M_ClearEvent?.Invoke();

                if (Data_Manager.Main_Players_Data.Player_Stage >= MAX_STAGE)
                {
                    Data_Manager.Main_Players_Data.Player_Stage = MAX_STAGE;
                    Base_Canvas.instance.Get_TOP_Popup().Initialize("최고 층에 도달하였습니다.");
                    break;
                }

                break;
            case Stage_State.Dead:
                Debug.Log("Stage : Dead");
                isDead = true;
                Base_Manager.Pool.Clear_Pool();

                if (bossTimerCoroutine != null)
                {
                    Base_Manager.instance.StopCoroutine(bossTimerCoroutine);
                    bossTimerCoroutine = null;
                }


                M_DeadEvent?.Invoke();
                break;
            case Stage_State.Dungeon:
                Dungeon_Enter_Type = Value;
                Debug.Log("Stage : Dungeon");
                Base_Manager.SOUND.Play(Sound.BGM, "Tier_Dungeon");
                isDungeon = true;
                DungeonCount = FULL_MAX_COUNT;
                M_DungeonEvent?.Invoke(Value);
                Base_Manager.instance.Coroutine_Action(2.0f, () => State_Change(Stage_State.Play));
                break;

            case Stage_State.Dungeon_Clear:
                Debug.Log("Stage : Dungeon_Clear");                   
                Base_Manager.SOUND.Play(Sound.BGM, "Village");                
                Base_Manager.Pool.Clear_Pool();
                M_DungeonClearEvent?.Invoke(Value);
                isDungeon = false;
                break;

            case Stage_State.Dungeon_Dead:
                Debug.Log("Stage : Dungeon_Dead");
                Base_Manager.Pool.Clear_Pool();
                Base_Manager.SOUND.Play(Sound.BGS, "Lose");
                Base_Manager.SOUND.Play(Sound.BGM, "Village");               
                M_DungeonDeadEvent?.Invoke();
                isDungeon = false;
                break;
        }
    }

    private IEnumerator BossTimer()
    {
        yield return new WaitForSecondsRealtime(BOSS_TIME_LIMIT);

        if (M_State == Stage_State.BossPlay)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("제한시간 30초 경과 ! 보스를 물리치기엔 전투력이 낮습니다.");
            State_Change(Stage_State.Dead);
        }
    }
}
