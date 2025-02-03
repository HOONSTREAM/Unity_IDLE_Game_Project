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
    /// ���� ���ӿ� ������ ��ġ�ϴ� �޼��� �Դϴ�.
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

                
                Destroy(players[i].gameObject); // ���� ������Ʈ ����
               
                players[i] = null;// ���� �ʱ�ȭ
            }
        }

        

        // 2. Base_Manager.Character.Set_Character�� ������� �� ���� ��ġ
        for (int i = 0; i < Base_Manager.Character.Set_Character.Length; i++)
        {            
            var Data = Base_Manager.Character.Set_Character[i];
            if (Data != null) // ��ȿ�� �����͸� ó��
            {
                Instatiate_Player(Data, i); // �� ���� ���� �� ��ġ
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
