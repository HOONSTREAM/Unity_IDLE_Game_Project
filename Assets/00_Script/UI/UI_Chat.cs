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
    [SerializeField] private GameObject ChatContent;
    [SerializeField] private GameObject ScrollView;
    [SerializeField] private TMP_InputField ChatInput;
    [SerializeField] private Button Send_Button;
    [SerializeField] private Button RePort_Button;

    private ChatClient ChatClient;
    private const string SERVER_GROUP_NAME = "global";
    private const string CHANNEL_NAME = "server-1";
    private const int CHANNEL_NUMBER = 1;

    private GameObject chatPanelPrefab;
    private Queue<MessageInfo> messageQueue = new Queue<MessageInfo>();
    private Queue<GameObject> chatItemPool = new Queue<GameObject>();
    private Dictionary<string, List<GameObject>> userChatItems = new Dictionary<string, List<GameObject>>();
    private bool isProcessing = false;

    private void Start()
    {
        ChatClient = new ChatClient(this, new ChatClientArguments());
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
    private GameObject GetChatItem()
    {
        if (chatItemPool.Count > 0)
            return chatItemPool.Dequeue();
        else
            return Instantiate(chatPanelPrefab);
    }
    private void ReturnChatItem(GameObject item)
    {
        item.SetActive(false);
        item.transform.SetParent(null);
        chatItemPool.Enqueue(item);
    }
    private IEnumerator ScrollToBottom()
    {
        yield return null;
        Canvas.ForceUpdateCanvases();
        ScrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
        yield return null;
    }
    public void OnChatMessage(MessageInfo messageInfo)
    {
        messageQueue.Enqueue(messageInfo);
        if (!isProcessing)
            StartCoroutine(ProcessMessagesBatch());
    }
    private IEnumerator ProcessMessagesBatch()
    {
        isProcessing = true;

        yield return StartCoroutine(ScrollToBottom());

        while (messageQueue.Count > 0)
        {
            var msg = messageQueue.Dequeue();
            yield return StartCoroutine(HandleChatMessage(msg)); 
        }

        yield return StartCoroutine(ScrollToBottom());
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
                inDate = userCallback.GetInDate();
            done = true;
        });
        yield return new WaitUntil(() => done);

        if (!string.IsNullOrEmpty(inDate))
        {
            done = false;
            Backend.URank.User.GetUserRank(Utils.LEADERBOARD_UUID, inDate, (rankCallback) =>
            {
                if (rankCallback.IsSuccess())
                {
                    var json = rankCallback.GetFlattenJSON();
                    if (json["rows"].Count > 0)
                    {
                        string rawRank = json["rows"][0]["rank"].ToString();
                        if (int.TryParse(rawRank, out int parsedRank) && parsedRank <= 10)
                        {
                            _rank = parsedRank.ToString();
                            rankColor = Color.yellow;
                            tierSprite = Utils.Get_Atlas(parsedRank.ToString());
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
        GameObject chatItem = GetChatItem();
        chatItem.transform.SetParent(ChatContent.transform, false);
        chatItem.SetActive(true);

        chatItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = messageInfo.Message;
        chatItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = rank == "UnRank" ? "[UnRank]" : $"[랭킹 {rank}위]";
        chatItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = color;
        chatItem.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = messageInfo.GamerName;
        chatItem.transform.GetChild(4).GetComponent<Image>().sprite = sprite;

        // 캐싱
        if (!userChatItems.ContainsKey(messageInfo.GamerName))
            userChatItems[messageInfo.GamerName] = new List<GameObject>();

        userChatItems[messageInfo.GamerName].Add(chatItem);
    }
    public override void DisableOBJ()
    {
        ChatClient?.Dispose();
        base.DisableOBJ();
    }
    public void OnJoinChannel(ChannelInfo channelInfo)
    {
        Debug.Log($"채널 입장 완료: {channelInfo.ChannelGroup}/{channelInfo.ChannelName}");

        StartCoroutine(ProcessInitialMessagesBatch(channelInfo.Messages));

        foreach (var player in channelInfo.Players)
            Debug.Log($"접속중: {player.Value.GamerName}");
    }
    private IEnumerator ProcessInitialMessagesBatch(List<MessageInfo> messages)
    {
        isProcessing = true;

        int batchSize = 10;
        int counter = 0;

        foreach (var msg in messages)
        {
            HandleChatMessage_Lite(msg);
            counter++;
            if (counter >= batchSize)
            {
                counter = 0;
                yield return null;
            }
        }

        yield return new WaitForEndOfFrame();
        StartCoroutine(ScrollToBottom());

        // 이후 랭크 정보 갱신
        StartCoroutine(UpdateRanksAfterLoad());

        isProcessing = false;
    }
    private IEnumerator UpdateRanksAfterLoad()
    {
        foreach (var kvp in userChatItems)
        {
            string gamerName = kvp.Key;
            List<GameObject> items = kvp.Value;

            string inDate = null;
            bool done = false;

            Backend.Social.GetUserInfoByNickName(gamerName, (callback) =>
            {
                if (callback.IsSuccess())
                    inDate = callback.GetInDate();
                done = true;
            });

            yield return new WaitUntil(() => done);

            if (!string.IsNullOrEmpty(inDate))
            {
                done = false;
                Backend.URank.User.GetUserRank(Utils.LEADERBOARD_UUID, inDate, (callback) =>
                {
                    if (callback.IsSuccess())
                    {
                        var json = callback.GetFlattenJSON();
                        if (json["rows"].Count > 0)
                        {
                            string rawRank = json["rows"][0]["rank"].ToString();
                            if (int.TryParse(rawRank, out int parsedRank) && parsedRank <= 10)
                            {
                                // 각 아이템 UI에 적용
                                foreach (var item in items)
                                {
                                    item.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"[랭킹 {parsedRank}위]";
                                    item.transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.yellow;
                                    item.transform.GetChild(4).GetComponent<Image>().sprite = Utils.Get_Atlas(parsedRank.ToString());
                                }
                            }
                        }
                    }
                    done = true;
                });

                yield return new WaitUntil(() => done);
            }
        }
    }
    private void HandleChatMessage_Lite(MessageInfo messageInfo)
    {
        string _rank = "UnRank";
        Color rankColor = Color.gray;
        Sprite tierSprite = Utils.Get_Atlas("Tier_Bronze");

        CreateChatItem(messageInfo, _rank, rankColor, tierSprite);
    }
    public void OnJoinChannelPlayer(string group, string name, ulong number, PlayerInfo player) => Debug.Log($"입장: {player.GamerName}");
    public void OnLeaveChannel(ChannelInfo channelInfo) { }
    public void OnLeaveChannelPlayer(string group, string name, ulong number, PlayerInfo player) { }
    public void OnSuccess(SUCCESS_MESSAGE success, object param) { }
    public void OnTranslateMessage(List<MessageInfo> messages) { }
    public void OnUpdatePlayerInfo(string group, string name, ulong number, PlayerInfo player) { }
    public void OnChangeGamerName(string oldName, string newName) { }
    public void OnDeleteMessage(MessageInfo msg) { }
    public void OnError(ERROR_MESSAGE error, object param) => Debug.LogError($"채팅 에러 발생: {error}");
    public void OnHideMessage(MessageInfo msg) { }
    public void OnWhisperMessage(WhisperMessageInfo msg) { }

    private void OnApplicationQuit() => ChatClient?.Dispose();
}
