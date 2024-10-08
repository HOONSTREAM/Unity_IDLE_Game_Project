using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Monster_Prefab;

    public int M_Count; // 몬스터의 수
    public float M_SpawnTime; // 몇 초마다 스폰이 될 것인지 결정.
    // 1. 몬스터는 여러마리가 몇 초 마다 수시로 여러번 스폰 되어야 한다.


    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
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

            var go = Instantiate(Monster_Prefab, pos, Quaternion.identity);

        }

        yield return new WaitForSeconds(M_SpawnTime);

        StartCoroutine(SpawnCoroutine());
    }
   
}
