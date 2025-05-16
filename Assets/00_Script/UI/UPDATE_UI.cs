using UnityEngine;

public class UPDATE_UI : MonoBehaviour
{
    private const string PlayStoreLink = "https://play.google.com/store/apps/details?id=com.LiaChen.IDLEGAME";
    private const string AppStoreLink = "itms-apps://itunes.apple.com/app/¾ÛID";

    public void Exit_Game()
    {
        Application.Quit();
    }

    public void Open_Update_Link()
    {
        Application.OpenURL(PlayStoreLink);
    }
    
}
