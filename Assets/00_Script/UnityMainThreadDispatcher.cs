using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> _executionQueue = new Queue<Action>();

    private static UnityMainThreadDispatcher _instance = null;

    public static bool Exists() => _instance != null;

    public static UnityMainThreadDispatcher Instance()
    {
        if (!Exists())
        {
            // ã��
            _instance = FindObjectOfType<UnityMainThreadDispatcher>();

            // ������ ����
            if (_instance == null)
            {
                var obj = new GameObject("UnityMainThreadDispatcher");
                _instance = obj.AddComponent<UnityMainThreadDispatcher>();
                DontDestroyOnLoad(obj);
            }
        }
        return _instance;
    }

    void Update()
    {
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {
                _executionQueue.Dequeue().Invoke();
            }
        }
    }

    /// <summary>
    /// ���� ������� ���� ����
    /// </summary>
    public void Enqueue(Action action)
    {
        if (action == null)
            return;

        lock (_executionQueue)
        {
            _executionQueue.Enqueue(action);
        }
    }

    /// <summary>
    /// �ڷ�ƾ�� ���� �����忡�� ���� ����
    /// </summary>
    public void Enqueue(IEnumerator action)
    {
        Enqueue(() => StartCoroutine(action));
    }
}