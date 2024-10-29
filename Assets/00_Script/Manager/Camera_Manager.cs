using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Manager : MonoBehaviour
{

    public static Camera_Manager instance = null;

    float Main_Distance = 5.0f;
    [Range(0.0f, 10.0f)]
    [SerializeField]
    private float Main_Distance_Value;
    [Space(20f)]
    [Header("Camera Shake")]
    [Range(0.0f, 10.0f)]
    [SerializeField]
    private float Duration;
    [Range(0.0f, 10.0f)]
    [SerializeField]
    private float Power;

    private Vector3 Original_Position;
    private Camera _camera;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        _camera = GetComponent<Camera>();
        Original_Position = transform.localPosition;
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

    public void Camera_Shake()
    {
        StartCoroutine(Camera_Coroutine());
    }

    IEnumerator Camera_Coroutine()
    {
        float timer = 0.0f;

        while(timer <= Duration)
        {
            transform.localPosition = Random.insideUnitSphere * Power + Original_Position;

            timer += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = Original_Position;
    }
}
