using UnityEngine;

public sealed class Coroutine_Runner : MonoBehaviour
{
    static Coroutine_Runner _instance;
    public static Coroutine_Runner Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("[Coroutine_Runner]");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<Coroutine_Runner>();
            }
            return _instance;
        }
    }
}