using BackEnd;
using GoogleMobileAds.Ump.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BackEnd_PostSystem : MonoBehaviour
{
    [System.Serializable]
    public class PostEvent : UnityEvent<List<PostData>> { }
    public PostEvent onGetPostListEvent = new PostEvent();


    private List<PostData> postList = new List<PostData>();

    private void Start()
    {       
        PostListGet(PostType.Admin);
        DontDestroyOnLoad(this);
    }

    public void All_ReCeive_Button()
    {
        PostReceiveAll(PostType.Admin);
    }
    public void PostListGet(PostType postType)
    {
        Backend.UPost.GetPostList(postType, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"우편 불러오기 중, 에러가 발생하였습니다 : {callback}");
            }

            else
            {
                Debug.Log($"우편 불러오기에 성공하였습니다 : {callback}");


                try
                {
                    LitJson.JsonData jsonData = callback.GetFlattenJSON()["postList"];

                    if(jsonData.Count <= 0)
                    {
                        Debug.LogWarning("우편함이 비어있습니다.");
                        return;
                    }

                    postList.Clear();

                    //현재 저장 가능한 모든 우편 정보 불러오기

                    for(int i = 0; i < jsonData.Count; i++)
                    {
                        PostData post = new PostData();
                        post.Title = jsonData[i]["title"].ToString();
                        post.content = jsonData[i]["content"].ToString();
                        post.inDate = jsonData[i]["inDate"].ToString();
                        post.expiration_Date = jsonData[i]["expirationDate"].ToString();

                        // 우편에 발송된 모든 아이템 정보 불러오기

                        foreach(LitJson.JsonData itemJson in jsonData[i]["items"])

                        {
                            if (itemJson.ContainsKey("chartName") &&
                                itemJson["chartName"].ToString() == Base_Manager.instance.GOODS_CHART_NAME)
                            {
                                
                                string itemName = itemJson["item"]["ItemName"].ToString();
                                int itemCount = int.Parse(itemJson["itemCount"].ToString());

                                if (post.post_reward.ContainsKey(itemName))
                                {
                                    post.post_reward[itemName] += itemCount;
                                }

                                else
                                {
                                    post.post_reward.Add(itemName, itemCount); 
                                }

                                post.isCanReceive = true;
                            }
                            else
                            {
                                Debug.LogWarning($"아직 지원하지 않는 차트 정보 입니다 : {jsonData[i]["chartName"].ToString()}");
                                post.isCanReceive = false;
                            }
                        }

                        postList.Add(post);

                    }

                    // 우편 리스트 불러오기가 완료되었을 때, 이벤트 메소드 호출
                    onGetPostListEvent?.Invoke(postList);

                    // 저장 가능한 우편 정보 모두 출력

                    for(int i = 0; i< postList.Count; i++)
                    {
                        Debug.Log($"{i}번째 우편\n {postList[i].ToString()}");
                    }
                }
                catch(System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
        });
    }
    public void PostReceive(PostType postType, int index)
    {
        if(postList.Count <= 0)
        {
            Debug.LogWarning("받을 수 있는 우편이 존재하지 않습니다. 혹은 우편 리스트 불러오기를 먼저 호출해주세요.");
            return;
        }

        if(index >= postList.Count)
        {
            Debug.LogError($"해당 우편은 존재하지 않습니다. : 요청 : index : {index} / 우편 최대 갯수 :{postList.Count}");
            return;
        }

        Debug.Log($"{postType.ToString()}의 {postList[index].inDate} 우편 수령을 요청합니다.");


        Backend.UPost.ReceivePostItem(postType, postList[index].inDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"{postType.ToString()}의 {postList[index].inDate} 우편 수령에 에러가 발생하였습니다. : {callback}");
            }

            Debug.Log($"{postType.ToString()}의 {postList[index].inDate} 우편 수령에 성공하였습니다. : {callback}");
            postList.RemoveAt(index);

            if (callback.GetFlattenJSON()["postItems"].Count > 0)
            {
                SavePostToLocal(callback.GetFlattenJSON()["postItems"]);

                _=Base_Manager.BACKEND.WriteData();
            }

            else
            {
                Debug.LogWarning("수령 가능한 우편이 존재하지 않습니다.");
            }
        });


    }
    public void PostReceive(PostType postType, string inDate)
    {
        PostReceive(postType, postList.FindIndex(item => item.inDate.Equals(inDate)));
    }
    public void PostReceiveAll(PostType postType)
    {
        if(postList.Count <= 0)
        {
            Debug.LogWarning("받을 수 있는 우편이 존재하지 않습니다. 혹은 우편 리스트 불러오기를 먼저 호출해주세요.");
            return;
        }

        Debug.Log($"{postType.ToString()} 우편 전체 수령을 요청합니다.");

        Backend.UPost.ReceivePostItemAll(postType, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"{postType.ToString()} 우편 전체 수령 중 에러가 발생했습니다. : {callback}");
                return;
            }

            Debug.Log($"우편 전체 수령에 성공하였습니다. : {callback}");

            postList.Clear(); // 모든 우편을 수령하여서, postList는 초기화한다.

            foreach(LitJson.JsonData postItemsJson in callback.GetFlattenJSON()["postItems"])
            {
                SavePostToLocal(postItemsJson);
            }

            _ = Base_Manager.BACKEND.WriteData();

        });
        
    }
    public void SavePostToLocal(LitJson.JsonData item)
    {
        try
        {
            foreach(LitJson.JsonData itemJson in item)
            {
                // 차트 파일 이름(*.xlsx) 와 Backend Console 에 등록한 차트 이름
                string chartFileName = itemJson["item"]["chartFileName"].ToString();
                string chartName = itemJson["chartName"].ToString();

                //GoodsChart.xlsx 에 등록한 첫번째 행 이름

                int ItemId = int.Parse(itemJson["item"]["ItemId"].ToString());
                string itemName = itemJson["item"]["ItemName"].ToString();
                string itemInfo = itemJson["item"]["ItemInfo"].ToString();

                // 우편 발송 할 때 , 작성하는 아이템 수량

                int itemCount = int.Parse(itemJson["itemCount"].ToString());

                // 우편으로 받은 재화를 게임 내 데이터에 적용
                // 여러 재화가 생기면, 여기를 업데이트 해야합니다!
                if (chartName.Equals(Base_Manager.instance.GOODS_CHART_NAME))
                {
                    if (itemName.Equals("Dia"))
                    {
                        Data_Manager.Main_Players_Data.DiaMond += itemCount;
                        Base_Manager.BACKEND.Log_Get_Dia("Post");
                    }
                }

                Debug.Log($"{chartName} - {chartFileName}");
                Debug.Log($"{ItemId} {itemName} : {itemInfo}, 획득 수량 : {itemCount}");
                Debug.Log($"아이템을 수령했습니다. : {itemName} - {itemCount}개");

            }
        }

        catch(System.Exception e)
        {
            Debug.LogError(e);
        }
    }


}
