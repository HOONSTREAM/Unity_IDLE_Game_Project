using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int M_Count; // 몬스터의 수
    public float M_SpawnTime; // 몇 초마다 스폰이 될 것인지 결정.
    // 1. 몬스터는 여러마리가 몇 초 마다 수시로 여러번 스폰 되어야 한다.

    //Spawner 에 손쉽게 접근하기 위해, static으로 설계
    public static List<Monster> m_monsters = new List<Monster>();
    public static List<Player> m_players = new List<Player>();

    private Coroutine coroutine;

    private void Start()
    {
        Base_Manager.Stage.M_PlayEvent += OnPlay;
        Base_Manager.Stage.M_BossEvent += OnBoss;
    }
    public void OnPlay()
    {
        coroutine = StartCoroutine(SpawnCoroutine());
    }
    public void OnBoss()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        for(int i = 0; i<m_monsters.Count; i++)
        {
            if (m_monsters[i].isDead != true)
            {
                m_monsters[i].isDead = true;
                Base_Manager.Pool.m_pool_Dictionary["Monster"].Return(m_monsters[i].gameObject);
            }
            
        }
        m_monsters.Clear();

        StartCoroutine(BossSetCoroutine());
      
    }    

    IEnumerator BossSetCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        var monster = Instantiate(Resources.Load<Monster>("Boss"), Vector3.zero, Quaternion.Euler(0, 180, 0)); // 보스 생성
        monster.Init();

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
    //Random.insideUnitSphere = Vector3(x,y,z)
    //Random.insideUnitCircle = Vector3(x,y)
    IEnumerator SpawnCoroutine()
    {
        Vector3 pos;

        for(int i = 0; i < M_Count; i++)
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

                value.GetComponent<Monster>().Init();
                value.transform.position = pos;
                value.transform.LookAt(Vector3.zero);
                m_monsters.Add(value.GetComponent<Monster>());

            });

        }

        yield return new WaitForSeconds(M_SpawnTime);

        coroutine = StartCoroutine(SpawnCoroutine());
    }
   
}
