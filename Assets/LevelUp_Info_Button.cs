using UnityEngine;

public class LevelUp_Info_Button : MonoBehaviour
{
    [SerializeField]
    private GameObject Level_Up_Info_Title;


    private void Start()
    {
        Level_Up_Info_Title.gameObject.SetActive(false);
    }
    public void Level_up_Info()
    {
        if (Level_Up_Info_Title.activeSelf)
        {
            Level_Up_Info_Title.gameObject.SetActive(false);
        }

        else
        {
            Level_Up_Info_Title.gameObject.SetActive(true);
        }
    }
}
