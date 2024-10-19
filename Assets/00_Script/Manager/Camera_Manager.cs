using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Manager : MonoBehaviour
{
    float Main_Distance = 5.0f;
    [Range(0.0f, 10.0f)]
    [SerializeField]
    private float Main_Distance_Value;

    private Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize,Monster_Distance(),Time.deltaTime * 2.0f);
    }

    private float Monster_Distance()
    {
        var players = Spawner.m_players.ToArray();
        float maxdistance = Main_Distance;

        foreach(var player in players)
        {
            float targetDistance = Vector3.Distance(Vector3.zero, player.transform.position) + Main_Distance_Value;

            if(targetDistance > maxdistance)
            {
                maxdistance = targetDistance;
            }
        }

        return maxdistance;
    }
}
