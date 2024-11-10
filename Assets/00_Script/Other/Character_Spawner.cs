using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Spawner : MonoBehaviour
{
    public Transform[] SpawnTransform = new Transform[6];


    private void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            SpawnTransform[i] = transform.GetChild(i).transform;
        }
    }
}
