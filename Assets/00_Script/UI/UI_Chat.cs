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

    private GameObject chatPanelPrefab;

    private Queue<MessageInfo> messageQueue = new Queue<MessageInfo>();
    private bool isProcessing = false;

    private void Start()
    {
        ChatClient = new ChatClient(this, new ChatClientArguments
        {
            
        });

        chatPanelPrefab = Resources.Load<GameObject>("PreFabs/ChatList_Panel");
    }

    private void Update()
    {
        ChatClient?.Update();
    }

    public void SendChatMessage()
    {

        if (ChatInput == null || ChatClient == null || string.IsNullOrEmpty(ChatInput.text)) return;

        string text = ChatInput.text;
        ChatInput.text = string.Empty;

        ChatClient.SendChatMessage(SERVER_GROUP_NAME, CHANNEL_NAME, CHANNEL_NUMBER, text);
    }

    IEnumerator ScrollToBottom()
    {
        yield return null; // �� ������ ��� (���̾ƿ� ���� �ð�)
        ScrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
    }
    public void OnChatMessage(MessageInfo messageInfo)
    {
        messageQueue.Enqueue(messageInfo);
        if (!isProcessing)
            StartCoroutine(ProcessMessages());
    }
    private IEnumerator ProcessMessages()
    {
        isProcessing = true;

        while (messageQueue.Count > 0)
        {
            MessageInfo message = messageQueue.Dequeue();
            yield return StartCoroutine(HandleChatMessage(message));
        }

        isProcessing = false;
    }
    private IEnumerator HandleChatMessage(MessageInfo messageInfo)
    {
        string _rank = "UnRank";
        Color rankColor = Color.gray;
        Sprite tierSprite = Utils.Get_Atlas("Tier_Bronze");

        bool done = false;
        string inDate = null;

        Backend.Social.GetUserInfoByNickName(messageInfo.GamerName, (userCallback) =>
        {
            if (userCallback.IsSuccess())
            {
                inDate = userCallback.GetInDate();
            }
            done = true;
        });

        yield return new WaitUntil(() => done);
        done = false;

        if (!string.IsNullOrEmpty(inDate))
        {
            Backend.URank.User.GetUserRank(Utils.LEADERBOARD_UUID, inDate, (rankCallback) =>
            {
                if (rankCallback.IsSuccess())
                {
                    var json = rankCallback.GetFlattenJSON();
                    if (json["rows"].Count > 0)
                    {
                        string rawRank = json["rows"][0]["rank"].ToString();
                        if (int.TryParse(rawRank, out int parsedRank))
                        {
                            if (parsedRank >= 1 && parsedRank <= 10)
                            {
                                _rank = parsedRank.ToString();
                                rankColor = Color.yellow;
                                tierSprite = Utils.Get_Atlas(parsedRank.ToString());
                            }
                            else
                            {
                                _rank = "UnRank"; // 11�� �̻� UnRank ó��
                            }
                        }
                    }
                }
                done = true;
            });

            yield return new WaitUntil(() => done);
        }

        CreateChatItem(messageInfo, _rank, rankColor, tierSprite);
    }
    private void CreateChatItem(MessageInfo messageInfo, string rank, Color color, Sprite sprite)
    {
        GameObject chatItem = Instantiate(chatPanelPrefab, ChatContent.transform);
        chatItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = messageInfo.Message;
        chatItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = rank == "UnRank" ? "[UnRank]" : $"[��ŷ {rank}��]";
        chatItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = color;
        chatItem.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = messageInfo.GamerName;
        chatItem.transform.GetChild(4).GetComponent<Image>().sprite = sprite;

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
