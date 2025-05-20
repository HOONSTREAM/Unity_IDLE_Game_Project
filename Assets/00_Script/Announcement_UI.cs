using UnityEngine;

public class Announcement_UI : MonoBehaviour
{
    [SerializeField]
    private GameObject Announcement_Text_UI;
    [SerializeField]
    private GameObject Update_Text_UI;
    [SerializeField]
    private GameObject Book_Update_Text_UI;


    void Start()
    {
        Announcement_Text_UI.gameObject.SetActive(true);
        Update_Text_UI.gameObject.SetActive(false);
        Book_Update_Text_UI.gameObject.SetActive(false);

    }

    public void Announcement_Button()
    {
        Base_Manager.SOUND.Play(Sound.BGS, "Click_12");
        Announcement_Text_UI.gameObject.SetActive(false);
        Update_Text_UI.gameObject.SetActive(false);
        Book_Update_Text_UI.gameObject.SetActive(false);

        Announcement_Text_UI.gameObject.SetActive(true);
    }
    public void Update_Button()
    {
        Base_Manager.SOUND.Play(Sound.BGS, "Click_12");
        Announcement_Text_UI.gameObject.SetActive(false);
        Update_Text_UI.gameObject.SetActive(false);
        Book_Update_Text_UI.gameObject.SetActive(false);

        Update_Text_UI.gameObject.SetActive(true);
    }
    public void Book_Update_Button()
    {
        Base_Manager.SOUND.Play(Sound.BGS, "Click_12");
        Announcement_Text_UI.gameObject.SetActive(false);
        Update_Text_UI.gameObject.SetActive(false);
        Book_Update_Text_UI.gameObject.SetActive(false);

        Book_Update_Text_UI.gameObject.SetActive(true);
    }


}
