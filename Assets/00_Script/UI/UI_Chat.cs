using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackndChat;
using UnityEngine.UI;
using System;
using TMPro;

public class UI_Chat : UI_Base, BackndChat.IChatClientListener
{
    [SerializeField]
    private GameObject ChatContent;
    [SerializeField]
    private GameObject ScrollView;
    [SerializeField]
    private TMP_InputField ChatInput;
    [SerializeField]
    private Button Send_Button;
    [SerializeField]
    private Button RePort_Button;


    private BackndChat.ChatClient ChatClient = null;

    private const string SERVER_GROUP_NAME = "global";
    private const string CHANNEL_NAME = "server-1";
    private const int CHANNEL_NUMBER = 1;

    private void Start()
    {
        ChatClient = new ChatClient(this, new ChatClientArguments());
        
    }

    private void Update()
    {
        ChatClient?.Update();
    }

    public void SendChatMessage()
    {
        
        if (ChatInput == null) return;

        if (ChatInput.text.Length == 0) return;

        string text = ChatInput.text;

        ChatInput.text = string.Empty;

        if (string.IsNullOrEmpty(text)) return;

        if (ChatClient == null) return;

        string channelGroup = SERVER_GROUP_NAME;
        string channelName = CHANNEL_NAME;
        ulong channelNumber = CHANNEL_NUMBER;

        ChatClient.SendChatMessage(channelGroup, channelName, channelNumber, text);

    }

    IEnumerator ScrollToBottom()
    {
        yield return null; // �� ������ ��� (���̾ƿ� ���� �ð�)
        ScrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
    }
    public void OnChatMessage(MessageInfo messageInfo)
    {
        string _rank = string.Empty;
        var userBro = Backend.Social.GetUserInfoByNickName(messageInfo.GamerName);
        string inDate = userBro.GetInDate();
        var leader_board_bro = Backend.Leaderboard.User.GetLeaderboard(Utils.LEADERBOARD_UUID);
        var rank = Backend.URank.User.GetUserRank(Utils.LEADERBOARD_UUID, inDate);

        if (rank.IsSuccess())
        {
            LitJson.JsonData rankListJson = rank.GetFlattenJSON();
            string extraName = string.Empty;

            for (int i = 0; i < rankListJson["rows"].Count; i++)
            {
                _rank = rankListJson["rows"][i]["rank"].ToString();
            }

            GameObject chatItem = Instantiate(Resources.Load<GameObject>("PreFabs/ChatList_Panel"), ChatContent.transform);
            chatItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{messageInfo.Message}";
            chatItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"[��ŷ {_rank.ToString()}��]";

            int parsedRank;

            if (int.TryParse(_rank, out parsedRank) && parsedRank >= 1 && parsedRank <= 10)
            {
                chatItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.yellow;
            }
            else
            {
                chatItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.gray;
            }

            chatItem.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = $"{messageInfo.GamerName}";
            chatItem.transform.GetChild(4).GetComponent<Image>().sprite = Utils.Get_Atlas(parsedRank.ToString()); // ��ũ �̹���

            if (int.Parse(_rank) >= 10)
            {
                chatItem.transform.GetChild(4).GetComponent<Image>().sprite = Utils.Get_Atlas("Bronze");
            }

        }

        else
        {
            GameObject chatItem = Instantiate(Resources.Load<GameObject>("PreFabs/ChatList_Panel"), ChatContent.transform);
            chatItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{messageInfo.Message}";
            chatItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"[UnRank]";
            chatItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.gray;
            chatItem.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = $"{messageInfo.GamerName}";
            chatItem.transform.GetChild(4).GetComponent<Image>().sprite = Utils.Get_Atlas("Bronze"); // ��ũ �̹���
        }

            StartCoroutine(ScrollToBottom());

    }

    public override void DisableOBJ()
    {
        ChatClient?.Dispose();
        base.DisableOBJ();
    }

    public void OnChangeGamerName(string oldGamerName, string newGamerName)
    {
        throw new System.NotImplementedException();
    }

    public void OnDeleteMessage(MessageInfo messageInfo)
    {
        throw new System.NotImplementedException();
    }

    public void OnError(ERROR_MESSAGE error, object param)
    {
        Debug.LogError($"ä�� ���� �߻�: {error}");
    }

    public void OnHideMessage(MessageInfo messageInfo)
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinChannel(BackndChat.ChannelInfo channelInfo)
    {
        Debug.Log($"ä�� ���� �Ϸ�: {channelInfo.ChannelGroup}/{channelInfo.ChannelName}");

        foreach (var msg in channelInfo.Messages)
        {
            OnChatMessage(msg);
            StartCoroutine(ScrollToBottom());
        }

        // ���� �������� ���� Ȯ��
        foreach (var player in channelInfo.Players)
        {
            Debug.Log($"������: {player.Value.GamerName}");
        }

    }

    public void OnJoinChannelPlayer(string channelGroup, string channelName, ulong channelNumber, PlayerInfo player)
    {
        Debug.Log($"[ä�� ����] {player.GamerName} �� {channelGroup}/{channelName} ä�ο� �����߽��ϴ�.");
    }

    public void OnLeaveChannel(ChannelInfo channelInfo)
    {
        
    }

    public void OnLeaveChannelPlayer(string channelGroup, string channelName, ulong channelNumber, PlayerInfo player)
    {
       
    }

    public void OnSuccess(SUCCESS_MESSAGE success, object param)
    {
        throw new System.NotImplementedException();
    }

    public void OnTranslateMessage(List<MessageInfo> messages)
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdatePlayerInfo(string channelGroup, string channelName, ulong channelNumber, PlayerInfo player)
    {
        throw new System.NotImplementedException();
    }

    private void OnApplicationQuit()
    {
        ChatClient?.Dispose();
    }

    public void OnWhisperMessage(WhisperMessageInfo messageInfo)
    {
        throw new NotImplementedException();
    }
}
