using BackEnd;
using BackndChat;
using System.Collections.Generic;
using UnityEngine;

public class Chat_Manager : MonoBehaviour, IChatClientListener
{
    public static Chat_Manager instance;
    private ChatClient chatClient;

    private const string SERVER_GROUP_NAME = "global";
    private const string CHANNEL_NAME = "server-1";
    private const int CHANNEL_NUMBER = 1;

    private Queue<MessageInfo> cachedMessages = new Queue<MessageInfo>();
    private List<MessageInfo> cachedInitialMessages = new List<MessageInfo>();

    public bool IsConnected => chatClient != null;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        chatClient = new ChatClient(this, new ChatClientArguments());
        chatClient.SendJoinOpenChannel(SERVER_GROUP_NAME, CHANNEL_NAME);
    }

    private void Update()
    {
        chatClient?.Update();
    }
    public void SendUserMessage(string content)
    {
        if (chatClient == null) return;
        chatClient.SendChatMessage(SERVER_GROUP_NAME, CHANNEL_NAME, CHANNEL_NUMBER, content);
    }
    public void LeaveChannel()
    {
        chatClient?.SendLeaveChannel(SERVER_GROUP_NAME, CHANNEL_NAME, CHANNEL_NUMBER);
        cachedInitialMessages.Clear();
    }

    public void RejoinChannel()
    {
        chatClient?.SendJoinOpenChannel(SERVER_GROUP_NAME, CHANNEL_NAME);
    }

    public void SendSystemMessage(string content)
    {
        if (chatClient == null) return;

        var nickName = Backend.UserNickName;
        string formatted = $"[SYSTEM] <color=#FFFF00>{nickName}</color>���� {content}";
        chatClient.SendChatMessage(SERVER_GROUP_NAME, CHANNEL_NAME, CHANNEL_NUMBER, formatted);
    }

    public void OnChatMessage(MessageInfo messageInfo)
    {
        var uiChat = GameObject.Find("@Chat")?.GetComponent<UI_Chat>();

        if (uiChat != null && uiChat.isActiveAndEnabled)
        {
            uiChat.EnqueueMessage(messageInfo);
        }
        else
        {
            cachedMessages.Enqueue(messageInfo);
        }
    }

    public void FlushCachedMessages()
    {
        var uiChat = GameObject.Find("@Chat")?.GetComponent<UI_Chat>();
        if (uiChat == null) return;

        while (cachedMessages.Count > 0)
        {
            uiChat.EnqueueMessage(cachedMessages.Dequeue());
        }
    }

    public List<MessageInfo> GetInitialMessages()
    {
        return cachedInitialMessages;
    }

    // ��Ÿ �������̽� �޼���� (�α׿�)
    public void OnJoinChannel(ChannelInfo channelInfo)
    {
        Debug.Log($"[ChatManager] ä�� ���� �Ϸ�: {channelInfo.ChannelGroup}/{channelInfo.ChannelName}");
        cachedInitialMessages = new List<MessageInfo>(channelInfo.Messages);

        // UI_Chat�� �����ִٸ� ��� �޽��� ����
        var uiChat = GameObject.Find("@Chat")?.GetComponent<UI_Chat>();
        if (uiChat != null && uiChat.isActiveAndEnabled)
        {
            uiChat.ReceiveInitialMessages(cachedInitialMessages);
            FlushCachedMessages();
        }
    }
    public void OnJoinChannelPlayer(string group, string name, ulong number, PlayerInfo player) { }
    public void OnLeaveChannel(ChannelInfo channelInfo) { }
    public void OnLeaveChannelPlayer(string group, string name, ulong number, PlayerInfo player) { }
    public void OnSuccess(SUCCESS_MESSAGE success, object param) { }
    public void OnTranslateMessage(List<MessageInfo> messages) { }
    public void OnUpdatePlayerInfo(string group, string name, ulong number, PlayerInfo player) { }
    public void OnChangeGamerName(string oldName, string newName) { }
    public void OnDeleteMessage(MessageInfo msg) { }
    public void OnError(ERROR_MESSAGE error, object param) => Debug.LogError($"[ChatManager] ����: {error}");
    public void OnHideMessage(MessageInfo msg) { }
    public void OnWhisperMessage(WhisperMessageInfo msg) { }
}
