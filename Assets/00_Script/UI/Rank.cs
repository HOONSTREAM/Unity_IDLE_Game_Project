using TMPro;
using UnityEngine;

public class Rank : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Rank_Text;
    [SerializeField] private TextMeshProUGUI nick_name_Text;
    [SerializeField] private TextMeshProUGUI RP_Text;

    private bool is_Stage = false;

    public void Bind(int rankIndex1Based, string nickname, long rp, bool is_Stage)
    {
        if (Rank_Text) Rank_Text.text = rankIndex1Based.ToString();
        if (nick_name_Text) nick_name_Text.text = string.IsNullOrEmpty(nickname) ? "-" : nickname;
        if (RP_Text && is_Stage) RP_Text.text = $"{rp:N0} Ãþ";
        if (RP_Text && !is_Stage) RP_Text.text = $"{rp:N0} ´Ü°è";
    }
}
