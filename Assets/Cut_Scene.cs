using UnityEngine;
using UnityEngine.SceneManagement;

public class Cut_Scene : MonoBehaviour
{
    [SerializeField] private GameObject Cut_1_Scene;
    [SerializeField] private GameObject Cut_2_Scene;
    [SerializeField] private GameObject Cut_3_Scene;

    private int cutIndex = 0;

    void Start()
    {
        Base_Manager.SOUND.Play(Sound.BGM, "prologue");
        Cut_1_Scene.SetActive(true);
        Cut_2_Scene.SetActive(false);
        Cut_3_Scene.SetActive(false);
    }

    void Update()
    {
        // 마우스 클릭 또는 터치
        if (Input.GetMouseButtonDown(0))
        {
            cutIndex++;

            switch (cutIndex)
            {
                case 1:
                    Base_Manager.SOUND.Play(Sound.BGS, "Click_12");
                    Cut_2_Scene.SetActive(true);
                    break;
                case 2:
                    Base_Manager.SOUND.Play(Sound.BGS, "Click_12");
                    Cut_3_Scene.SetActive(true);
                    break;
                case 3:
                    Loading_Scene.instance.Main_Game_Start();
                    break;
            }
        }
    }
}
