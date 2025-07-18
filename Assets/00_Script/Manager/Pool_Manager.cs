using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public interface IPool
{
    Transform parentTransform { get; set; }
    Queue<GameObject> pool { get; set; }
    GameObject Get(Action<GameObject> action = null);

    void Return(GameObject obj, Action<GameObject> action = null);

}


public class Object_Pool : IPool
{
    public Queue<GameObject> pool { get; set; } = new Queue<GameObject>();

    public Transform parentTransform { get; set; }

    /// <summary>
    /// 몬스터 오브젝트 풀링객체를 queue에서 빼오고, 오브젝트를 활성화 시킵니다.
    /// 그리고, 특정 액션(기능)이 구현되어 있다면 실행시킵니다.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public GameObject Get(Action<GameObject> action = null)
    {
        GameObject obj = null;

        while (pool.Count > 0)
        {
            obj = pool.Dequeue();

            if (obj == null || obj.Equals(null)) continue; // Destroy된 객체는 무시

            obj.SetActive(true);

            action?.Invoke(obj);
            return obj;
        }

        Debug.LogWarning("[Object_Pool] 풀에 유효한 오브젝트가 없습니다.");
        return null;
    }

    /// <summary>
    /// 몬스터 오브젝트 풀링객체를 queue에 넣고, 오브젝트를 비활성화 시킵니다.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="action"></param>
    public void Return(GameObject obj, Action<GameObject> action = null)
    {
        if (obj == null || obj.Equals(null))
        {
            Debug.LogWarning("[Object_Pool] Return 시도된 오브젝트가 null 또는 파괴된 상태입니다.");
            return;
        }

        try
        {
            obj.transform.parent = parentTransform;
            obj.SetActive(false);
            pool.Enqueue(obj);
            action?.Invoke(obj);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[Object_Pool] Return 중 예외 발생: {ex.Message}\n{ex.StackTrace}");
        }

    }

}
public class Pool_Manager 
{
    // IPool 인터페이스를 value로 반환하는 딕셔너리를 new로 생성합니다.
    public Dictionary<string, IPool> m_pool_Dictionary = new Dictionary<string, IPool>();

    /// <summary>
    /// 베이스 매니저 오브젝트의 트랜스폼이며, 모든 매니저는 베이스 매니저 오브젝트 산하에 위치 시킵니다.
    /// </summary>
    private Transform base_manger_obj = null;

    /// <summary>
    /// Base_Manager 오브젝트 산하에 위치할수 있도록 합니다. (하이어라키 정리)
    /// </summary>
    /// <param name="T"></param>
    public void Initialize(Transform T)
    {
        base_manger_obj = T;
    }
    
    public IPool Pooling_OBJ (string path)
    {
        // 딕셔너리 키를 검사해서 path 키가 없으면, Pool 오브젝트를 추가시킵니다.
        if (m_pool_Dictionary.ContainsKey(path) == false)
        {
            Add_Pool(path);
        }

        //딕셔너리 키가 존재하지만, Queue의 카운트가 0이면, 새로운 오브젝트를 넣을 Queue를 추가합니다.
        if (m_pool_Dictionary[path].pool.Count <= 0)
        {
            Add_Queue(path);
        }

        return m_pool_Dictionary[path]; // IPool 인터페이스를 반환
    }

    /// <summary>
    /// 새로운 Pool을 생성합니다.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private GameObject Add_Pool(string path)
    {
        GameObject obj = new GameObject(path + "@POOL");
        obj.transform.SetParent(base_manger_obj);
        Object_Pool T_Component = new Object_Pool();

        m_pool_Dictionary.Add(path, T_Component);

        T_Component.parentTransform = obj.transform;

        return obj;
    }

    /// <summary>
    /// 새로운 오브젝트를 생성하고, 리턴하여, Queue에 집어넣습니다.
    /// </summary>
    /// <param name="path"></param>
    private void Add_Queue(string path)
    {
        var go = Base_Manager.instance.Instantiate_Path(path);

        if (go == null)
        {
            Debug.LogError($"[Pool_Manager] {path} 경로의 오브젝트 생성 실패");
            return;
        }

        go.transform.parent = m_pool_Dictionary[path].parentTransform;
        m_pool_Dictionary[path].Return(go);
    }

    public void Clear_Pool()
    {
        foreach (var poolEntry in m_pool_Dictionary)
        {
            var pool = poolEntry.Value;

            while (pool.pool.Count > 0)
            {
                var obj = pool.pool.Dequeue();
                if (obj != null && !obj.Equals(null))
                {
                    UnityEngine.Object.Destroy(obj);
                }
            }

            if (pool.parentTransform != null && !pool.parentTransform.Equals(null))
            {
                UnityEngine.Object.Destroy(pool.parentTransform.gameObject);
            }
        }

        Base_Canvas.instance.All_Layer_Destroy();
        m_pool_Dictionary.Clear();
    }

}
