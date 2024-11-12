using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Spawner : MonoBehaviour
{
    public Transform[] SpawnTransform = new Transform[6];
    public static Player[] players = new Player[6];


    private void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            SpawnTransform[i] = transform.GetChild(i).transform;
        }

        Base_Manager.Stage.M_ReadyEvent += Set_Hero_Main_Game;
    }

    /// <summary>
    /// 메인 게임에 영웅을 배치하는 메서드 입니다.
    /// </summary>
    public void Set_Hero_Main_Game()
    {
        for(int i = 0; i< Base_Manager.Character.Set_Character.Length; i++)
        {
            var Data = Base_Manager.Character.Set_Character[i];

            if(Data != null)
            {
                if (players[i] != null)
                {
                    if (players[i].CH_Data != Data.Data)
                    {
                        Destroy(players[i].gameObject);
                        Instatiate_Player(Data, i);
                    }
                }

                else
                {
                    Instatiate_Player(Data, i);
                }
              
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
