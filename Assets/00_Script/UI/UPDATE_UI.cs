using UnityEngine;

public class UPDATE_UI : MonoBehaviour
{
    private const string PlayStoreLink = "market://details?id=패키지 네임";
    private const string AppStoreLink = "itms-apps://itunes.apple.com/app/앱ID";

    public void Exit_Game()
    {
        Application.Quit();
    }

    public void Open_Update_Link()
    {
        Application.OpenURL(PlayStoreLink);
    }
    
}
