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

    public void OnChatMessage(MessageInfo messageInfo)
    {
        GameObject chatItem = Instantiate(Resources.Load<GameObject>("PreFabs/ChatList_Panel"), ChatContent.transform);
        chatItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{messageInfo.Message}";
        chatItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"[랭킹 1위]";
        chatItem.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = $"{messageInfo.GamerName}";
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
        Debug.LogError($"채팅 에러 발생: {error}");
    }

    public void OnHideMessage(MessageInfo messageInfo)
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinChannel(BackndChat.ChannelInfo channelInfo)
    {
        Debug.Log($"채널 입장 완료: {channelInfo.ChannelGroup}/{channelInfo.ChannelName}");

        foreach (var msg in channelInfo.Messages)
        {
            OnChatMessage(msg);
        }

        // 현재 접속중인 유저 확인
        foreach (var player in channelInfo.Players)
        {
            Debug.Log($"접속중: {player.Value.GamerName}");
        }

    }

    public void OnJoinChannelPlayer(string channelGroup, string channelName, ulong channelNumber, PlayerInfo player)
    {
        Debug.Log($"[채널 입장] {player.GamerName} 이 {channelGroup}/{channelName} 채널에 입장했습니다.");
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
