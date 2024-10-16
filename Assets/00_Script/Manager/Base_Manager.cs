using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Manager : MonoBehaviour
{
    public static Base_Manager instance;


    private static Pool_Manager _pool = new Pool_Manager();
    private static Player_Manager _player = new Player_Manager();
    public static Pool_Manager Pool { get { return _pool; } }
    public static Player_Manager Player { get { return _player; } }
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if(instance == null)
        {
            instance = this;
            Pool.Initialize(transform);
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    public GameObject Instantiate_Path(string path)
    {
        return Instantiate(Resources.Load<GameObject>(path));
    }

    public void Return_Pool(float timer, GameObject obj, string path)
    {
        StartCoroutine(Return_Pool_Coroutine(timer, obj, path));
    }

    IEnumerator Return_Pool_Coroutine(float timer, GameObject obj, string path)
    {
        yield return new WaitForSeconds(timer);
        Pool.m_pool_Dictionary[path].Return(obj);
    }

}
