using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Spawner : MonoBehaviour
{
    public Transform[] SpawnTransform = new Transform[6];
    public static Player[] players = new Player[6];


    private void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            SpawnTransform[i] = transform.GetChild(i).transform;
        }
       
    }

    /// <summary>
    /// 메인 게임에 영웅을 배치하는 메서드 입니다.
    /// </summary>
    public void Set_Hero_Main_Game()
    {
        
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
            {               
                if (Spawner.m_players.Contains(players[i]))
                {
                    Spawner.m_players.Remove(players[i]);                    
                }

                
                Destroy(players[i].gameObject); // 기존 오브젝트 삭제
               
                players[i] = null;// 참조 초기화
            }
        }

        

        // 2. Base_Manager.Character.Set_Character를 기반으로 새 영웅 배치
        for (int i = 0; i < Base_Manager.Character.Set_Character.Length; i++)
        {            
            var Data = Base_Manager.Character.Set_Character[i];
            if (Data != null) // 유효한 데이터만 처리
            {
                Instatiate_Player(Data, i); // 새 영웅 생성 및 배치
            }
        }

    }

    private void Instatiate_Player(Character_Holder Data, int value)
    {
        string temp = Data.Data.M_Character_Name;
        var go = Instantiate(Resources.Load<GameObject>("Character/" + temp));
        players[value] = go.GetComponent<Player>();
        go.transform.position = SpawnTransform[value].transform.position;
        go.transform.LookAt(Vector3.zero);
    }
}
