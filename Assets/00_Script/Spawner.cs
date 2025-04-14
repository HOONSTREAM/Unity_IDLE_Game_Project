using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private int M_Count; // 몬스터의 수
    private float M_SpawnTime; // 몇 초마다 스폰이 될 것인지 결정.
    // 1. 몬스터는 여러마리가 몇 초 마다 수시로 여러번 스폰 되어야 한다.

    //Spawner 에 손쉽게 접근하기 위해, static으로 설계
    public static List<Monster> m_monsters = new List<Monster>();
    public static List<Player> m_players = new List<Player>();

    private Coroutine coroutine;

    [SerializeField]
    private GameObject[] Maps;
    [SerializeField]
    private GameObject Main_Game_Map;

    private void Start()
    {
        Base_Manager.Stage.M_ReadyEvent += OnReady;
        Base_Manager.Stage.M_PlayEvent += OnPlay;
        Base_Manager.Stage.M_BossEvent += OnBoss;
        Base_Manager.Stage.M_DungeonEvent += OnDungeon;
    }

    private void Stop_Coroutine_And_Delete_Monster()
    {
        if (coroutine != null)
        {
            StopAllCoroutines();
        }

        for (int i = 0; i < m_monsters.Count; i++)
        {
            if (m_monsters[i].isDead != true)
            {
                if (m_monsters[i].isBoss == false)
                {
                    m_monsters[i].isDead = true;
                    Base_Manager.Pool.m_pool_Dictionary["Monster"].Return(m_monsters[i].gameObject);                
                }

            }

        }

        m_monsters.Clear();
    }

    private void Back_To_MainGame_Map()
    {
        Maps[0].gameObject.SetActive(false);
        Maps[1].gameObject.SetActive(false);
        Maps[2].gameObject.SetActive(false);

        Main_Game_Map.gameObject.SetActive(true);

        Stage_Manager.isDungeon_Map_Change = false;

    }

    public void OnReady()
    {
        Stop_Coroutine_And_Delete_Monster();
        Back_To_MainGame_Map();
    }

    public void OnPlay()
    {
        if (Stage_Manager.isDungeon)
        {
            return;
        }

        M_Count = int.Parse(CSV_Importer.Spawn_Design[Data_Manager.Main_Players_Data.Player_Stage]["Spawn_Count"].ToString());
        M_SpawnTime = float.Parse(CSV_Importer.Spawn_Design[Data_Manager.Main_Players_Data.Player_Stage]["Spawn_Timer"].ToString());

        coroutine = StartCoroutine(SpawnCoroutine(M_Count, M_SpawnTime));
    }
    public void OnDungeon(int Value)
    {
        for(int i = 0; i <Maps.Length; i++)
        {
            Maps[i].gameObject.SetActive(false);
        }
        Main_Game_Map.gameObject.SetActive(false);
        Maps[Value].gameObject.SetActive(true);

        Stage_Manager.isDungeon_Map_Change = true;

        if(Value == 0) // 다이아몬드 던전
        {
            Stop_Coroutine_And_Delete_Monster();
            Base_Manager.Pool.Clear_Pool(); // 풀링객체 초기화
            coroutine = StartCoroutine(SpawnCoroutine(30, -1, (Stage_Manager.Dungeon_Level + 1) * 5));

        }

        if(Value == 1) // 골드 던전
        {
            Stop_Coroutine_And_Delete_Monster();
            Base_Manager.Pool.Clear_Pool(); // 풀링객체 초기화
            StartCoroutine(BossSetCoroutine());
        }

        if(Value == 2) // 승급 던전
        {
            Stop_Coroutine_And_Delete_Monster();
            Base_Manager.Pool.Clear_Pool(); // 풀링객체 초기화
            StartCoroutine(Tier_BossSetCoroutine());
        } 

       
    }

    public void OnBoss()
    {
        Stop_Coroutine_And_Delete_Monster();
        StartCoroutine(BossSetCoroutine());     
    }    

    IEnumerator BossSetCoroutine(int Player_Stage = 0)
    {
        yield return new WaitForSeconds(2.0f);
        Monster monster = null;

        if(Stage_Manager.isDungeon == false)
        {
            var go = Base_Manager.Pool.Pooling_OBJ("Boss").Get((value) =>
            {
                // 풀링이 생성될때의 기능을 구현한다.
                value.GetComponent<Monster>().Init(Player_Stage);
            });

            monster = go.GetComponent<Monster>();
        }

        else
        {

            var go = Base_Manager.Pool.Pooling_OBJ("Gold_Dungeon").Get((value) =>
            {
                // 풀링이 생성될때의 기능을 구현한다.
                value.GetComponent<Monster>().Init((Stage_Manager.Dungeon_Level + 1) * 10); // TODO : 레벨디자인 필요
            });

            monster = go.GetComponent<Monster>();
        }

        Vector3 Pos = monster.transform.position; // 같은 변수를 사용할 때는, 한 변수로 묶어서 사용하면 메모리 절약이 됨. (중복계산방지)


        // 일정 소환거리 내부에 플레이어가 존재하면, 보스 소환 시, 넉백을 합니다.
        for(int i = 0; i<m_players.Count; i++)
        {
            if(Vector3.Distance(Pos, m_players[i].transform.position) <= 3.0f)
            {
                m_players[i].transform.LookAt(monster.transform.position);
                m_players[i].Knock_Back();
            }
         
        }

        yield return new WaitForSeconds(1.5f);

        m_monsters.Add(monster);

        Base_Manager.Stage.State_Change(Stage_State.BossPlay);
    }

    IEnumerator Tier_BossSetCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        Monster monster = null;

        var go = Base_Manager.Pool.Pooling_OBJ("Tier_Dungeon").Get((value) =>
        {
            // 풀링이 생성될때의 기능을 구현한다.
            value.GetComponent<Monster>().Init((Stage_Manager.Dungeon_Level + 1) * 2); // TODO : 레벨디자인 필요
        });

        monster = go.GetComponent<Monster>();
        Vector3 Pos = monster.transform.position; // 같은 변수를 사용할 때는, 한 변수로 묶어서 사용하면 메모리 절약이 됨. (중복계산방지)


        // 일정 소환거리 내부에 플레이어가 존재하면, 보스 소환 시, 넉백을 합니다.
        for (int i = 0; i < m_players.Count; i++)
        {
            if (Vector3.Distance(Pos, m_players[i].transform.position) <= 3.0f)
            {
                m_players[i].transform.LookAt(monster.transform.position);
                m_players[i].Knock_Back();
            }

        }

        yield return new WaitForSeconds(1.5f);

        m_monsters.Add(monster);

        Base_Manager.Stage.State_Change(Stage_State.BossPlay);
    }
    //Random.insideUnitSphere = Vector3(x,y,z)
    //Random.insideUnitCircle = Vector3(x,y)
    IEnumerator SpawnCoroutine(int Count, float SpawnTime, int Dungeon_Difficulty_Value = 0)
    {
        
        Vector3 pos;

        int Monster_Spawn_Value = Count - m_monsters.Count;

        for(int i = 0; i < Monster_Spawn_Value; i++)
        {
            pos = Vector3.zero + Random.insideUnitSphere * 5.0f;
            pos.y = 0.0f;
            Vector3 returnPos = Vector3.zero;

            while (Vector3.Distance(pos, Vector3.zero) <= 3.0f)
            {
                pos = Vector3.zero + Random.insideUnitSphere * 5.0f;
                pos.y = 0.0f;
            }

            //몬스터 스폰
            var go = Base_Manager.Pool.Pooling_OBJ("Monster").Get((value) => 
            {
                // 풀링이 생성될때의 기능을 구현한다.

                value.GetComponent<Monster>().Init(Dungeon_Difficulty_Value);                
                value.transform.position = pos;
                value.transform.LookAt(Vector3.zero);
                m_monsters.Add(value.GetComponent<Monster>());

            });

        }

        yield return new WaitForSeconds(SpawnTime);

        if(SpawnTime > 0.0f)
        {
            coroutine = StartCoroutine(SpawnCoroutine(Count, SpawnTime));
        }
       
    }
   
}
